using System;
using System.Runtime.Serialization;

namespace HansKindberg.Serialization
{
	[Serializable]
	public class SerializableDelegateResolver : DefaultSerializableResolver, ISerializableDelegateResolver
	{
		#region Fields

		[NonSerialized] private ITypeValidator _typeValidator;

		#endregion

		#region Properties

		protected internal ITypeValidator TypeValidator
		{
			get { return this._typeValidator ?? (this._typeValidator = new DefaultTypeValidator()); }
		}

		#endregion

		//private const string _anonymousDelegateSerializationInformationName = "AnonymousDelegate";
		//private readonly TDelegate _delegateInstance;
		//private const string _methodSerializationInformationName = "Method";
		//private readonly ISerializableResolver _serializableResolver = new DefaultSerializableResolver();
		//private readonly ITypeValidator _typeValidator = new TypeValidator();

		#region Methods

		public override T GetInstance<T>(SerializationInfo serializationInformation)
		{
			//if (serializationInformation == null)
			//	throw new ArgumentNullException("serializationInformation");

			//if (serializationInformation.TryGetValue(this.IsSerializableSerializationInformationName, false))
			//{
			//	this._delegateInstance = this.SerializableResolver.GetInstance<TDelegate>(info);
			//	return;
			//}

			//Type type = this.SerializableResolver.GetInstanceType(info);
			//MethodInfo methodInfo = (MethodInfo)info.GetValue(_methodSerializationInformationName, typeof(MethodInfo));
			//Serializable<object, SerializableDelegateResolver<TDelegate>> anonymousDelegate = (Serializable<object, SerializableDelegateResolver<TDelegate>>)info.GetValue(_anonymousDelegateSerializationInformationName, typeof(Serializable<object, SerializableDelegateResolver<TDelegate>>));
			//this._delegateInstance = (TDelegate)(object)System.Delegate.CreateDelegate(type, anonymousDelegate.Instance, methodInfo);

			return base.GetInstance<T>(serializationInformation);
		}

		protected internal virtual bool IsSerializableDelegate(Delegate unTypedDelegate)
		{
			if(unTypedDelegate == null)
				throw new ArgumentNullException("unTypedDelegate");

			if(unTypedDelegate.Target == null)
				return true;

			if(unTypedDelegate.Method == null)
				return false;

			if(unTypedDelegate.Method.DeclaringType == null)
				return false;

			return unTypedDelegate.Method.DeclaringType.GetCustomAttributes(typeof(SerializableAttribute), false).Length > 0;
		}

		public override void SetInstance<T>(T instance, SerializationInfo serializationInformation)
		{
			//if (info == null)
			//	throw new ArgumentNullException("info");

			//Delegate unTypedDelegate = (Delegate)(object)this.DelegateInstance;

			//if (this.IsSerializableDelegate(unTypedDelegate))
			//{
			//	this.SerializableResolver.SetIsSerializable(info, true);
			//	this.SerializableResolver.SetInstance(info, this.DelegateInstance);
			//	return;
			//}

			//this.SerializableResolver.SetIsSerializable(info, false);
			//this.SerializableResolver.SetInstanceType(info, this.DelegateInstance.GetType());
			//info.AddValue(_methodSerializationInformationName, unTypedDelegate.Method);
			//info.AddValue(_anonymousDelegateSerializationInformationName, new Serializable<object, SerializableDelegateResolver<TDelegate>>(unTypedDelegate.Target, unTypedDelegate.Method.DeclaringType));

			base.SetInstance<T>(instance, serializationInformation);
		}

		public virtual void ValidateDelegateType(Type delegateType)
		{
			this.TypeValidator.ValidateThatTheTypeIsADelegate(delegateType);
		}

		#endregion

		//protected internal override bool TryAddFieldValueToSerializationInformation(object instance, FieldInfo field, SerializationInfo serializationInformation)
		//{
		//	if (instance == null)
		//		throw new ArgumentNullException("instance");
		//	if (field == null)
		//		throw new ArgumentNullException("field");
		//	if (serializationInformation == null)
		//		throw new ArgumentNullException("serializationInformation");
		//	if (typeof(Delegate).IsAssignableFrom(field.FieldType))
		//	{
		//		serializationInformation.AddValue(this.GetSerializationInformationName(field), new SerializableDelegate<TDelegate>((TDelegate)field.GetValue(instance)));
		//		return true;
		//	}
		//	return base.TryAddFieldValueToSerializationInformation(instance, field, serializationInformation);
		//}
		//protected internal override bool TrySetFieldValueFromSerializationInformation(object instance, FieldInfo field, SerializationInfo serializationInformation)
		//{
		//	if (instance == null)
		//		throw new ArgumentNullException("instance");
		//	if (field == null)
		//		throw new ArgumentNullException("field");
		//	if (serializationInformation == null)
		//		throw new ArgumentNullException("serializationInformation");
		//	if (typeof(Delegate).IsAssignableFrom(field.FieldType))
		//	{
		//		field.SetValue(instance, ((SerializableDelegate<TDelegate>)serializationInformation.GetValue(this.GetSerializationInformationName(field), typeof(SerializableDelegate<TDelegate>))).DelegateInstance);
		//		return true;
		//	}
		//	return base.TrySetFieldValueFromSerializationInformation(instance, field, serializationInformation);
		//}
	}
}