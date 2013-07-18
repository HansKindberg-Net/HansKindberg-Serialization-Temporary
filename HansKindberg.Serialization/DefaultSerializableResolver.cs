using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Castle.DynamicProxy;
using HansKindberg.Serialization.Extensions;

namespace HansKindberg.Serialization
{
	[Serializable]
	public class DefaultSerializableResolver : ISerializableResolver
	{
		#region Fields

		private static readonly IDictionary<Type, IEnumerable<FieldInfo>> _fieldsCache = new Dictionary<Type, IEnumerable<FieldInfo>>();
		private const string _instanceSerializationInformationName = "Instance";
		private const string _instanceTypeSerializationInformationName = "Type";
		private static readonly IDictionary<Type, bool> _isSerializableCache = new Dictionary<Type, bool>();
		private const string _isSerializableSerializationInformationName = "IsSerializable";
		private static readonly object _lockObject = new object();
		[NonSerialized] private IProxyBuilder _proxyBuilder;

		#endregion

		#region Properties

		protected internal virtual string InstanceSerializationInformationName
		{
			get { return _instanceSerializationInformationName; }
		}

		protected internal virtual string InstanceTypeSerializationInformationName
		{
			get { return _instanceTypeSerializationInformationName; }
		}

		protected internal virtual string IsSerializableSerializationInformationName
		{
			get { return _isSerializableSerializationInformationName; }
		}

		protected internal virtual IProxyBuilder ProxyBuilder
		{
			get { return this._proxyBuilder ?? (this._proxyBuilder = new DefaultProxyBuilder()); }
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

			return FormatterServices.GetUninitializedObject(this.ConvertUninitializedObjectType(type));
		}

		protected internal virtual IEnumerable<FieldInfo> GetCachedFields(Type type)
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
						fields = this.GetFields(type);
						_fieldsCache.Add(type, fields);
					}
				}
			}

			return fields;

			// ReSharper restore PossibleMultipleEnumeration
		}

		protected internal virtual IEnumerable<FieldInfo> GetFields(Type type)
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

		public virtual T GetInstance<T>(SerializationInfo serializationInformation)
		{
			if(serializationInformation == null)
				throw new ArgumentNullException("serializationInformation");

			try
			{
				if(serializationInformation.TryGetValue(this.IsSerializableSerializationInformationName, false))
					return (T) serializationInformation.GetValue(this.InstanceSerializationInformationName, typeof(T));

				Type type = serializationInformation.TryGetValue<Type>(this.InstanceTypeSerializationInformationName, null);
				object instance = this.CreateUninitializedObject(type);

				foreach(FieldInfo field in this.GetCachedFields(type))
				{
					if(this.TrySetFieldValueFromSerializationInformation(instance, field, serializationInformation))
						continue;

					Serializable<object> serializable = (Serializable<object>) serializationInformation.GetValue(this.GetSerializationInformationName(field), typeof(Serializable<object>));
					field.SetValue(instance, serializable != null ? serializable.Instance : null);
				}

				return (T) instance;
			}
			catch(Exception exception)
			{
				throw new ArgumentException("The instance could not be retrieved from the serialization-information.", "serializationInformation", exception);
			}
		}

		protected internal virtual string GetSerializationInformationName(FieldInfo field)
		{
			if(field == null)
				throw new ArgumentNullException("field");

			if(field.DeclaringType == null)
				throw new ArgumentException("The field has no declaring type.", "field");

			return field.DeclaringType.FullName + "." + field.Name;
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
						isSerializable = type.IsSerializable && this.GetGenericArgumentsRecursive(type).All(genericArgument => genericArgument.IsSerializable);
						_isSerializableCache.Add(type, isSerializable);
					}
				}
			}

			return isSerializable;
		}

		public virtual void SetInstance<T>(T instance, SerializationInfo serializationInformation)
		{
			if(Equals(instance, null))
				throw new ArgumentNullException("instance");

			if(serializationInformation == null)
				throw new ArgumentNullException("serializationInformation");

			if(this.IsSerializable(instance.GetType()))
			{
				serializationInformation.AddValue(this.InstanceSerializationInformationName, instance);
				serializationInformation.AddValue(this.IsSerializableSerializationInformationName, true);
				return;
			}

			serializationInformation.AddValue(this.IsSerializableSerializationInformationName, false);
			serializationInformation.AddValue(this.InstanceTypeSerializationInformationName, instance.GetType());

			foreach(FieldInfo field in this.GetCachedFields(instance.GetType()))
			{
				if(this.TryAddFieldValueToSerializationInformation(instance, field, serializationInformation))
					continue;

				Serializable<object> serializable = null;
				object fieldValue = field.GetValue(instance);

				if(fieldValue != null)
					serializable = new Serializable<object>(fieldValue, this);

				serializationInformation.AddValue(this.GetSerializationInformationName(field), serializable);
			}
		}

		protected internal virtual bool TryAddFieldValueToSerializationInformation(object instance, FieldInfo field, SerializationInfo serializationInformation)
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
					serializationInformation.AddValue(this.GetSerializationInformationName(field), field.GetValue(instance));
					return true;
				}
			}

			return false;
		}

		protected internal virtual bool TrySetFieldValueFromSerializationInformation(object instance, FieldInfo field, SerializationInfo serializationInformation)
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

				if(serializationInformation.TryGetValue(this.GetSerializationInformationName(field), out fieldValue))
				{
					field.SetValue(instance, fieldValue);
					return true;
				}
			}

			return false;
		}

		#endregion
	}
}