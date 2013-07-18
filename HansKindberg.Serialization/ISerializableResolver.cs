using System.Runtime.Serialization;

namespace HansKindberg.Serialization
{
	public interface ISerializableResolver
	{
		#region Methods

		T GetInstance<T>(SerializationInfo serializationInformation);
		void SetInstance<T>(T instance, SerializationInfo serializationInformation);

		#endregion
	}
}