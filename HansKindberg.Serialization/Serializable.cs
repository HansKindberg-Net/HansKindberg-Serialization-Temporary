using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
	/// </summary>
	[Serializable]
	public class Serializable<T> : ISerializable
	{
		#region Fields

		private readonly T _instance;
		private readonly ISerializableResolver _serializableResolver;
		private const string _serializableResolverSerializationInformationName = "SerializableResolver";

		#endregion

		#region Constructors

		public Serializable(T instance, ISerializableResolver serializableResolver)
		{
			if(Equals(instance, null))
				throw new ArgumentNullException("instance");

			if(serializableResolver == null)
				throw new ArgumentNullException("serializableResolver");

			if(!serializableResolver.GetType().IsSerializable)
				throw new ArgumentException("The serializable resolver has to be serializable.", "serializableResolver");

			this._instance = instance;
			this._serializableResolver = serializableResolver;
		}

		protected Serializable(SerializationInfo info, StreamingContext context)
		{
			if(info == null)
				throw new ArgumentNullException("info");

			try
			{
				// The serializable resolver have to be serializable. We could also handle it by using some kind of service-locator for the serializable resolver here instead.
				this._serializableResolver = (ISerializableResolver) info.GetValue(_serializableResolverSerializationInformationName, typeof(ISerializableResolver));
			}
			catch(Exception exception)
			{
				throw new ArgumentException("The serializable resolver could not be retrieved from the serialization-information.", "info", exception);
			}

			this._instance = this._serializableResolver.GetInstance<T>(info);
		}

		#endregion

		#region Properties

		public virtual T Instance
		{
			get { return this._instance; }
		}

		protected internal virtual ISerializableResolver SerializableResolver
		{
			get { return this._serializableResolver; }
		}

		#endregion

		#region Methods

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if(info == null)
				throw new ArgumentNullException("info");

			info.AddValue(_serializableResolverSerializationInformationName, this.SerializableResolver);

			this.SerializableResolver.SetInstance(this.Instance, info);
		}

		#endregion
	}
}