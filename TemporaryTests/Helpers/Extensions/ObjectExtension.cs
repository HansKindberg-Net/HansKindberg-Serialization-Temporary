using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TemporaryTests.Helpers.Extensions
{
	public static class ObjectExtension
	{
		#region Methods

		public static object DeserializeBinary(string binarySerializedObject)
		{
			if(binarySerializedObject == null)
				throw new ArgumentNullException("binarySerializedObject");

			using(MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(binarySerializedObject)))
			{
				return new BinaryFormatter().Deserialize(memoryStream);
			}
		}

		public static string SerializeBinary(this object value)
		{
			if(value == null)
				throw new ArgumentNullException("value");

			using(MemoryStream memoryStream = new MemoryStream())
			{
				new BinaryFormatter().Serialize(memoryStream, value);
				return Convert.ToBase64String(memoryStream.ToArray());
			}
		}

		#endregion
	}
}