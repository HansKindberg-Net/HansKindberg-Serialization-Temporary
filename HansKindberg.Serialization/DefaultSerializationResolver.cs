using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Castle.DynamicProxy;

namespace HansKindberg.Serialization
{
	public class DefaultSerializationResolver : ISerializationResolver
	{
		#region Fields

		private static readonly IDictionary<Type, IEnumerable<FieldInfo>> _fieldsForSerializationCache = new Dictionary<Type, IEnumerable<FieldInfo>>();
		private static readonly object _lockObject = new object();
		private readonly IMemoryFormatter _memoryFormatter;
		private readonly IMemoryFormatterFactory _memoryFormatterFactory;
		private readonly IProxyBuilder _proxyBuilder;

		private static readonly Type[] _serializableBaseTypes = new[]
			{
				typeof(Serializable),
				typeof(ValueType)
			};

		private static readonly IDictionary<Type, bool> _serializableTypesCache = new Dictionary<Type, bool>();
		private readonly IList<SerializationFailure> _serializationFailures = new List<SerializationFailure>();

		private static readonly Type[] _unserializableBaseTypes = new[]
			{
				typeof(Delegate)
			};

		private static readonly Type[] _unserializableDeclaringTypes = new[]
			{
				typeof(ListDictionary)
			};

		private static readonly Type[] _unserializableTypes = new[]
			{
				typeof(HybridDictionary),
				typeof(ListDictionary),
				Type.GetType("System.Runtime.Remoting.Messaging.ServerObjectTerminatorSink", true),
				Type.GetType("System.Runtime.Remoting.Messaging.StackBuilderSink", true)
			};

		#endregion

		#region Constructors

		public DefaultSerializationResolver(IProxyBuilder proxyBuilder, IMemoryFormatterFactory memoryFormatterFactory)
		{
			if(proxyBuilder == null)
				throw new ArgumentNullException("proxyBuilder");

			if(memoryFormatterFactory == null)
				throw new ArgumentNullException("memoryFormatterFactory");

			this._memoryFormatter = memoryFormatterFactory.Create();
			this._memoryFormatterFactory = memoryFormatterFactory;
			this._proxyBuilder = proxyBuilder;
		}

		#endregion

		#region Properties

		public virtual bool DecideIfAnInstanceIsSerializableByActuallySerializingIt { get; set; }

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		protected internal virtual IDictionary<Type, IEnumerable<FieldInfo>> FieldsForSerializationCache
		{
			get { return _fieldsForSerializationCache; }
		}

		protected internal virtual IMemoryFormatter MemoryFormatter
		{
			get { return this._memoryFormatter; }
		}

		protected internal virtual IMemoryFormatterFactory MemoryFormatterFactory
		{
			get { return this._memoryFormatterFactory; }
		}

		protected internal virtual IProxyBuilder ProxyBuilder
		{
			get { return this._proxyBuilder; }
		}

		protected internal virtual IEnumerable<Type> SerializableBaseTypes
		{
			get { return _serializableBaseTypes; }
		}

		protected internal virtual IDictionary<Type, bool> SerializableTypesCache
		{
			get { return _serializableTypesCache; }
		}

		public virtual IList<SerializationFailure> SerializationFailures
		{
			get { return this._serializationFailures; }
		}

		protected internal virtual IEnumerable<Type> UnserializableBaseTypes
		{
			get { return _unserializableBaseTypes; }
		}

		protected internal virtual IEnumerable<Type> UnserializableDeclaringTypes
		{
			get { return _unserializableDeclaringTypes; }
		}

		protected internal virtual IEnumerable<Type> UnserializableTypes
		{
			get { return _unserializableTypes; }
		}

		#endregion

		#region Methods

		protected internal virtual Type ConvertUninitializedObjectType(Type type)
		{
			if(type == null)
				return null;

			if(type.IsInterface)
				return this.ProxyBuilder.CreateInterfaceProxyTypeWithoutTarget(type, null, ProxyGenerationOptions.Default);

			if(type.IsAbstract)
				return this.ProxyBuilder.CreateClassProxyType(type, null, ProxyGenerationOptions.Default);

			return type;
		}

