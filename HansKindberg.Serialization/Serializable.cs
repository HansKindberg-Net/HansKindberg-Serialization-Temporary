using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
	/// </summary>
	[Serializable]
	public class Serializable : Serializable<object>
	{
		#region Constructors

		public Serializable(object instance) : base(instance) {}
		protected internal Serializable(object instance, ISerializableResolver serializableResolver) : base(instance, serializableResolver) {}
		protected Serializable(SerializationInfo info, StreamingContext context) : base(info, context) {}
		protected internal Serializable(SerializationInfo info, StreamingContext context, string index) : base(info, context, index) {}
		protected internal Serializable(SerializationInfo info, StreamingContext context, ISerializableResolver serializableResolver) : base(info, context, serializableResolver) {}
		protected internal Serializable(SerializationInfo info, StreamingContext context, ISerializableResolver serializableResolver, string index) : base(info, context, serializableResolver, index) {}

		#endregion
	}

	/// <summary>
	/// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
	/// </summary>
	[Serializable]
	public class Serializable<T> : ISerializable
	{
		#region Fields

		private readonly T _instance;
		private ISerializableResolver _serializableResolver;

		#endregion

		#region Constructors

		public Serializable(T instance)
		{
			if(Equals(instance, null))
				throw new ArgumentNullException("instance");

			this._instance = instance;
		}

		protected internal Serializable(T instance, ISerializableResolver serializableResolver) : this(instance)
		{
			if(serializableResolver == null)
				throw new ArgumentNullException("serializableResolver");

			this._serializableResolver = serializableResolver;
		}

		protected Serializable(SerializationInfo info, StreamingContext context) : this(info, context, string.Empty) {}

		protected internal Serializable(SerializationInfo info, StreamingContext context, string index)
		{
			this._instance = this.SerializableResolver.InstanceFromSerializationInformation<T>(info, context, index);
		}

		protected internal Serializable(SerializationInfo info, StreamingContext context, ISerializableResolver serializableResolver) : this(info, context, serializableResolver, string.Empty) {}

		protected internal Serializable(SerializationInfo info, StreamingContext context, ISerializableResolver serializableResolver, string index)
		{
			if(serializableResolver == null)
				throw new ArgumentNullException("serializableResolver");

			this._serializableResolver = serializableResolver;

			this._instance = this.SerializableResolver.InstanceFromSerializationInformation<T>(info, context, index);
		}

		#endregion

		#region Properties

		public virtual T Instance
		{
			get { return this._instance; }
		}

		protected internal ISerializableResolver SerializableResolver
		{
			get { return this._serializableResolver ?? (this._serializableResolver = SerializableResolverLocator.Instance.SerializableResolver); }
		}

		#endregion

		#region Methods

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			this.GetObjectData(info, context, string.Empty);
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context, string index)
		{
			this.SerializableResolver.InstanceToSerializationInformation(this.Instance, info, context, index);
		}

		#endregion
	}
}