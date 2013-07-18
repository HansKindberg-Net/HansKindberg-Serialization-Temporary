using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace HansKindberg.Serialization.Tests.Mocks
{
	[Serializable]
	[SuppressMessage("Microsoft.Usage", "CA2229:ImplementSerializationConstructors")]
	public class SerializableMock<T> : Serializable<T>
	{
		#region Constructors

		public SerializableMock(T instance, ISerializableResolver serializableResolver) : base(instance, serializableResolver) {}
		public SerializableMock(SerializationInfo info, StreamingContext context) : base(info, context) {}

		#endregion
	}
}