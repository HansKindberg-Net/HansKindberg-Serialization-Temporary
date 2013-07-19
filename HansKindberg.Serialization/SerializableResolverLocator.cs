using Castle.DynamicProxy;

namespace HansKindberg.Serialization
{
	public static class SerializableResolverLocator
	{
		#region Fields

		private static volatile ISerializableResolverLocator _instance;
		private static readonly object _lockObject = new object();

		#endregion

		#region Properties

		public static ISerializableResolverLocator Instance
		{
			get
			{
				if(_instance == null)
				{
					lock(_lockObject)
					{
						if(_instance == null)
						{
							_instance = new DefaultSerializableResolverLocator(new DefaultProxyBuilder());
						}
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