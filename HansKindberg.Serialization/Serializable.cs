using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace HansKindberg.Serialization
{
	[Serializable]
	public abstract class Serializable
	{
		#region Fields

		[NonSerialized] private object _instance;
		private object _serializableInstance;
		[NonSerialized] private ISerializableResolver _serializableResolver;
		private Type _type;

		#endregion

		#region Constructors

		protected Serializable(object instance, ISerializableResolver serializableResolver)
		{
			if(serializableResolver == null)
				throw new ArgumentNullException("serializableResolver");

			this._instance = instance;
			this._serializableResolver = serializableResolver;
			this.SetType(instance);
		}

		#endregion

		#region Properties

		protected internal virtual object InstanceInternal
		{
			get { return this._instance; }
			set
			{
				this._instance = value;
				this.SetType(value);
			}
		}

		protected internal virtual object SerializableInstance
		{
			get { return this._serializableInstance; }
			set { this._serializableInstance = value; }
		}

		protected internal virtual ISerializableResolver SerializableResolver
		{
			get { return this._serializableResolver; }
		}

		[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
		protected internal virtual Type Type
		{
			get { return this._type; }
			set { this._type = value; }
		}

		#endregion

		#region Methods

		protected internal abstract object CreateDeserializedInstance();
		protected internal abstract object CreateSerializableInstance();

		protected internal virtual bool IsSerializable(object instance)
		{
			return instance == null || this.SerializableResolver.IsSerializable(instance.GetType());
		}

		protected internal void SetType(object instance)
		{
			this._type = instance != null ? instance.GetType() : null;
		}

		#endregion

		#region Eventhandlers

		protected internal virtual void OnDeserialized(StreamingContext streamingContext)
		{
			this.InstanceInternal = this.CreateDeserializedInstance();
		}

		[OnDeserialized]
		private void OnDeserializedInternal(StreamingContext streamingContext)
		{
			this.OnDeserialized(streamingContext);
		}

		protected internal virtual void OnDeserializing(StreamingContext streamingContext)
		{
			this._serializableResolver = SerializableResolverLocator.Instance.SerializableResolver;
		}

		[OnDeserializing]
		private void OnDeserializingInternal(StreamingContext streamingContext)
		{
			this.OnDeserializing(streamingContext);
		}

		protected internal virtual void OnSerialized(StreamingContext streamingContext) {}

		[OnSerialized]
		private void OnSerializedInternal(StreamingContext streamingContext)
		{
			this.OnSerialized(streamingContext);
		}

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
	}

	/// <summary>
	/// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
	/// </summary>
	[Serializable]
	public class Serializable<T> : Serializable
	{
		#region Constructors

		public Serializable(T instance) : base(instance, SerializableResolverLocator.Instance.SerializableResolver) {}
		protected internal Serializable(T instance, ISerializableResolver serializableResolver) : base(instance, serializableResolver) {}

		#endregion

		#region Properties

		public virtual T Instance
		{
			get { return (T) this.InstanceInternal; }
		}

		#endregion

		#region Methods

		protected internal override object CreateDeserializedInstance()
		{
			throw new NotImplementedException();
		}

		protected internal override object CreateSerializableInstance()
		{
			if(this.IsSerializable(this.Instance))
				return this.Instance;

			if(this.Type.IsArray)
				return new SerializableArray(this.Instance as Array).CreateSerializableInstance();

			if(typeof(Delegate).IsAssignableFrom(this.Type))
				return new SerializableDelegate(this.Instance as Delegate).CreateSerializableInstance();

			return this.SerializableResolver.GetSerializableFields(this.Instance).ToArray();
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