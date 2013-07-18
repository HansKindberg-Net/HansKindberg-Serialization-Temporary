using System;
using System.Runtime.Serialization;

namespace HansKindberg.Serialization.Tests.Mocks
{
	public class UnserializableSerializableResolverMock : ISerializableResolver
	{
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
			return default(T);
		}

		public virtual void SetInstance<T>(T instance, SerializationInfo serializationInformation) {}

		#endregion
	}
}