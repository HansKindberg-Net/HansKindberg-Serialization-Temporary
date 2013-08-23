using System;
using Castle.DynamicProxy;

namespace HansKindberg.Serialization.InversionOfControl
{
	public class DefaultServiceLocator : IServiceLocator
	{
		#region Fields

		private readonly IMemoryFormatterFactory _memoryFormatterFactory;
		private readonly IProxyBuilder _proxyBuilder;
		private readonly ISerializationResolver _serializationResolver;

		#endregion

		#region Constructors

		public DefaultServiceLocator()
		{
			this._memoryFormatterFactory = new DefaultMemoryFormatterFactory();
			this._proxyBuilder = new DefaultProxyBuilder();
			this._serializationResolver = new DefaultSerializationResolver(this._proxyBuilder, this._memoryFormatterFactory);
		}

		#endregion

		#region Properties

		protected internal virtual IMemoryFormatterFactory MemoryFormatterFactory
		{
			get { return this._memoryFormatterFactory; }
		}

		protected internal virtual IProxyBuilder ProxyBuilder
		{
			get { return this._proxyBuilder; }
		}

		protected internal virtual ISerializationResolver SerializationResolver
		{
			get { return this._serializationResolver; }
		}

		#endregion

		#region Methods

		public virtual object GetService(Type serviceType)
		{
			if(typeof(ICircularReferenceTracker) == serviceType)
				return new DefaultCircularReferenceTracker();

			if(typeof(IMemoryFormatterFactory) == serviceType)
				return this.MemoryFormatterFactory;

			if(typeof(IProxyBuilder) == serviceType)
				return this.ProxyBuilder;

			if(typeof(ISerializationResolver) == serviceType)
				return this.SerializationResolver;

			return null;
		}

		public virtual T GetService<T>()
		{
			return (T) this.GetService(typeof(T));
		}

		#endregion
	}
}