using System.Collections.Generic;
using System.Linq;
using HansKindberg.Serialization.IntegrationTests.Helpers.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Serialization.IntegrationTests
{
	[TestClass]
	public class SerializableArrayTest
	{
		#region Methods

		[TestMethod]
		public void System_StringArray_ShouldBeSerializable()
		{
			SerializableArray<string[]> serializableArray = new SerializableArray<string[]>(new[] {"First", "Second", "Third"});
			string binarySerializedSerializableArray = serializableArray.SerializeBinary();

			Assert.IsNotNull(binarySerializedSerializableArray);

			SerializableArray<string[]> deserializedSerializableArray = (SerializableArray<string[]>) ObjectExtension.DeserializeBinary(binarySerializedSerializableArray);

			IEnumerable<string> deserializedArray = deserializedSerializableArray.Array.ToArray();

			Assert.AreEqual(3, deserializedArray.Count());
			Assert.AreEqual("First", deserializedArray.ElementAt(0));
			Assert.AreEqual("Second", deserializedArray.ElementAt(1));
			Assert.AreEqual("Third", deserializedArray.ElementAt(2));
		}

		#endregion
	}
}