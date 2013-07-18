using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using HansKindberg.Serialization.IntegrationTests.Helpers.Extensions;
using HansKindberg.Serialization.IntegrationTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HansKindberg.Serialization.IntegrationTests
{
	[TestClass]
	public class SerializableTest
	{
		#region Methods

		[TestMethod]
		public void BinaryFormatter_SerializeAndDeserialize_IfTheSerializableResolverIsSerializable_ShouldNotThrowASerializationException()
		{
			const string instance = "Test";
			Serializable<object> serializable = new Serializable<object>(new object(), new SerializableSerializableResolverMock(instance));
			string binarySerializedSerializable = serializable.SerializeBinary();
			Serializable<object> deserializedSerializable = (Serializable<object>) ObjectExtension.DeserializeBinary(binarySerializedSerializable);
			Assert.AreEqual(instance, deserializedSerializable.Instance);
		}

		[TestMethod]
		[ExpectedException(typeof(SerializationException))]
		public void BinaryFormatter_Serialize_IfTheSerializableResolverIsNotSerializable_ShouldThrowASerializationException()
		{
			Serializable<object> serializable = new Serializable<object>(new object(), Mock.Of<ISerializableResolver>());
			// ReSharper disable PossibleNullReferenceException
			serializable.GetType().GetField("_serializableResolver", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(serializable, new UnserializableSerializableResolverMock());
			// ReSharper restore PossibleNullReferenceException
			IFormatter formatter = new BinaryFormatter();

			using(MemoryStream memoryStream = new MemoryStream())
			{
				formatter.Serialize(memoryStream, serializable);
			}
		}

		[TestMethod]
		public void HttpContext_ShouldBeSerializable() {}

		[TestMethod]
		public void HttpPostedFile_ShouldBeSerializable() {}

		[TestMethod]
		public void HttpRequest_ShouldBeSerializable()
		{
			const string filename = "Default.html";
			const string relativePath = "/" + filename;

			HttpRequest httpRequest = new HttpRequest(filename, "http://localhost" + relativePath, string.Empty);
			Assert.AreEqual(relativePath, httpRequest.FilePath);
			Assert.AreEqual(relativePath, httpRequest.RawUrl);
			Assert.AreEqual("http://localhost" + relativePath, httpRequest.Url.ToString());

			Serializable<HttpRequest> serializableHttpRequest = new Serializable<HttpRequest>(httpRequest, new DefaultSerializableResolver());
			string binarySerializedSerializableHttpRequest = serializableHttpRequest.SerializeBinary();
			Serializable<HttpRequest> deserializedSerializableHttpRequest = (Serializable<HttpRequest>) ObjectExtension.DeserializeBinary(binarySerializedSerializableHttpRequest);

			HttpRequest deserializedHttpRequest = deserializedSerializableHttpRequest.Instance;
			Assert.AreEqual(httpRequest.FilePath, deserializedHttpRequest.FilePath);
			Assert.AreEqual(httpRequest.RawUrl, deserializedHttpRequest.RawUrl);
			Assert.AreEqual(httpRequest.Url.ToString(), deserializedHttpRequest.Url.ToString());
		}

		[TestMethod]
		public void MailMessage_ShouldBeSerializable()
		{
			string binarySerializedSerializableMailMessage;

			using(MailMessage mailMessage = new MailMessage("from@company.com", "first@company.com"))
			{
				using(MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("Test")))
				{
					mailMessage.Attachments.Add(new Attachment(memoryStream, new ContentType(MediaTypeNames.Text.Html)));
					mailMessage.Body = "<html><body><h1>Body</h1></body></html>";
					mailMessage.BodyEncoding = Encoding.UTF8;
					mailMessage.Headers.Add("Header", "HeaderValue");
					mailMessage.IsBodyHtml = true;
					mailMessage.Priority = MailPriority.High;
					mailMessage.Subject = "Subject";
					mailMessage.To.Add("second@company.com");
					mailMessage.To.Add("third@company.com");

					Serializable<MailMessage> serializableMailMessage = new Serializable<MailMessage>(mailMessage, new DefaultSerializableResolver());
					binarySerializedSerializableMailMessage = serializableMailMessage.SerializeBinary();
				}
			}

			Serializable<MailMessage> deserializedSerializableMailMessage = (Serializable<MailMessage>) ObjectExtension.DeserializeBinary(binarySerializedSerializableMailMessage);

			using(MailMessage deserializedMailMessage = deserializedSerializableMailMessage.Instance)
			{
				Assert.IsNotNull(deserializedMailMessage);
			}
		}

		[TestMethod]
		public void PrerequisiteTest_BinaryFormatter_SerializeAndDeserialize_WithMemoryStream()
		{
			const string value = "Test";
			string binarySerializedObject = value.SerializeBinary();
			Assert.AreEqual(value, ObjectExtension.DeserializeBinary(binarySerializedObject));
		}

		#endregion
	}
}