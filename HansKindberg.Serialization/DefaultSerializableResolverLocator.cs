using System;
using Castle.DynamicProxy;

namespace HansKindberg.Serialization
{
	public class DefaultSerializableResolverLocator : ISerializableResolverLocator
	{
		#region Fields

		private readonly ISerializableArrayResolver _serializableArrayResolver;
		private readonly ISerializableDelegateResolver _serializableDelegateResolver;
		private readonly ISerializableResolver _serializableResolver;

		#endregion

		#region Constructors

		public DefaultSerializableResolverLocator(IProxyBuilder proxyBuilder)
		{
			if(proxyBuilder == null)
				throw new ArgumentNullException("proxyBuilder");

			this._serializableArrayResolver = new DefaultSerializableArrayResolver(proxyBuilder);
			this._serializableDelegateResolver = new DefaultSerializableDelegateResolver(proxyBuilder);
			this._serializableResolver = new DefaultSerializableResolver(proxyBuilder);
		}

		#endregion

		#region Properties

		public virtual ISerializableArrayResolver SerializableArrayResolver
		{
			get { return this._serializableArrayResolver; }
		}

		public virtual ISerializableDelegateResolver SerializableDelegateResolver
		{
			get { return this._serializableDelegateResolver; }
		}

		public virtual ISerializableResolver SerializableResolver
		{
			get { return this._serializableResolver; }
		}

		#endregion
	}
}