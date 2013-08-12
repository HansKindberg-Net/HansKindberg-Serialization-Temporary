using System.Runtime.Serialization;

namespace HansKindberg.Serialization
{
	public interface ISerializableResolver
	{
		#region Methods

		T InstanceFromSerializationInformation<T>(SerializationInfo serializationInformation, StreamingContext streamingContext, string index);
		void InstanceToSerializationInformation<T>(T instance, SerializationInfo serializationInformation, StreamingContext streamingContext, string index);

		#endregion
	}
}