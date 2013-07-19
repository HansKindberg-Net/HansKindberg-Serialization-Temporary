using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
	/// </summary>
	[Serializable]
	public class SerializableArray : SerializableArray<object>
	{
		#region Constructors

		public SerializableArray(object array) : base(array) {}
		protected internal SerializableArray(object array, ISerializableArrayResolver serializableArrayResolver) : base(array, serializableArrayResolver) {}
		protected SerializableArray(SerializationInfo info, StreamingContext context) : base(info, context) {}
		protected internal SerializableArray(SerializationInfo info, StreamingContext context, string index) : base(info, context, index) {}
		protected internal SerializableArray(SerializationInfo info, StreamingContext context, ISerializableArrayResolver serializableArrayResolver) : base(info, context, serializableArrayResolver) {}
		protected internal SerializableArray(SerializationInfo info, StreamingContext context, ISerializableArrayResolver serializableArrayResolver, string index) : base(info, context, serializableArrayResolver, index) {}

		#endregion
	}

	/// <summary>
	/// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
	/// </summary>
	[Serializable]
	public class SerializableArray<T> : ISerializable
	{
		#region Fields

		private readonly T _array;
		private ISerializableArrayResolver _serializableArrayResolver;

		#endregion

		#region Constructors

		public SerializableArray(T array)
		{
			if(Equals(array, null))
				throw new ArgumentNullException("array");

			Type type = array.GetType();

			if(!type.IsArray)
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The type \"{0}\" must be an array.", type.FullName), "array");

			this._array = array;
		}

		protected internal SerializableArray(T array, ISerializableArrayResolver serializableArrayResolver) : this(array)
		{
			if(serializableArrayResolver == null)
				throw new ArgumentNullException("serializableArrayResolver");

			this._serializableArrayResolver = serializableArrayResolver;
		}

		protected SerializableArray(SerializationInfo info, StreamingContext context) : this(info, context, string.Empty) {}

		protected internal SerializableArray(SerializationInfo info, StreamingContext context, string index)
		{
			this._array = this.SerializableArrayResolver.GetArray<T>(info, context, index);
		}

		protected internal SerializableArray(SerializationInfo info, StreamingContext context, ISerializableArrayResolver serializableArrayResolver) : this(info, context, serializableArrayResolver, string.Empty) {}

		protected internal SerializableArray(SerializationInfo info, StreamingContext context, ISerializableArrayResolver serializableArrayResolver, string index)
		{
			if(serializableArrayResolver == null)
				throw new ArgumentNullException("serializableArrayResolver");

			this._serializableArrayResolver = serializableArrayResolver;

			this._array = this.SerializableArrayResolver.GetArray<T>(info, context, index);
		}

		#endregion

		#region Properties

		public virtual T Array
		{
			get { return this._array; }
		}

		protected internal ISerializableArrayResolver SerializableArrayResolver
		{
			get { return this._serializableArrayResolver ?? (this._serializableArrayResolver = SerializableResolverLocator.Instance.SerializableArrayResolver); }
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
			this.SerializableArrayResolver.SetArray(this.Array, info, context, index);
		}

		#endregion
	}
}