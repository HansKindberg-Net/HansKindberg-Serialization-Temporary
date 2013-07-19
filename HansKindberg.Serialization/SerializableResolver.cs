using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Castle.DynamicProxy;
using HansKindberg.Serialization.Extensions;

namespace HansKindberg.Serialization
{
	public abstract class SerializableResolver
	{
		#region Fields

		private static readonly IDictionary<Type, IEnumerable<FieldInfo>> _fieldsCache = new Dictionary<Type, IEnumerable<FieldInfo>>();
		private static readonly IDictionary<Type, IEnumerable<Type>> _genericArgumentsRecursiveCache = new Dictionary<Type, IEnumerable<Type>>();
		private static readonly IDictionary<Type, bool> _isSerializableCache = new Dictionary<Type, bool>();
		private const string _isSerializableSerializationInformationName = "IsSerializable";
		private static readonly object _lockObject = new object();
		private readonly IProxyBuilder _proxyBuilder;
		private const string _typeSerializationInformationName = "Type";

		#endregion

		#region Constructors

		protected SerializableResolver(IProxyBuilder proxyBuilder)
		{
			if(proxyBuilder == null)
				throw new ArgumentNullException("proxyBuilder");

			this._proxyBuilder = proxyBuilder;
		}

		#endregion

		#region Properties

		protected internal virtual string IsSerializableSerializationInformationName
		{
			get { return _isSerializableSerializationInformationName; }
		}

		protected internal virtual IProxyBuilder ProxyBuilder
		{
			get { return this._proxyBuilder; }
		}

		protected internal virtual string TypeSerializationInformationName
		{
			get { return _typeSerializationInformationName; }
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

		protected internal virtual T CreateUninitializedObject<T>()
		{
			return (T) this.CreateUninitializedObject(typeof(T));
		}

		protected internal virtual object CreateUninitializedObject(Type type)
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

		protected internal virtual string FieldSerializationInformationName(FieldInfo field)
		{
			if(field == null)
				throw new ArgumentNullException("field");

			if(field.DeclaringType == null)
				throw new ArgumentException("The field has no declaring type.", "field");

			return field.DeclaringType.FullName + "." + field.Name;
		}

		protected internal virtual IEnumerable<FieldInfo> GetFields(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			// ReSharper disable PossibleMultipleEnumeration

			IEnumerable<FieldInfo> fields;

			if(!_fieldsCache.TryGetValue(type, out fields))
			{
				lock(_lockObject)
				{
					if(!_fieldsCache.TryGetValue(type, out fields))
					{
						fields = this.GetFieldsInternal(type);
						_fieldsCache.Add(type, fields);
					}
				}
			}

			return fields;

			// ReSharper restore PossibleMultipleEnumeration
		}

		protected internal virtual IEnumerable<FieldInfo> GetFieldsInternal(Type type)
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

			// ReSharper disable PossibleMultipleEnumeration

			IEnumerable<Type> genericArgumentsRecursive;

			if(!_genericArgumentsRecursiveCache.TryGetValue(type, out genericArgumentsRecursive))
			{
				lock(_lockObject)
				{
					if(!_genericArgumentsRecursiveCache.TryGetValue(type, out genericArgumentsRecursive))
					{
						genericArgumentsRecursive = this.GetGenericArgumentsRecursiveInternal(type);
						_genericArgumentsRecursiveCache.Add(type, genericArgumentsRecursive);
					}
				}
			}

			return genericArgumentsRecursive;

			// ReSharper restore PossibleMultipleEnumeration
		}

		protected internal virtual IEnumerable<Type> GetGenericArgumentsRecursiveInternal(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			foreach(Type genericArgument in type.GetGenericArguments())
			{
				yield return genericArgument;

				foreach(Type childGenericArgument in this.GetGenericArgumentsRecursiveInternal(genericArgument))
				{
					yield return childGenericArgument;
				}
			}
		}

		protected internal virtual bool IsSerializable(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			bool isSerializable;

			if(!_isSerializableCache.TryGetValue(type, out isSerializable))
			{
				lock(_lockObject)
				{
					if(!_isSerializableCache.TryGetValue(type, out isSerializable))
					{
						isSerializable = this.IsSerializableInternal(type);
						_isSerializableCache.Add(type, isSerializable);
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

			if(typeof(NameValueCollection).IsAssignableFrom(type))
				return true;

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

		protected internal virtual bool TryAddFieldValueToSerializationInformation(object instance, FieldInfo field, SerializationInfo serializationInformation, string index)
		{
			if(instance == null)
				throw new ArgumentNullException("instance");

			if(field == null)
				throw new ArgumentNullException("field");

			if(serializationInformation == null)
				throw new ArgumentNullException("serializationInformation");

			if(this.IsSerializable(field.FieldType))
			{
				object fieldValue = field.GetValue(instance);

				if(fieldValue != null && this.IsSerializable(fieldValue.GetType()))
				{
					serializationInformation.AddValue(this.FieldSerializationInformationName(field) + index, field.GetValue(instance));
					return true;
				}

				if(fieldValue == null)
				{
					serializationInformation.AddValue(this.FieldSerializationInformationName(field) + index, null);
					return true;
				}
			}

			return false;
		}

		protected internal virtual bool TrySetFieldValueFromSerializationInformation(object instance, FieldInfo field, SerializationInfo serializationInformation, string index)
		{
			if(instance == null)
				throw new ArgumentNullException("instance");

			if(field == null)
				throw new ArgumentNullException("field");

			if(serializationInformation == null)
				throw new ArgumentNullException("serializationInformation");

			if(this.IsSerializable(field.FieldType))
			{
				object fieldValue;

				if(serializationInformation.TryGetValue(this.FieldSerializationInformationName(field) + index, out fieldValue))
				{
					if(fieldValue != null)
						field.SetValue(instance, fieldValue);

					return true;
				}
			}

			return false;
		}

		#endregion
	}
}