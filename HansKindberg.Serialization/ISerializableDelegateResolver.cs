using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace HansKindberg.Serialization
{
	public interface ISerializableDelegateResolver
	{
		#region Methods

		T GetDelegate<T>(SerializationInfo serializationInformation, StreamingContext streamingContext, string index);

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "delegate")]
		void SetDelegate<T>(T @delegate, SerializationInfo serializationInformation, StreamingContext streamingContext, string index);

		#endregion
	}
}