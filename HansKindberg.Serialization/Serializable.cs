using System;
using System.Collections;
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

		private Guid? _circularReferenceId;
		[NonSerialized] private ICircularReferenceTracker _circularReferenceTracker;
		private readonly Guid _id;
		[NonSerialized] private object _instance;
		[NonSerialized] private bool? _instanceIsSerializable;
		private Type _instanceType;
		[NonSerialized] private bool _investigateSerializability;
		[NonSerialized] private readonly IList<SerializationResult> _investigationResult;
		private object _serializableInstance;
		[NonSerialized] private ISerializationResolver _serializationResolver;

		#endregion

		#region Constructors

		protected Serializable(object instance, ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker, bool investigateSerializability, IList<SerializationResult> investigationResult)
		{
			if(serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			if(circularReferenceTracker == null)
				throw new ArgumentNullException("circularReferenceTracker");

			if(investigationResult == null)
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

		#region Properties

		protected internal virtual Guid? CircularReferenceId
		{
			get { return this._circularReferenceId; }
			set { this._circularReferenceId = value; }
		}

		protected internal virtual ICircularReferenceTracker CircularReferenceTracker
		{
			get { return this._circularReferenceTracker; }
			set { this._circularReferenceTracker = value; }
		}

		public virtual Guid Id
		{
			get { return this._id; }
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

		protected internal virtual bool? InstanceIsSerializableInternal
		{
			get { return this._instanceIsSerializable; }
			set { this._instanceIsSerializable = value; }
		}

		protected internal virtual Type InstanceType
		{
			get { return this._instanceType; }
			set { this._instanceType = value; }
		}

		protected internal virtual bool InvestigateSerializabilityInternal
		{
			get { return this._investigateSerializability; }
			set { this._investigateSerializability = value; }
		}

		protected internal virtual IList<SerializationResult> InvestigationResultInternal
		{
			get { return this._investigationResult; }
		}

		protected internal virtual object SerializableInstance
		{
			get { return this._serializableInstance; }
			set { this._serializableInstance = value; }
		}

		protected internal virtual ISerializationResolver SerializationResolver
		{
			get { return this._serializationResolver; }
			set { this._serializationResolver = value; }
		}

		#endregion

		#region Methods

		protected internal virtual object CreateDeserializedInstance(ISerializationResolver serializationResolver)
		{
			//Array serializableArray = (Array)this.SerializableInstance;

			//Array array = (Array)Activator.CreateInstance(this.InstanceType, new object[] { serializableArray.Length });

			//for (int i = 0; i < array.Length; i++)
			//{
			//	object item = serializableArray.GetValue(i);

			//	if (item == null)
			//		continue;

			//	Serializable itemAsSerializable = item as Serializable;

			//	array.SetValue(itemAsSerializable != null ? itemAsSerializable.Instance : item, i);
			//}

			//return array;





			object instance = serializationResolver.CreateUninitializedObject(this.InstanceType);

			foreach (SerializableField serializableField in (IEnumerable<SerializableField>)this.SerializableInstance)
			{
				serializableField.FieldInformation.SetValue(instance, serializableField.Instance);
			}

			return instance;




			//Serializable serializable = this.SerializableInstance as Serializable;

			//if(serializable != null)
			//	return serializable.CreateDeserializedInstance(serializationResolver);

			//return this.SerializableInstance;
		}

		//protected internal virtual Serializable CreateSerializable(object instance)
		//{
		//	if(instance == null)
		//		return null;

		//	Array array = instance as Array;
		//	if(array != null)
		//		return new SerializableArray(array, this.SerializationResolver, this.CircularReferenceTracker, this.InvestigateSerializabilityInternal, this.InvestigationResultInternal);

		//	return new SerializableObject(instance, this.SerializationResolver, this.CircularReferenceTracker, this.InvestigateSerializabilityInternal, this.InvestigationResultInternal);
		//}

		protected internal virtual object CreateSerializableInstance()
		{
			if(this.InstanceIsSerializable)
				return this.Instance;

			Array array = this.Instance as Array;
			if(array != null)
			{
				object[] serializableArray = new object[array.Length];

				for (int i = 0; i < array.Length; i++)
				{
					object item = array.GetValue(i);

					if(this.IsSerializable(item))
						serializableArray[i] = item;
					else
						serializableArray[i] = new SerializableInternal(item, this.SerializationResolver, this.CircularReferenceTracker, this.InvestigateSerializabilityInternal, this.InvestigationResultInternal);
				}

				return serializableArray;
			}

			var serializableFields = new List<SerializableField>();

			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var fieldInformation in this.SerializationResolver.GetFieldsForSerialization(this.Instance.GetType()))
			{
				object fieldValue = fieldInformation.GetValue(this.Instance);

				if (fieldValue == null)
					continue;

				serializableFields.Add(new SerializableField(fieldInformation, fieldValue, this.SerializationResolver, this.CircularReferenceTracker, this.InvestigateSerializabilityInternal, this.InvestigationResultInternal));
			}
			// ReSharper restore LoopCanBeConvertedToQuery

			return serializableFields.ToArray();
		}

		protected internal virtual void HandleDeserialization(ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker, IList<Serializable> instancesReferencingCircularReference)
		{
			if (circularReferenceTracker == null)
				throw new ArgumentNullException("circularReferenceTracker");

			if (instancesReferencingCircularReference == null)
				throw new ArgumentNullException("instancesReferencingCircularReference");

			var enumerable = this.InstanceAsEnumerable(this.SerializableInstance);

			foreach (var serializableItem in enumerable.OfType<Serializable>())
			{
				serializableItem.HandleDeserialization(serializationResolver, circularReferenceTracker, instancesReferencingCircularReference);
			}

			if (this.CircularReferenceId != null)
			{
				instancesReferencingCircularReference.Add(this);
				return;
			}

			this.Instance = this.CreateDeserializedInstance(serializationResolver);

			circularReferenceTracker.TrackInstanceIfNecessary(this);
		}

		protected internal virtual IEnumerable InstanceAsEnumerable(object instance)
		{
			IEnumerable enumerable = instance as IEnumerable;

			if(enumerable != null)
				return enumerable;

			List<object> list = new List<object>();

			if(instance != null)
				list.Add(instance);

			return list.ToArray();
		}

		protected internal virtual bool IsSerializable(object instance)
		{
			if(this.InvestigateSerializabilityInternal)
			{
				SerializationResult serializationResult = this.SerializationResolver.TrySerialize(instance);
				this.InvestigationResultInternal.Add(serializationResult);
				return serializationResult.IsSerializable;
			}

			return this.SerializationResolver.IsSerializable(instance);
		}

		protected internal virtual void PrepareForSerialization()
		{
			this.CircularReferenceTracker.TrackInstanceIfNecessary(this);

			if(this.SetCircularReferenceIdIfNecessary())
				return;

			this.SerializableInstance = this.CreateSerializableInstance();

			var enumerable = this.InstanceAsEnumerable(this.SerializableInstance);

			foreach(var serializableItem in enumerable.OfType<Serializable>())
			{
				serializableItem.PrepareForSerialization();
			}
		}

		protected internal virtual bool SetCircularReferenceIdIfNecessary()
		{
			if(this.InstanceIsSerializable)
				return false;

			Guid? instanceId = this.CircularReferenceTracker.GetTrackedInstanceId(this.Instance);

			if(!instanceId.HasValue)
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The instance for serializable with id \"{0}\" has not been tracked.", this.Id));

			if(instanceId.Value == this.Id)
				return false;

			this.CircularReferenceId = instanceId;
			this.CircularReferenceTracker.AddReference(instanceId.Value);

			return true;
		}

		protected internal void SetInstanceType(object instance)
		{
			this._instanceType = instance != null ? instance.GetType() : null;
		}

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

		#region Methods

		protected internal virtual void ClearCircularReferenceTracking()
		{
			this.CircularReferenceIds.Clear();
			this.CircularReferenceTracker.Clear();
		}

		#endregion

		#region Eventhandlers

		protected internal override void OnDeserialized(StreamingContext streamingContext)
		{
			foreach(var circularReferenceId in this.CircularReferenceIds)
			{
				this.CircularReferenceTracker.AddReference(circularReferenceId);
			}

			var instancesReferencingCircularReference = new List<Serializable>();
			this.HandleDeserialization(this.SerializationResolver, this.CircularReferenceTracker, instancesReferencingCircularReference);

			foreach(var serializable in instancesReferencingCircularReference)
			{
				if(!serializable.CircularReferenceId.HasValue)
					throw new InvalidOperationException("The serializable is classified as referencing a circular reference but has no circular-reference-id.");

				serializable.Instance = this.CircularReferenceTracker.GetTrackedInstance(serializable.CircularReferenceId.Value);
			}

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
			this.PrepareForSerialization();

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