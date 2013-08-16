using System;
using Castle.DynamicProxy;

namespace HansKindberg.Serialization
{
	public class DefaultSerializableResolverLocator : ISerializableResolverLocator
	{
		#region Fields

		private readonly ISerializableResolver _serializableResolver;

		#endregion

		#region Constructors

		public DefaultSerializableResolverLocator(IProxyBuilder proxyBuilder)
		{
			if(proxyBuilder == null)
				throw new ArgumentNullException("proxyBuilder");

			this._serializableResolver = new DefaultSerializableResolver(proxyBuilder);
		}

		#endregion

		#region Properties


		public virtual ISerializableResolver SerializableResolver
		{
			get { return this._serializableResolver; }
		}

		#endregion
	}
}