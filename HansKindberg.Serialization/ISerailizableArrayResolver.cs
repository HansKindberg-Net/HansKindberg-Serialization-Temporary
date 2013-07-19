using System.Runtime.Serialization;

namespace HansKindberg.Serialization
{
	public interface ISerializableArrayResolver
	{
		#region Methods

		T GetArray<T>(SerializationInfo serializationInformation, StreamingContext streamingContext, string index);
		void SetArray<T>(T array, SerializationInfo serializationInformation, StreamingContext streamingContext, string index);

		#endregion
	}
}