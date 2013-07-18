using System;
using System.Runtime.Serialization;

namespace HansKindberg.Serialization.IntegrationTests.Mocks
{
	[Serializable]
	public class SerializableSerializableResolverMock : ISerializableResolver
	{
		#region Fields

		private readonly object _instance;

		#endregion

		#region Constructors

		public SerializableSerializableResolverMock(object instance)
		{
			this._instance = instance;
		}

		#endregion

		#region Methods

		public virtual T CreateUninitializedObject<T>()
		{
			return default(T);
		}

		public virtual object CreateUninitializedObject(Type type)
		{
			return null;
		}

		public virtual T GetInstance<T>(SerializationInfo serializationInformation)
		{
			return (T) this._instance;
		}

		public virtual void SetInstance<T>(T instance, SerializationInfo serializationInformation) {}

		#endregion
	}
}