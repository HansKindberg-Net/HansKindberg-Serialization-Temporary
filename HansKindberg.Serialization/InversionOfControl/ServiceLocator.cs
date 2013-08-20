using Castle.DynamicProxy;

namespace HansKindberg.Serialization.InversionOfControl
{
	public static class ServiceLocator
	{
		#region Fields

		private static volatile IServiceLocator _instance;
		private static readonly object _lockObject = new object();

		#endregion

		#region Properties

		public static IServiceLocator Instance
		{
			get
			{
				if(_instance == null)
				{
					lock(_lockObject)
					{
						if(_instance == null)
							_instance = new DefaultServiceLocator(new DefaultSerializationResolver(new DefaultProxyBuilder()));
					}
				}

				return _instance;
			}
			set
			{
				if(value == _instance)
					return;

				lock(_lockObject)
				{
					_instance = value;
				}
			}
		}

		#endregion
	}
}