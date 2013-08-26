using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using HansKindberg.Serialization.InversionOfControl;

namespace HansKindberg.Serialization
{
	
	[Serializable]
	public abstract class Serializable
	{
		#region Fields

		private readonly Guid _id;
		private Guid? _circularReferenceId;
		private Type _instanceType;
		private object _serializableInstance;
		[NonSerialized]
		private readonly IList<SerializationResult> _investigationResult;
		[NonSerialized]
		private bool _investigateSerializability;


		[NonSerialized]
		private ICircularReferenceTracker _circularReferenceTracker;
		[NonSerialized]
		private ISerializationResolver _serializationResolver;

		[NonSerialized]
		private bool? _instanceIsSerializable;

		[NonSerialized] private object _instance;
		

		#endregion

		#region Constructors

		protected Serializable(object instance, ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker, bool investigateSerializability, IList<SerializationResult> investigationResult)
		{
			if (serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			if (circularReferenceTracker == null)
				throw new ArgumentNullException("circularReferenceTracker");

			if (investigationResult == null)
				throw new ArgumentNullException("investigationResult");

			this._circularReferenceTracker = circularReferenceTracker;
			this._id = Guid.NewGuid();
			this._instance = instance;
			this._investigateSerializability = investigateSerializability;
			this._investigationResult = investigationResult;
			this._serializationResolver = serializationResolver;
			this.SetInstanceType(instance);
		}

		#endregion

		protected internal void SetInstanceType(object instance)
		{
			this._instanceType = instance != null ? instance.GetType() : null;
		}

		#region Properties

		protected internal virtual bool InstanceIsArray
		{
			get { return this.Instance is Array; }
		}

		protected internal virtual bool InstanceIsDelegate
		{
			get { return this.Instance is Delegate; }
		}

		protected internal virtual bool? InstanceIsSerializableInternal
		{
			get { return this._instanceIsSerializable; }
			set { this._instanceIsSerializable = value; }
		}

		public virtual object Instance
		{
			get { return this._instance; }
			protected internal set
			{
				this.InstanceIsSerializableInternal = null;
				this.SetInstanceType(value);
				this._instance = value;
			}
		}

		public abstract bool InstanceIsSerializable { get; }

		protected internal virtual ICircularReferenceTracker CircularReferenceTracker
		{
			get { return this._circularReferenceTracker; }
			set { this._circularReferenceTracker = value; }
		}

		protected internal virtual ISerializationResolver SerializationResolver
		{
			get { return this._serializationResolver; }
			set { this._serializationResolver = value; }
		}

		protected internal virtual IList<SerializationResult> InvestigationResultInternal
		{
			get { return this._investigationResult; }
		}

		protected internal virtual bool InvestigateSerializabilityInternal
		{
			get { return this._investigateSerializability; }
			set { this._investigateSerializability = value; }
		}



		public virtual Guid Id
		{
			get { return this._id; }
		}

		protected internal virtual Guid? CircularReferenceId
		{
			get { return this._circularReferenceId; }
			set { this._circularReferenceId = value; }
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

		#endregion

		#region Methods

		protected internal virtual bool IsSerializable(object instance)
		{
			if (this.InvestigateSerializabilityInternal)
			{
				SerializationResult serializationResult = this.SerializationResolver.TrySerialize(instance);
				this.InvestigationResultInternal.Add(serializationResult);
				return serializationResult.IsSerializable;
			}

			return this.SerializationResolver.IsSerializable(instance);
		}



		//protected internal virtual Delegate CreateDeserializedDelegate()
		//{
		//	var @delegate = this.SerializableInstance as Delegate;

		//	if(@delegate != null)
		//		return @delegate;

		//	SerializableDelegate serializableDelegate = (SerializableDelegate) this.SerializableInstance;

		//	return Delegate.CreateDelegate(this.InstanceType, serializableDelegate.Instance, serializableDelegate.MethodInformation);
		//}

		//protected internal virtual IEnumerable<SerializableField> CreateDeserializedFields()
		//{
		//	return (IEnumerable<SerializableField>)this.SerializableInstance;
		//}

		protected internal virtual Serializable CreateSerializable(object instance)
		{
			if(this.IsSerializable(instance))
				throw new InvalidOperationException("Change this message.");


			//if (serializationResolver == null)
			//	throw new ArgumentNullException("serializationResolver");

			//if (this.InstanceType == null)
			//	return null;

			//if (this.InstanceIsSerializable(serializationResolver))
			//	return this.SerializableInstance;

			//if (this.InstanceIsArray)
			//	return this.CreateDeserializedArray();

			////if(this.InstanceIsDelegate)
			////	return this.CreateDeserializedDelegate();

			//object instance = serializationResolver.CreateUninitializedObject(this.InstanceType);

			//foreach (SerializableField deserializedField in this.CreateDeserializedFields())
			//{
			//	deserializedField.FieldInformation.SetValue(instance, deserializedField.Instance);
			//}

			//return instance;
		}

		protected internal virtual object CreateDeserializedInstance(ISerializationResolver serializationResolver)
		{
			return null;


			//if (serializationResolver == null)
			//	throw new ArgumentNullException("serializationResolver");

			//if (this.InstanceType == null)
			//	return null;

			//if (this.InstanceIsSerializable(serializationResolver))
			//	return this.SerializableInstance;

			//if (this.InstanceIsArray)
			//	return this.CreateDeserializedArray();

			////if(this.InstanceIsDelegate)
			////	return this.CreateDeserializedDelegate();

			//object instance = serializationResolver.CreateUninitializedObject(this.InstanceType);

			//foreach (SerializableField deserializedField in this.CreateDeserializedFields())
			//{
			//	deserializedField.FieldInformation.SetValue(instance, deserializedField.Instance);
			//}

			//return instance;
		}



		//protected internal virtual object CreateSerializableDelegate(ISerializationResolver serializationResolver)
		//{
		//	if(serializationResolver == null)
		//		throw new ArgumentNullException("serializationResolver");

		//	Delegate @delegate = (Delegate) this.Instance;

		//	if(@delegate == null)
		//		return @delegate;

		//	if(@delegate.Target == null)
		//		return @delegate;

		//	if(@delegate.Method == null)
		//		return @delegate;

		//	if(@delegate.Method.DeclaringType == null)
		//		return @delegate;

		//	if(serializationResolver.IsSerializable(@delegate.Method.DeclaringType))
		//		return @delegate;

		//	return new SerializableDelegate(@delegate.Method, @delegate.Target);
		//}

		protected internal abstract object CreateSerializableInstance();

		protected internal virtual void PrepareForSerialization()
		{
			this.CircularReferenceTracker.TrackInstanceIfNecessary(this);

			if (this.SetCircularReferenceIdIfNecessary())
				return;

			this.SerializableInstance = this.InstanceIsSerializable ? this.Instance : this.CreateSerializableInstance();
		}

		//protected internal virtual object CreateSerializableInstance(ISerializationResolver serializationResolver)
		//{
		//	if(serializationResolver == null)
		//		throw new ArgumentNullException("serializationResolver");

		//	if(this.InstanceIsSerializable(serializationResolver))
		//		return this.Instance;

		//	if(this.InstanceIsArray)
		//		return this.CreateSerializableArray(serializationResolver);

		//	//if(this.InstanceIsDelegate)
		//	//	return this.CreateSerializableDelegate(serializationResolver);

		//	return this.CreateSerializableFields(serializationResolver);
		//}

		//protected internal virtual bool InstanceIsSerializable(ISerializationResolver serializationResolver)
		//{
		//	if(serializationResolver == null)
		//		throw new ArgumentNullException("serializationResolver");

		//	return this.InstanceType == null || serializationResolver.IsSerializable(this.Instance);
		//}

		protected internal virtual void Deserialize(ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker, IList<Serializable> instancesReferencingCircularReference)
		{
			//if(circularReferenceTracker == null)
			//	throw new ArgumentNullException("circularReferenceTracker");

			//if(instancesReferencingCircularReference == null)
			//	throw new ArgumentNullException("instancesReferencingCircularReference");

			//var serializable = this.SerializableInstance as Serializable;

			//if(serializable != null)
			//{
			//	serializable.Deserialize(serializationResolver, circularReferenceTracker, instancesReferencingCircularReference);
			//}
			//else
			//{
			//	var serializableFields = this.SerializableInstance as IEnumerable<SerializableField>;

			//	if(serializableFields != null)
			//	{
			//		foreach(var serializableField in serializableFields)
			//		{
			//			serializableField.Deserialize(serializationResolver, circularReferenceTracker, instancesReferencingCircularReference);
			//		}
			//	}
			//}

			//if(this.CircularReferenceId != null)
			//{
			//	instancesReferencingCircularReference.Add(this);
			//	return;
			//}

			//this.Instance = this.CreateDeserializedInstance(serializationResolver);

			//circularReferenceTracker.TrackInstanceIfNecessary(this, serializationResolver);
		}

		protected internal virtual bool SetCircularReferenceIdIfNecessary()
		{
			if (this.InstanceIsSerializable)
				return false;

			Guid? instanceId = this.CircularReferenceTracker.GetTrackedInstanceId(this.Instance);

			if (!instanceId.HasValue)
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The instance for serializable with id \"{0}\" has not been tracked.", this.Id));

			if (instanceId.Value == this.Id)
				return false;

			this.CircularReferenceId = instanceId;
			this.CircularReferenceTracker.AddReference(instanceId.Value);

			return true;
		}

