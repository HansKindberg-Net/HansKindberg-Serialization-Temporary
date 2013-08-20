using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using HansKindberg.Serialization.InversionOfControl;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// This code/idea is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
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
			if(this.SerializableInstance is Delegate)
				return (T) this.SerializableInstance;

			SerializableDelegate serializableDelegate = (SerializableDelegate) this.SerializableInstance;

			return (T) (object) Delegate.CreateDelegate(this.InstanceType, serializableDelegate.Instance, serializableDelegate.MethodInformation);
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
			Delegate @delegate = (Delegate) (object) this.Instance;

			if(@delegate == null)
				return @delegate;

			if(@delegate.Target == null)
				return @delegate;

			if(@delegate.Method == null)
				return @delegate;

			if(@delegate.Method.DeclaringType == null)
				return @delegate;

			if(this.SerializationResolver.IsSerializable(@delegate.Method.DeclaringType))
				return @delegate;

			return new SerializableDelegate(@delegate.Method, @delegate.Target, this.SerializationResolver);
		}

		protected internal virtual IEnumerable<SerializableField> CreateSerializableFields()
		{
			List<SerializableField> serializableFields = new List<SerializableField>();

			if(this.InstanceType != null)
				serializableFields.AddRange(this.SerializationResolver.GetFieldsToSerialize(this.InstanceType).Select(fieldInfo => new SerializableField(fieldInfo, fieldInfo.GetValue(this.Instance), this.SerializationResolver)));

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

		protected internal virtual void OnDeserializing(StreamingContext streamingContext)
		{
			this._serializationResolver = ServiceLocator.Instance.GetService<ISerializationResolver>();
		}

		[OnDeserializing]
		private void OnDeserializingInternal(StreamingContext streamingContext)
		{
			this.OnDeserializing(streamingContext);
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
}