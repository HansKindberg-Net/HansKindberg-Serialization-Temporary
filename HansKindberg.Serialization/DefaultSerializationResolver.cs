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

		private static readonly IDictionary<Type, IEnumerable<FieldInfo>> _fieldsCache = new Dictionary<Type, IEnumerable<FieldInfo>>();
		//private static readonly IDictionary<Type, IEnumerable<Type>> _genericArgumentsRecursiveCache = new Dictionary<Type, IEnumerable<Type>>();
		private static readonly IDictionary<Type, bool> _isSerializableCache = new Dictionary<Type, bool>();
		private static readonly object _lockObject = new object();
		private readonly IProxyBuilder _proxyBuilder;

		#endregion

		#region Constructors

		public DefaultSerializationResolver(IProxyBuilder proxyBuilder)
		{
			if(proxyBuilder == null)
				throw new ArgumentNullException("proxyBuilder");

			this._proxyBuilder = proxyBuilder;
		}

		#endregion

		#region Properties

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static IDictionary<Type, IEnumerable<FieldInfo>> FieldsCache
		{
			get { return _fieldsCache; }
		}

		//[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		//public static IDictionary<Type, IEnumerable<Type>> GenericArgumentsRecursiveCache
		//{
		//	get { return _genericArgumentsRecursiveCache; }
		//}

		public static IDictionary<Type, bool> IsSerializableCache
		{
			get { return _isSerializableCache; }
		}

		protected internal virtual IProxyBuilder ProxyBuilder
		{
			get { return this._proxyBuilder; }
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

		public virtual IEnumerable<FieldInfo> GetFieldsToSerialize(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			// ReSharper disable PossibleMultipleEnumeration

			IEnumerable<FieldInfo> fields;

			if(!FieldsCache.TryGetValue(type, out fields))
			{
				lock(_lockObject)
				{
					if(!FieldsCache.TryGetValue(type, out fields))
					{
						fields = this.GetFieldsToSerializeInternal(type);
						FieldsCache.Add(type, fields);
					}
				}
			}

			return fields;

			// ReSharper restore PossibleMultipleEnumeration
		}

		protected internal virtual IEnumerable<FieldInfo> GetFieldsToSerializeInternal(Type type)
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

		//protected internal virtual IEnumerable<Type> GetGenericArgumentsRecursive(Type type)
		//{
		//	if(type == null)
		//		throw new ArgumentNullException("type");

		//	// ReSharper disable PossibleMultipleEnumeration

		//	IEnumerable<Type> genericArgumentsRecursive;

		//	if(!GenericArgumentsRecursiveCache.TryGetValue(type, out genericArgumentsRecursive))
		//	{
		//		lock(_lockObject)
		//		{
		//			if(!GenericArgumentsRecursiveCache.TryGetValue(type, out genericArgumentsRecursive))
		//			{
		//				genericArgumentsRecursive = this.GetGenericArgumentsRecursiveInternal(type);
		//				GenericArgumentsRecursiveCache.Add(type, genericArgumentsRecursive);
		//			}
		//		}
		//	}

		//	return genericArgumentsRecursive;

		//	// ReSharper restore PossibleMultipleEnumeration
		//}

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

		protected internal virtual bool IsGenericSerializableType(Type type)
		{
			//if(type != null)
			//{
				while(type != null)
				{
					if(type.IsGenericType && typeof(Serializable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
						return true;

					type = type.BaseType;
				}

				//if(typeof(SerializableField).IsAssignableFrom(type))
				//	return true;

				//if (typeof(SerializableDelegate).IsAssignableFrom(type))
				//	return true;
			//}

			return false;
		}

		public virtual bool IsSerializable(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			bool isSerializable;

			if(!IsSerializableCache.TryGetValue(type, out isSerializable))
			{
				lock(_lockObject)
				{
					if(!IsSerializableCache.TryGetValue(type, out isSerializable))
					{
						isSerializable = this.IsSerializableInternal(type);
						IsSerializableCache.Add(type, isSerializable);
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

			if(this.IsGenericSerializableType(type))
				return true;

			//if(typeof(NameValueCollection).IsAssignableFrom(type))
			//	return true;

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