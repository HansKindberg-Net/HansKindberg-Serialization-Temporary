using System;
using System.Reflection;
using System.Runtime.Serialization;
using Castle.DynamicProxy;
using HansKindberg.Serialization.Extensions;

namespace HansKindberg.Serialization
{
	public class DefaultSerializableResolver : SerializableResolver, ISerializableResolver
	{
		#region Fields

		private const string _instanceSerializationInformationName = "Instance";

		#endregion

		#region Constructors

		public DefaultSerializableResolver(IProxyBuilder proxyBuilder) : base(proxyBuilder) {}

		#endregion

		#region Properties

		protected internal virtual string InstanceSerializationInformationName
		{
			get { return _instanceSerializationInformationName; }
		}

		#endregion

		#region Methods

		protected internal virtual object GetInstance(SerializationInfo serializationInformation, StreamingContext streamingContext, string index)
		{
			if(serializationInformation == null)
				throw new ArgumentNullException("serializationInformation");

			try
			{
				if(serializationInformation.TryGetValue(this.IsSerializableSerializationInformationName + index, false))
					return serializationInformation.GetValue(this.InstanceSerializationInformationName + index, typeof(object));

				Type type = serializationInformation.TryGetValue<Type>(this.TypeSerializationInformationName + index, null);

				if(type.IsArray)
					return new SerializableArray(serializationInformation, streamingContext, index).Array;

				if(typeof(Delegate).IsAssignableFrom(type))
					return new SerializableDelegate(serializationInformation, streamingContext, index).Delegate;

				object instance = this.CreateUninitializedObject(type);

				foreach(FieldInfo field in this.GetFields(type))
				{
					// YOU ARE TESTING HERE - MailMessage.Headers does not serialize
					if (field.Name == "headers")
					{
						throw new InvalidOperationException("// YOU ARE TESTING HERE - MailMessage.Headers does not serialize");
						string name = field.Name;
						name = name;
					}

					if(this.TrySetFieldValueFromSerializationInformation(instance, field, serializationInformation, index))
						continue;

					Serializable<object> serializable = (Serializable<object>) serializationInformation.GetValue(this.FieldSerializationInformationName(field) + index, typeof(Serializable<object>));
					field.SetValue(instance, serializable != null ? serializable.Instance : null);
				}

				return instance;
			}
			catch(Exception exception)
			{
				throw new ArgumentException("The instance could not be retrieved from the serialization-information.", "serializationInformation", exception);
			}
		}

		public virtual T InstanceFromSerializationInformation<T>(SerializationInfo serializationInformation, StreamingContext streamingContext, string index)
		{
			return (T) this.GetInstance(serializationInformation, streamingContext, index);
		}

		protected internal virtual void SetInstance(object instance, SerializationInfo serializationInformation, StreamingContext streamingContext, string index)
		{
			if(instance == null)
				throw new ArgumentNullException("instance");

			if(instance.GetType().IsArray)
			{
				new SerializableArray(instance).GetObjectData(serializationInformation, streamingContext, index);
				return;
			}

			if(instance is Delegate)
			{
				new SerializableDelegate(instance).GetObjectData(serializationInformation, streamingContext, index);
				return;
			}

			if(serializationInformation == null)
				throw new ArgumentNullException("serializationInformation");

			if(this.IsSerializable(instance.GetType()))
			{
				serializationInformation.AddValue(this.InstanceSerializationInformationName + index, instance);
				serializationInformation.AddValue(this.IsSerializableSerializationInformationName + index, true);
				return;
			}

			serializationInformation.AddValue(this.IsSerializableSerializationInformationName + index, false);
			serializationInformation.AddValue(this.TypeSerializationInformationName + index, instance.GetType());

			foreach(FieldInfo field in this.GetFields(instance.GetType()))
			{
				if(this.TryAddFieldValueToSerializationInformation(instance, field, serializationInformation, index))
					continue;

				Serializable<object> serializable = null;
				object fieldValue = field.GetValue(instance);

				if(fieldValue != null)
					serializable = new Serializable(fieldValue);

				serializationInformation.AddValue(this.FieldSerializationInformationName(field) + index, serializable);
			}
		}

		public virtual void InstanceToSerializationInformation<T>(T instance, SerializationInfo serializationInformation, StreamingContext streamingContext, string index)
		{
			this.SetInstance((object) instance, serializationInformation, streamingContext, index);
		}

		#endregion
	}
}