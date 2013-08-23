using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using HansKindberg.Serialization.InversionOfControl;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// This class is mainly for internal use and is not intended to be used in your code. Use <see cref="Serializable&lt;T&gt;" /> instead.
	/// </summary>
	[Serializable]
	public class Serializable
	{
		#region Fields

		private readonly Guid _id;
		[NonSerialized] private object _instance;
		private Guid? _circularReferenceId;
		private Type _instanceType;
		private object _serializableInstance;

		#endregion

		#region Constructors

		public Serializable(object instance)
		{
			this._instance = instance;
			this._id = Guid.NewGuid();
			this._instanceType = Equals(instance, null) ? null : instance.GetType();
		}

		#endregion

		#region Properties

		public virtual Guid Id
		{
			get { return this._id; }
		}

		public virtual object Instance
		{
			get { return this._instance; }
			protected internal set { this._instance = value; }
		}

		protected internal virtual bool InstanceIsArray
		{
			get { return this.InstanceType != null && this.InstanceType.IsArray; }
		}

		protected internal virtual bool InstanceIsDelegate
		{
			get { return this.InstanceType != null && typeof(Delegate).IsAssignableFrom(this.InstanceType); }
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

		protected internal virtual Array CreateDeserializedArray()
		{
			Array serializableArray = (Array)this.SerializableInstance;

			Array array = (Array)Activator.CreateInstance(this.InstanceType, new object[] { serializableArray.Length });

			for (int i = 0; i < array.Length; i++)
			{
				object item = serializableArray.GetValue(i);

				if (item == null)
					continue;

				Serializable itemAsSerializable = item as Serializable;

				array.SetValue(itemAsSerializable != null ? itemAsSerializable.Instance : item, i);
			}

			return array;
		}

		//protected internal virtual Delegate CreateDeserializedDelegate()
		//{
		//	var @delegate = this.SerializableInstance as Delegate;

		//	if(@delegate != null)
		//		return @delegate;

		//	SerializableDelegate serializableDelegate = (SerializableDelegate) this.SerializableInstance;

		//	return Delegate.CreateDelegate(this.InstanceType, serializableDelegate.Instance, serializableDelegate.MethodInformation);
		//}

		protected internal virtual IEnumerable<SerializableField> CreateDeserializedFields()
		{
			return (IEnumerable<SerializableField>)this.SerializableInstance;
		}

		protected internal virtual object CreateDeserializedInstance(ISerializationResolver serializationResolver)
		{
			if (serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			if (this.InstanceType == null)
				return null;

			if (this.InstanceIsSerializable(serializationResolver))
				return this.SerializableInstance;

			if (this.InstanceIsArray)
				return this.CreateDeserializedArray();

			//if(this.InstanceIsDelegate)
			//	return this.CreateDeserializedDelegate();

			object instance = serializationResolver.CreateUninitializedObject(this.InstanceType);

			foreach (SerializableField deserializedField in this.CreateDeserializedFields())
			{
				deserializedField.FieldInformation.SetValue(instance, deserializedField.Instance);
			}

			return instance;
		}

		protected internal virtual Array CreateSerializableArray(ISerializationResolver serializationResolver)
		{
			if(serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			Array array = (Array) this.Instance;

			if(this.InstanceIsSerializable(serializationResolver))
				return array;

			object[] serializableArray = new object[array.Length];

			for(int i = 0; i < array.Length; i++)
			{
				object item = array.GetValue(i);

				if(serializationResolver.IsSerializable(item))
					serializableArray[i] = item;
				else
					serializableArray[i] = new Serializable(item);
			}

			return serializableArray;
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

		protected internal virtual IEnumerable<SerializableField> CreateSerializableFields(ISerializationResolver serializationResolver)
		{
			if(serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			var serializableFields = new List<SerializableField>();

			if(this.InstanceType != null)
				serializableFields.AddRange(serializationResolver.GetFieldsForSerialization(this.InstanceType).Select(fieldInfo => new SerializableField(fieldInfo, fieldInfo.GetValue(this.Instance))));

			return serializableFields.ToArray();
		}

		protected internal virtual object CreateSerializableInstance(ISerializationResolver serializationResolver)
		{
			if(serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			if(this.InstanceIsSerializable(serializationResolver))
				return this.Instance;

			if(this.InstanceIsArray)
				return this.CreateSerializableArray(serializationResolver);

			//if(this.InstanceIsDelegate)
			//	return this.CreateSerializableDelegate(serializationResolver);

			return this.CreateSerializableFields(serializationResolver);
		}

		protected internal virtual bool InstanceIsSerializable(ISerializationResolver serializationResolver)
		{
			if(serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			return this.InstanceType == null || serializationResolver.IsSerializable(this.Instance);
		}

		protected internal virtual void SetInstance(ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker, IList<Serializable> instancesReferencingCircularReference)
		{
			if(circularReferenceTracker == null)
				throw new ArgumentNullException("circularReferenceTracker");

			if(instancesReferencingCircularReference == null)
				throw new ArgumentNullException("instancesReferencingCircularReference");

			var serializable = this.SerializableInstance as Serializable;

			if(serializable != null)
			{
				serializable.SetInstance(serializationResolver, circularReferenceTracker, instancesReferencingCircularReference);
			}
			else
			{
				var serializableFields = this.SerializableInstance as IEnumerable<SerializableField>;

				if(serializableFields != null)
				{
					foreach(var serializableField in serializableFields)
					{
						serializableField.SetInstance(serializationResolver, circularReferenceTracker, instancesReferencingCircularReference);
					}
				}
			}

			if(this.CircularReferenceId != null)
			{
				instancesReferencingCircularReference.Add(this);
				return;
			}

			this.Instance = this.CreateDeserializedInstance(serializationResolver);

			circularReferenceTracker.TrackInstanceIfNecessary(this, serializationResolver);
		}

		protected internal virtual bool SetInstanceTrackerIdIfNecessary(ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker)
		{
			if(serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			if(circularReferenceTracker == null)
				throw new ArgumentNullException("circularReferenceTracker");

			if(this.InstanceIsSerializable(serializationResolver))
				return false;

			Guid? instanceId = circularReferenceTracker.GetTrackedInstanceId(this.Instance);

			if(!instanceId.HasValue)
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The instance for serializable with id \"{0}\" has not been tracked.", this.Id));

			if(instanceId.Value == this.Id)
				return false;

			this.SerializableInstance = null;
			this.CircularReferenceId = instanceId;
			circularReferenceTracker.AddReference(instanceId.Value);

			return true;
		}

		protected internal virtual void SetSerializableInstance(ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker)
		{
			if(serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			if(circularReferenceTracker == null)
				throw new ArgumentNullException("circularReferenceTracker");

			circularReferenceTracker.TrackInstanceIfNecessary(this, serializationResolver);

			if(this.SetInstanceTrackerIdIfNecessary(serializationResolver, circularReferenceTracker))
				return;

			this.SerializableInstance = this.CreateSerializableInstance(serializationResolver);

			Serializable serializable = this.SerializableInstance as Serializable;

			if(serializable != null)
			{
				serializable.SetSerializableInstance(serializationResolver, circularReferenceTracker);
				return;
			}

			IEnumerable<SerializableField> serializableFields = this.SerializableInstance as IEnumerable<SerializableField>;

			if(serializableFields == null)
				return;

			foreach(var serializableField in serializableFields)
			{
				serializableField.SetSerializableInstance(serializationResolver, circularReferenceTracker);
			}
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
	}

	/// <summary>
	/// Generic serializable wrapper to be able to serialize/deserialize theoretically any type of object.
	/// The idea is originally from: <see href="http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx">Anonymous Method Serialization, by Fredrik Norén, 12 Feb 2009</see>
	/// </summary>
	[Serializable]
	public class Serializable<T> : Serializable
	{
		#region Fields

		private readonly IList<Guid> _circularReferenceIds;
		[NonSerialized] private ICircularReferenceTracker _circularReferenceTracker;
		private bool _decideIfAnInstanceIsSerializableByActuallySerializingIt;
		[NonSerialized] private ISerializationResolver _serializationResolver;

		#endregion

		#region Constructors

		public Serializable(T instance) : this(instance, ServiceLocator.Instance.GetService<ISerializationResolver>(), ServiceLocator.Instance.GetService<ICircularReferenceTracker>()) {}

		protected internal Serializable(T instance, ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker) : base(instance)
		{
			if(serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			if(circularReferenceTracker == null)
				throw new ArgumentNullException("circularReferenceTracker");

			this._circularReferenceIds = new List<Guid>();
			this._circularReferenceTracker = circularReferenceTracker;
			this._serializationResolver = serializationResolver;
		}

		#endregion

		#region Properties

		protected internal virtual IList<Guid> CircularReferenceIds
		{
			get { return this._circularReferenceIds; }
		}

		protected internal virtual ICircularReferenceTracker CircularReferenceTracker
		{
			get { return this._circularReferenceTracker; }
		}

		public virtual bool DecideIfAnInstanceIsSerializableByActuallySerializingIt
		{
			get { return this._decideIfAnInstanceIsSerializableByActuallySerializingIt; }
			set { this.SerializationResolver.DecideIfAnInstanceIsSerializableByActuallySerializingIt = this._decideIfAnInstanceIsSerializableByActuallySerializingIt = value; }
		}

		public new virtual T Instance
		{
			get { return (T) base.Instance; }
			protected internal set { base.Instance = value; }
		}

		protected internal virtual ISerializationResolver SerializationResolver
		{
			get { return this._serializationResolver; }
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

			this.SerializationResolver.DecideIfAnInstanceIsSerializableByActuallySerializingIt = this.DecideIfAnInstanceIsSerializableByActuallySerializingIt;

			var instancesReferencingCircularReference = new List<Serializable>();
			this.SetInstance(this.SerializationResolver, this.CircularReferenceTracker, instancesReferencingCircularReference);

			base.OnDeserialized(streamingContext);

			this.ClearCircularReferenceTracking();
		}

		protected internal virtual void OnDeserializing(StreamingContext streamingContext)
		{
			this._circularReferenceTracker = ServiceLocator.Instance.GetService<ICircularReferenceTracker>();
			this._serializationResolver = ServiceLocator.Instance.GetService<ISerializationResolver>();
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
			this.SetSerializableInstance(this.SerializationResolver, this.CircularReferenceTracker);

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