		public virtual object CreateUninitializedObject(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			if(typeof(string).IsAssignableFrom(type))
				return string.Empty;

			try
			{
				return FormatterServices.GetUninitializedObject(this.ConvertUninitializedObjectType(type));
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "An uninitialized object of type \"{0}\" could not be created.", type.FullName), exception);
			}
		}

		public virtual IEnumerable<FieldInfo> GetFieldsForSerialization(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			// ReSharper disable PossibleMultipleEnumeration

			IEnumerable<FieldInfo> fields;

			if(!this.FieldsForSerializationCache.TryGetValue(type, out fields))
			{
				lock(_lockObject)
				{
					if(!this.FieldsForSerializationCache.TryGetValue(type, out fields))
					{
						fields = this.GetFieldsForSerializationInternal(type);
						this.FieldsForSerializationCache.Add(type, fields);
					}
				}
			}

			return fields;

			// ReSharper restore PossibleMultipleEnumeration
		}

		protected internal virtual IEnumerable<FieldInfo> GetFieldsForSerializationInternal(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			while(type != null)
			{
				foreach(FieldInfo field in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
				{
					yield return field;
				}

				type = type.BaseType;
			}
		}

		protected internal virtual IEnumerable<Type> GetGenericArgumentsRecursive(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			foreach(Type genericArgument in type.GetGenericArguments())
			{
				yield return genericArgument;

				foreach(Type childGenericArgument in this.GetGenericArgumentsRecursive(genericArgument))
				{
					yield return childGenericArgument;
				}
			}
		}

		public virtual bool IsSerializable(object instance)
		{
			if(instance == null)
				return true;

			if(this.DecideIfAnInstanceIsSerializableByActuallySerializingIt)
			{
				try
				{
					this.MemoryFormatter.Serialize(instance);

					return true;
				}
				catch(SerializationException originalSerializationException)
				{
					SerializationException serializationException = new SerializationException(string.Format(CultureInfo.InvariantCulture, "The type \"{0}\" could not be serialized.", instance.GetType().FullName), originalSerializationException);

					if(this.SerializationFailures.Count == int.MaxValue)
						this.SerializationFailures.RemoveAt(0);

					this.SerializationFailures.Add(new SerializationFailure {Type = instance.GetType(), SerializationException = serializationException});

					return false;
				}
			}

			return this.IsSerializable(instance.GetType());
		}

		protected internal virtual bool IsSerializable(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			bool isSerializable;

			if(!this.SerializableTypesCache.TryGetValue(type, out isSerializable))
			{
				lock(_lockObject)
				{
					if(!this.SerializableTypesCache.TryGetValue(type, out isSerializable))
					{
						isSerializable = this.IsSerializableInternal(type);
						this.SerializableTypesCache.Add(type, isSerializable);
					}
				}
			}

			return isSerializable;
		}

		protected internal virtual bool IsSerializableInternal(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			if(!type.IsSerializable)
				return false;

			if(this.SerializableBaseTypes.Any(serializableBaseType => serializableBaseType.IsAssignableFrom(type)))
				return true;

			if(this.UnserializableBaseTypes.Any(unserializableBaseType => unserializableBaseType.IsAssignableFrom(type)))
				return false;

			if(this.UnserializableTypes.Any(unserializableType => unserializableType == type))
				return false;

			Type declaringType = type.DeclaringType;

			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			if(declaringType != null && this.UnserializableDeclaringTypes.Any(unserializableDeclaringType => unserializableDeclaringType == declaringType))
				return false;
			// ReSharper restore ConditionIsAlwaysTrueOrFalse

			if(type.HasElementType)
			{
				Type elementType = type.GetElementType();

				if(elementType != null && (elementType == typeof(object) || !elementType.IsSerializable))
					return false;
			}

			if(typeof(IEnumerable).IsAssignableFrom(type))
			{
				if(typeof(ArrayList).IsAssignableFrom(type) || typeof(Hashtable).IsAssignableFrom(type))
					return false;
			}

			if(!this.GetGenericArgumentsRecursive(type).All(genericArgument => genericArgument.IsSerializable))
				return false;

			return true;
		}

		#endregion
	}
}