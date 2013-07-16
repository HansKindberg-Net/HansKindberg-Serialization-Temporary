using System;
using System.Runtime.Serialization;
using Castle.DynamicProxy;

namespace HansKindberg.Serialization
{
	public class DefaultSerializableFactory : ISerializableFactory
	{
		#region Fields

		private readonly IProxyBuilder _proxyBuilder;

		#endregion

		#region Constructors

		public DefaultSerializableFactory(IProxyBuilder proxyBuilder)
		{
			if(proxyBuilder == null)
				throw new ArgumentNullException("proxyBuilder");

			this._proxyBuilder = proxyBuilder;
		}

		#endregion

		#region Properties

		protected internal virtual IProxyBuilder ProxyBuilder
		{
			get { return this._proxyBuilder; }
		}

		#endregion

		#region Methods

		public virtual T CreateUninitializedObject<T>()
		{
			return (T) this.CreateUninitializedObject(typeof(T));
		}

		public virtual object CreateUninitializedObject(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			if(typeof(string).IsAssignableFrom(type))
				return string.Empty;

			if(type.IsInterface)
				type = this.ProxyBuilder.CreateInterfaceProxyTypeWithoutTarget(type, null, ProxyGenerationOptions.Default);
			else if(type.IsAbstract)
				type = this.ProxyBuilder.CreateClassProxyType(type, null, ProxyGenerationOptions.Default);

			return FormatterServices.GetUninitializedObject(type);
		}

		#endregion
	}
}