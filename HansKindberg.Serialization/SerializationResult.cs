using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace HansKindberg.Serialization
{
	[Serializable]
	public class SerializationResult
	{
		#region Fields

		private readonly object _instance;
		private readonly Type _instanceType;
		private readonly SerializationException _serializationException;
		private readonly string _serializationString;

		#endregion

		#region Constructors

		protected internal SerializationResult(object instance)
		{
			this._instance = instance;
			this._instanceType = instance != null ? instance.GetType() : null;
		}

		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string")]
		public SerializationResult(object instance, string serializationString) : this(instance)
		{
			this._serializationString = serializationString;
		}

		public SerializationResult(object instance, SerializationException serializationException) : this(instance)
		{
			this._serializationException = serializationException;
		}

		#endregion

		#region Properties

		public virtual object Instance
		{
			get { return this._instance; }
		}

		public virtual Type InstanceType
		{
			get { return this._instanceType; }
		}

		public virtual bool IsSerializable
		{
			get { return this._serializationException == null; }
		}

		public virtual SerializationException SerializationException
		{
			get { return this._serializationException; }
		}

		public virtual string SerializationString
		{
			get { return this._serializationString; }
		}

		#endregion
	}
}