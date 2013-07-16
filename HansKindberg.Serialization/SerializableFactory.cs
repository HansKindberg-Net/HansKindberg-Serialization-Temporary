using Castle.DynamicProxy;

namespace HansKindberg.Serialization
{
	public static class SerializableFactory
	{
		#region Fields

		private static volatile ISerializableFactory _instance;
		private static readonly object _lockObject = new object();

		#endregion

		#region Properties

		public static ISerializableFactory Instance
		{
			get
			{
				if(_instance == null)
				{
					lock(_lockObject)
					{
						if(_instance == null)
							_instance = new DefaultSerializableFactory(new DefaultProxyBuilder());
					}
				}

				return _instance;
			}
			set
			{
				lock(_lockObject)
				{
					if(value == _instance)
						return;

					_instance = value;
				}
			}
		}

		#endregion
	}
}