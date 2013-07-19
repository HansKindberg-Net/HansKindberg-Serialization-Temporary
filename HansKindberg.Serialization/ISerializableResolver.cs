using System.Runtime.Serialization;

namespace HansKindberg.Serialization
{
	public interface ISerializableResolver
	{
		#region Methods

		T GetInstance<T>(SerializationInfo serializationInformation, StreamingContext streamingContext, string index);
		void SetInstance<T>(T instance, SerializationInfo serializationInformation, StreamingContext streamingContext, string index);

		#endregion
	}
}