		//protected internal virtual void Serialize()
		//{


		//	circularReferenceTracker.TrackInstanceIfNecessary(this, serializationResolver);

		//	if (this.SetCircularReferenceIdIfNecessary(serializationResolver, circularReferenceTracker))
		//		return;

		//	this.SerializableInstance = this.CreateSerializableInstance(serializationResolver);

		//	Serializable serializable = this.SerializableInstance as Serializable;

		//	if (serializable != null)
		//	{
		//		serializable.Serialize(serializationResolver, circularReferenceTracker);
		//		return;
		//	}
		//}

		#endregion

		#region Eventhandlers

		protected internal virtual void OnDeserialized(StreamingContext streamingContext)
		{
			this.CircularReferenceId = null;
			this.SerializableInstance = null;
		}

		[OnDeserialized]
		private void OnDeserializedInternal(StreamingContext streamingContext)
		{
			this.OnDeserialized(streamingContext);
		}

		protected internal virtual void OnSerialized(StreamingContext streamingContext)
		{
			this.CircularReferenceId = null;
			this.SerializableInstance = null;
		}

		[OnSerialized]
		private void OnSerializedInternal(StreamingContext streamingContext)
		{
			this.OnSerialized(streamingContext);
		}

		#endregion
	}

	/// <summary>
	/// Generic serializable wrapper to be able to serialize/deserialize theoretically any type of object.
	/// The idea is originally from: <see href="http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx">Anonymous Method Serialization, by Fredrik Norén, 12 Feb 2009</see>
	/// </summary>
	[Serializable]
	public class Serializable<T> : GenericSerializable<T>
	{
		#region Fields

