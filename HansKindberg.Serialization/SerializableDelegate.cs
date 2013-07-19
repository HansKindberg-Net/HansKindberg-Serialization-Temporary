using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// Makes delegates serializable where possible.
	/// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
	/// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public class SerializableDelegate : SerializableDelegate<object>
	{
		#region Constructors

		public SerializableDelegate(object @delegate) : base(@delegate) {}
		protected internal SerializableDelegate(object @delegate, ISerializableDelegateResolver serializableDelegateResolver) : base(@delegate, serializableDelegateResolver) {}
		protected SerializableDelegate(SerializationInfo info, StreamingContext context) : base(info, context) {}
		protected internal SerializableDelegate(SerializationInfo info, StreamingContext context, string index) : base(info, context, index) {}
		protected internal SerializableDelegate(SerializationInfo info, StreamingContext context, ISerializableDelegateResolver serializableDelegateResolver) : base(info, context, serializableDelegateResolver) {}
		protected internal SerializableDelegate(SerializationInfo info, StreamingContext context, ISerializableDelegateResolver serializableDelegateResolver, string index) : base(info, context, serializableDelegateResolver, index) {}

		#endregion
	}

	/// <summary>
	/// Makes delegates serializable where possible.
	/// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
	/// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public class SerializableDelegate<T> : ISerializable
	{
		#region Fields

		private readonly T _delegate;
		private ISerializableDelegateResolver _serializableDelegateResolver;

		#endregion

		#region Constructors

		public SerializableDelegate(T @delegate)
		{
			if(Equals(@delegate, null))
				throw new ArgumentNullException("delegate");

			Type type = @delegate.GetType();

			if(!typeof(Delegate).IsAssignableFrom(type))
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The type \"{0}\" must be a delegate ({1}).", type.FullName, typeof(Delegate)), "delegate");

			this._delegate = @delegate;
		}

		protected internal SerializableDelegate(T @delegate, ISerializableDelegateResolver serializableDelegateResolver) : this(@delegate)
		{
			if(serializableDelegateResolver == null)
				throw new ArgumentNullException("serializableDelegateResolver");

			this._serializableDelegateResolver = serializableDelegateResolver;
		}

		protected SerializableDelegate(SerializationInfo info, StreamingContext context) : this(info, context, string.Empty) {}

		protected internal SerializableDelegate(SerializationInfo info, StreamingContext context, string index)
		{
			this._delegate = this.SerializableDelegateResolver.GetDelegate<T>(info, context, index);
		}

		protected internal SerializableDelegate(SerializationInfo info, StreamingContext context, ISerializableDelegateResolver serializableDelegateResolver) : this(info, context, serializableDelegateResolver, string.Empty) {}

		protected internal SerializableDelegate(SerializationInfo info, StreamingContext context, ISerializableDelegateResolver serializableDelegateResolver, string index)
		{
			if(serializableDelegateResolver == null)
				throw new ArgumentNullException("serializableDelegateResolver");

			this._serializableDelegateResolver = serializableDelegateResolver;

			this._delegate = this.SerializableDelegateResolver.GetDelegate<T>(info, context, index);
		}

		#endregion

		#region Properties

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Delegate")]
		public virtual T Delegate
		{
			get { return this._delegate; }
		}

		protected internal ISerializableDelegateResolver SerializableDelegateResolver
		{
			get { return this._serializableDelegateResolver ?? (this._serializableDelegateResolver = SerializableResolverLocator.Instance.SerializableDelegateResolver); }
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
			this.SerializableDelegateResolver.SetDelegate(this.Delegate, info, context, index);
		}

		#endregion
	}
}