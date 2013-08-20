using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using HansKindberg.Serialization.IoC;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
	/// </summary>
	[Serializable]
	public class Serializable<T>
	{
		#region Fields

		[NonSerialized] private T _instance;
		private Type _instanceType;
		private object _serializableInstance;
		[NonSerialized] private ISerializationResolver _serializationResolver;

		#endregion

		#region Constructors

		public Serializable(T instance) : this(instance, ServiceLocator.Instance.GetService<ISerializationResolver>()) {}

		protected internal Serializable(T instance, ISerializationResolver serializationResolver)
		{
			if(serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			this._instance = instance;
			this._instanceType = Equals(instance, null) ? null : instance.GetType();
			this._serializationResolver = serializationResolver;
		}

		#endregion

		#region Properties

		public virtual T Instance
		{
			get
			{
				if(this.SerializableInstance != null)
					this.Instance = this.CreateDeserializedInstance();

				return this._instance;
			}
			protected internal set
			{
				this._instance = value;
				this.SerializableInstance = null;
			}
		}

		protected internal virtual bool InstanceIsArray
		{
			get { return this.InstanceType != null && this.InstanceType.IsArray; }
		}

		protected internal virtual bool InstanceIsDelegate
		{
			get { return this.InstanceType != null && typeof(Delegate).IsAssignableFrom(this.InstanceType); }
		}

		protected internal virtual bool InstanceIsSerializable
		{
			get { return this.InstanceType == null || this.SerializationResolver.IsSerializable(this.InstanceType); }
		}

		protected internal virtual Type InstanceType
		{
			get { return this._instanceType; }
			set { this._instanceType = value; }
		}

		protected internal virtual object SerializableInstance
		{
			get { return this._serializableInstance; }
			set { this._serializableInstance = value; }
		}

		protected internal virtual ISerializationResolver SerializationResolver
		{
			get { return this._serializationResolver; }
		}

		#endregion

		#region Methods

		protected internal virtual T CreateDeserializedArray()
		{
			Array serializableArray = (Array) this.SerializableInstance;

			Array array = (Array) Activator.CreateInstance(this.InstanceType, new object[] {serializableArray.Length});

			for(int i = 0; i < array.Length; i++)
			{
				object item = serializableArray.GetValue(i);

				if(item == null)
					continue;

				Serializable<object> itemAsSerializable = item as Serializable<object>;

				array.SetValue(itemAsSerializable != null ? itemAsSerializable.Instance : item, i);
			}

			return (T) (object) array;
		}

		protected internal virtual T CreateDeserializedDelegate()
		{
			return default(T);
		}

		protected internal virtual IEnumerable<SerializableField> CreateDeserializedFields()
		{
			return (IEnumerable<SerializableField>) this.SerializableInstance;
		}

		protected internal virtual T CreateDeserializedInstance()
		{
			if(this.InstanceType == null)
				return default(T);

			if(this.InstanceIsSerializable)
				return (T) this.SerializableInstance;

			if(this.InstanceIsArray)
				return this.CreateDeserializedArray();

			if(this.InstanceIsDelegate)
				return this.CreateDeserializedDelegate();

			object instance = this.SerializationResolver.CreateUninitializedObject(this.InstanceType);

			foreach(SerializableField deserializedField in this.CreateDeserializedFields())
			{
				deserializedField.FieldInformation.SetValue(instance, deserializedField.Instance);
			}

			return (T) instance;
		}

		protected internal virtual Array CreateSerializableArray()
		{
			Array array = (Array) (object) this.Instance;

			if(this.InstanceIsSerializable)
				return array;

			object[] serializableArray = new object[array.Length];

			for(int i = 0; i < array.Length; i++)
			{
				object item = array.GetValue(i);

				if(item == null || this.SerializationResolver.IsSerializable(item.GetType()))
					serializableArray[i] = item;
				else
					serializableArray[i] = new Serializable<object>(item, this.SerializationResolver);
			}

			return serializableArray;
		}

		protected internal virtual object CreateSerializableDelegate()
		{
			throw new NotImplementedException();
		}

		protected internal virtual IEnumerable<SerializableField> CreateSerializableFields()
		{
			List<SerializableField> serializableFields = new List<SerializableField>();

			if(this.InstanceType != null)
				serializableFields.AddRange(this.SerializationResolver.GetFieldsToSerialize(this.InstanceType).Select(fieldInfo => new SerializableField(fieldInfo, fieldInfo.GetValue(this.Instance))));

			return serializableFields.ToArray();
		}

		protected internal virtual object CreateSerializableInstance()
		{
			if(this.InstanceIsSerializable)
				return this.Instance;

			if(this.InstanceIsArray)
				return this.CreateSerializableArray();

			if(this.InstanceIsDelegate)
				return this.CreateSerializableDelegate();

			return this.CreateSerializableFields().ToArray();
		}

		#endregion

		#region Eventhandlers

		//protected internal virtual void OnDeserialized(StreamingContext streamingContext)
		//{
		//	this.Instance = this.CreateDeserializedInstance();
		//}

		//[OnDeserialized]
		//private void OnDeserializedInternal(StreamingContext streamingContext)
		//{
		//	this.OnDeserialized(streamingContext);
		//}

		protected internal virtual void OnDeserializing(StreamingContext streamingContext)
		{
			this._serializationResolver = ServiceLocator.Instance.GetService<ISerializationResolver>();
		}

		[OnDeserializing]
		private void OnDeserializingInternal(StreamingContext streamingContext)
		{
			this.OnDeserializing(streamingContext);
		}

		//protected internal virtual void OnSerialized(StreamingContext streamingContext) {}

		//[OnSerialized]
		//private void OnSerializedInternal(StreamingContext streamingContext)
		//{
		//	this.OnSerialized(streamingContext);
		//}

		protected internal virtual void OnSerializing(StreamingContext streamingContext)
		{
			this.SerializableInstance = this.CreateSerializableInstance();
		}

		[OnSerializing]
		private void OnSerializingInternal(StreamingContext streamingContext)
		{
			this.OnSerializing(streamingContext);
		}

		#endregion

		//#region Fields
		//private readonly T _instance;
		//private ISerializableResolver _serializableResolver;
		//#endregion
		//#region Constructors
		//public Serializable(T instance)
		//{
		//	if(Equals(instance, null))
		//		throw new ArgumentNullException("instance");
		//	this._instance = instance;
		//}
		//protected internal Serializable(T instance, ISerializableResolver serializableResolver) : this(instance)
		//{
		//	if(serializableResolver == null)
		//		throw new ArgumentNullException("serializableResolver");
		//	this._serializableResolver = serializableResolver;
		//}
		//protected Serializable(SerializationInfo info, StreamingContext context) : this(info, context, string.Empty) {}
		//protected internal Serializable(SerializationInfo info, StreamingContext context, string index)
		//{
		//	this._instance = this.SerializableResolver.InstanceFromSerializationInformation<T>(info, context, index);
		//}
		//protected internal Serializable(SerializationInfo info, StreamingContext context, ISerializableResolver serializableResolver) : this(info, context, serializableResolver, string.Empty) {}
		//protected internal Serializable(SerializationInfo info, StreamingContext context, ISerializableResolver serializableResolver, string index)
		//{
		//	if(serializableResolver == null)
		//		throw new ArgumentNullException("serializableResolver");
		//	this._serializableResolver = serializableResolver;
		//	this._instance = this.SerializableResolver.InstanceFromSerializationInformation<T>(info, context, index);
		//}
		//#endregion
		//#region Properties
		//public virtual T Instance
		//{
		//	get { return this._instance; }
		//}
		//protected internal ISerializableResolver SerializableResolver
		//{
		//	get { return this._serializableResolver ?? (this._serializableResolver = SerializableResolverLocator.Instance.SerializableResolver); }
		//}
		//#endregion
		//#region Methods
		//[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		//public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		//{
		//	this.GetObjectData(info, context, string.Empty);
		//}
		//public virtual void GetObjectData(SerializationInfo info, StreamingContext context, string index)
		//{
		//	this.SerializableResolver.InstanceToSerializationInformation(this.Instance, info, context, index);
		//}
		//#endregion
	}
}