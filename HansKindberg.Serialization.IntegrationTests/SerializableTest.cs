using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Serialization.IntegrationTests
{
	[TestClass]
	public class SerializableTest
	{
		#region Methods

		[TestMethod]
		public void HttpContext_ShouldBeSerializable() {}

		[TestMethod]
		public void HttpPostedFile_ShouldBeSerializable() {}

		[TestMethod]
		public void HttpRequest_ShouldBeSerializable()
		{
			//HttpRequest httpRequest = new HttpRequest("Default.html", "http://localhost/Default.html", string.Empty);

			//string serializedHttpRequest = httpRequest.Serialize();

			//Assert.IsNotNull(serializedHttpRequest);
		}

		#endregion
	}

	public static class SerializationExtensions
	{
		#region Methods

		[SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		public static T Deserialize<T>(this string serialized)
		{
			DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T));
			using(StringReader stringReader = new StringReader(serialized))
			{
				using(XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
				{
					return (T) dataContractSerializer.ReadObject(xmlTextReader);
				}
			}
		}

		[SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		public static string Serialize<T>(this T value)
		{
			DataContractSerializer dataContractSerializer = new DataContractSerializer(value.GetType());
			using(StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using(XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
				{
					dataContractSerializer.WriteObject(xmlTextWriter, value);
					return stringWriter.ToString();
				}
			}
		}

		#endregion
	}
}