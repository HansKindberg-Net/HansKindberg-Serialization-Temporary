using HansKindberg.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TemporaryTests.Helpers.Extensions;

namespace TemporaryTests
{
	[TestClass]
	public class UnitTest1
	{
		#region Methods

		[TestMethod]
		public void Unserializable_ShouldNotBeSerializable()
		{
			Unserializable[] unserializableArray = new[] {new Unserializable(), new Unserializable(), new Unserializable()};
			Serializable<Unserializable[]> serializableUnserializableArray = new Serializable<Unserializable[]>(unserializableArray);
			string binarySerializedUnserializableArray = serializableUnserializableArray.SerializeBinary();

			Serializable<Unserializable[]> deserializedSerializableUnserializableArray = (Serializable<Unserializable[]>) ObjectExtension.DeserializeBinary(binarySerializedUnserializableArray);
			Unserializable[] deserializedUnserializableArray = deserializedSerializableUnserializableArray.Instance;

			Assert.AreEqual(3, deserializedUnserializableArray.Length);
			Assert.IsNotNull(deserializedUnserializableArray[0]);
			Assert.IsNotNull(deserializedUnserializableArray[1]);
			Assert.IsNotNull(deserializedUnserializableArray[2]);

			//Assert.AreEqual(value, ObjectExtension.DeserializeBinary(binarySerializedObject));
		}

		#endregion
	}

	public class Unserializable {}
}