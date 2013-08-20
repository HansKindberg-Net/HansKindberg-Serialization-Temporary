using System;

namespace HansKindberg.Serialization.InversionOfControl
{
	public class DefaultServiceLocator : IServiceLocator
	{
		#region Fields

		private readonly ISerializationResolver _serializationResolver;

		#endregion

		#region Constructors

		public DefaultServiceLocator(ISerializationResolver serializationResolver)
		{
			if(serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			this._serializationResolver = serializationResolver;
		}

		#endregion

		#region Properties

		protected internal virtual ISerializationResolver SerializationResolver
		{
			get { return this._serializationResolver; }
		}

		#endregion

		#region Methods

		public virtual object GetService(Type serviceType)
		{
			return typeof(ISerializationResolver) == serviceType ? this.SerializationResolver : null;
		}

		public virtual T GetService<T>()
		{
			return (T) this.GetService(typeof(T));
		}

		#endregion
	}
}