		private readonly IList<Guid> _circularReferenceIds;
		
		#endregion

		#region Constructors

		public Serializable(T instance) : this(instance, ServiceLocator.Instance.GetService<ISerializationResolver>(), ServiceLocator.Instance.GetService<ICircularReferenceTracker>()) {}

		protected internal Serializable(T instance, ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker) : base(instance, serializationResolver, circularReferenceTracker, false, new List<SerializationResult>())
		{
			this._circularReferenceIds = new List<Guid>();
		}

		#endregion

		#region Properties

		protected internal virtual IList<Guid> CircularReferenceIds
		{
			get { return this._circularReferenceIds; }
		}



		public virtual bool InvestigateSerializability
		{
			get { return this.InvestigateSerializabilityInternal; }
			set { this.InvestigateSerializabilityInternal = value; }
		}


		public virtual IEnumerable<SerializationResult> InvestigationResult
		{
			get { return this.InvestigationResultInternal.ToArray(); }
		}





		#endregion

		protected internal virtual void ClearCircularReferenceTracking()
		{
			this.CircularReferenceIds.Clear();
			this.CircularReferenceTracker.Clear();
		}

		#region Eventhandlers

		protected internal override void OnDeserialized(StreamingContext streamingContext)
		{
			foreach(var circularReferenceId in this.CircularReferenceIds)
			{
				this.CircularReferenceTracker.AddReference(circularReferenceId);
			}

			var instancesReferencingCircularReference = new List<Serializable>();
			this.Deserialize(this.SerializationResolver, this.CircularReferenceTracker, instancesReferencingCircularReference);

			base.OnDeserialized(streamingContext);

			this.ClearCircularReferenceTracking();
		}

		protected internal virtual void OnDeserializing(StreamingContext streamingContext)
		{
			this.CircularReferenceTracker = ServiceLocator.Instance.GetService<ICircularReferenceTracker>();
			this.SerializationResolver = ServiceLocator.Instance.GetService<ISerializationResolver>();
		}

		[OnDeserializing]
		private void OnDeserializingInternal(StreamingContext streamingContext)
		{
			this.OnDeserializing(streamingContext);
		}

		protected internal override void OnSerialized(StreamingContext streamingContext)
		{
			base.OnSerialized(streamingContext);

			this.ClearCircularReferenceTracking();
		}

		protected internal virtual void OnSerializing(StreamingContext streamingContext)
		{
			this.Serialize(this.SerializationResolver, this.CircularReferenceTracker);

			foreach(var circularReferenceId in this.CircularReferenceTracker.References)
			{
				this.CircularReferenceIds.Add(circularReferenceId);
			}
		}

		[OnSerializing]
		private void OnSerializingInternal(StreamingContext streamingContext)
		{
			this.OnSerializing(streamingContext);
		}

		#endregion
	}
}