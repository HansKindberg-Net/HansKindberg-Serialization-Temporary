using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
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
			Unserializable[] unserializableArray = new[]{new Unserializable(), new Unserializable(), new Unserializable()};
			SerializableArray serializableUnserializableArray = new SerializableArray(unserializableArray);
			string binarySerializedUnserializable = serializableUnserializableArray.SerializeBinary();
			//Assert.AreEqual(value, ObjectExtension.DeserializeBinary(binarySerializedObject));
		}

		//[TestMethod]
		//public void PrerequisiteTest_BinaryFormatter_SerializeAndDeserialize_WithMemoryStream()
		//{
		//	const string value = "Test";
		//	string binarySerializedObject = value.SerializeBinary();
		//	Assert.AreEqual(value, ObjectExtension.DeserializeBinary(binarySerializedObject));
		//}

		//[TestMethod]
		//public void System_Collections_ArrayList_ShouldBeSerializable()
		//{
		//	ArrayList arrayList = new ArrayList();
		//	arrayList.Add("Test");
		//	Serializable<ArrayList> serializableArrayList = new Serializable<ArrayList>(arrayList);
		//	string binarySerializedSerializableArrayList = serializableArrayList.SerializeBinary();
		//	Serializable<ArrayList> deserializedSerializableArrayList = (Serializable<ArrayList>) ObjectExtension.DeserializeBinary(binarySerializedSerializableArrayList);
		//	ArrayList deserializedArrayList = deserializedSerializableArrayList.Instance;
		//	Assert.IsNotNull(deserializedArrayList);
		//	Assert.AreEqual(1, deserializedArrayList.Count);
		//	Assert.AreEqual("Test", deserializedArrayList[0]);
		//}

		//[TestMethod]
		//public void System_Collections_Specialized_NameValueCollection_ShouldBeSerializable()
		//{
		//	NameValueCollection nameValueCollection = new NameValueCollection
		//		{
		//			{"FirstName", "FirstValue"},
		//			{"SecondName", "SecondValue"},
		//			{"ThirdName", "ThirdValue"}
		//		};
		//	Serializable<NameValueCollection> serializableNameValueCollection = new Serializable<NameValueCollection>(nameValueCollection);
		//	string binarySerializedSerializableNameValueCollection = serializableNameValueCollection.SerializeBinary();
		//	Serializable<NameValueCollection> deserializedSerializableNameValueCollection = (Serializable<NameValueCollection>) ObjectExtension.DeserializeBinary(binarySerializedSerializableNameValueCollection);
		//	NameValueCollection deserializedNameValueCollection = deserializedSerializableNameValueCollection.Instance;
		//	Assert.AreEqual(3, deserializedNameValueCollection.Count);
		//	Assert.AreEqual("FirstName", deserializedNameValueCollection.Keys[0]);
		//	Assert.AreEqual("FirstValue", deserializedNameValueCollection[0]);
		//	Assert.AreEqual("SecondName", deserializedNameValueCollection.Keys[1]);
		//	Assert.AreEqual("SecondValue", deserializedNameValueCollection[1]);
		//	Assert.AreEqual("ThirdName", deserializedNameValueCollection.Keys[2]);
		//	Assert.AreEqual("ThirdValue", deserializedNameValueCollection[2]);
		//}

		//[TestMethod]
		//public void System_Net_Mail_AttachmentCollection_ShouldBeSerializable()
		//{
		//	string binarySerializedSerializableAttachmentCollection;
		//	Encoding encoding = Encoding.UTF8;
		//	using(AttachmentCollection attachmentCollection = (AttachmentCollection) Activator.CreateInstance(typeof(AttachmentCollection), true))
		//	{
		//		using(MemoryStream firstMemoryStream = new MemoryStream(encoding.GetBytes("First")))
		//		{
		//			attachmentCollection.Add(new Attachment(firstMemoryStream, new ContentType(MediaTypeNames.Application.Pdf)));
		//			using(MemoryStream secondMemoryStream = new MemoryStream(encoding.GetBytes("Second")))
		//			{
		//				attachmentCollection.Add(new Attachment(secondMemoryStream, new ContentType(MediaTypeNames.Image.Gif)));
		//				using(MemoryStream thirdMemoryStream = new MemoryStream(encoding.GetBytes("Third")))
		//				{
		//					attachmentCollection.Add(new Attachment(thirdMemoryStream, new ContentType(MediaTypeNames.Text.Html)));
		//					Serializable<AttachmentCollection> serializableAttachmentCollection = new Serializable<AttachmentCollection>(attachmentCollection);
		//					binarySerializedSerializableAttachmentCollection = serializableAttachmentCollection.SerializeBinary();
		//				}
		//			}
		//		}
		//	}
		//	Serializable<AttachmentCollection> deserializedSerializableAttachmentCollection = (Serializable<AttachmentCollection>) ObjectExtension.DeserializeBinary(binarySerializedSerializableAttachmentCollection);
		//	using(AttachmentCollection deserializedAttachmentCollection = deserializedSerializableAttachmentCollection.Instance)
		//	{
		//		// ReSharper disable PossibleNullReferenceException
		//		Assert.AreEqual(3, deserializedAttachmentCollection.Count);
		//		Assert.AreEqual(new ContentType(MediaTypeNames.Application.Pdf).MediaType, deserializedAttachmentCollection[0].ContentType.MediaType);
		//		Assert.AreEqual(typeof(MemoryStream), deserializedAttachmentCollection[0].ContentStream.GetType());
		//		Assert.AreEqual("First", encoding.GetString((deserializedAttachmentCollection[0].ContentStream as MemoryStream).ToArray()));
		//		Assert.AreEqual(new ContentType(MediaTypeNames.Image.Gif).MediaType, deserializedAttachmentCollection[1].ContentType.MediaType);
		//		Assert.AreEqual(typeof(MemoryStream), deserializedAttachmentCollection[1].ContentStream.GetType());
		//		Assert.AreEqual("Second", encoding.GetString((deserializedAttachmentCollection[1].ContentStream as MemoryStream).ToArray()));
		//		Assert.AreEqual(new ContentType(MediaTypeNames.Text.Html).MediaType, deserializedAttachmentCollection[2].ContentType.MediaType);
		//		Assert.AreEqual(typeof(MemoryStream), deserializedAttachmentCollection[2].ContentStream.GetType());
		//		Assert.AreEqual("Third", encoding.GetString((deserializedAttachmentCollection[2].ContentStream as MemoryStream).ToArray()));
		//		// ReSharper restore PossibleNullReferenceException
		//	}
		//}

		//[TestMethod]
		//public void System_Net_Mail_Attachment_ShouldBeSerializable()
		//{
		//	string binarySerializedSerializableAttachment;
		//	using(MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("Test")))
		//	{
		//		Attachment attachment = new Attachment(memoryStream, new ContentType(MediaTypeNames.Text.Html));
		//		Serializable<Attachment> serializableAttachment = new Serializable<Attachment>(attachment);
		//		binarySerializedSerializableAttachment = serializableAttachment.SerializeBinary();
		//	}
		//	Serializable<Attachment> deserializedSerializableAttachment = (Serializable<Attachment>) ObjectExtension.DeserializeBinary(binarySerializedSerializableAttachment);
		//	using(Attachment deserializedAttachment = deserializedSerializableAttachment.Instance)
		//	{
		//		// ReSharper disable PossibleNullReferenceException
		//		Assert.AreEqual(new ContentType(MediaTypeNames.Text.Html).MediaType, deserializedAttachment.ContentType.MediaType);
		//		Assert.AreEqual(typeof(MemoryStream), deserializedAttachment.ContentStream.GetType());
		//		Assert.AreEqual("Test", Encoding.UTF8.GetString((deserializedAttachment.ContentStream as MemoryStream).ToArray()));
		//		// ReSharper restore PossibleNullReferenceException
		//	}
		//}

		//[TestMethod]
		//public void System_Net_Mail_MailMessage_ShouldBeSerializable()
		//{
		//	const string body = "<html><body><h1>Body</h1></body></html>";
		//	Encoding bodyEncoding = Encoding.UTF8;
		//	const bool isBodyHtml = true;
		//	const MailPriority priority = MailPriority.High;
		//	const string subject = "Subject";
		//	string binarySerializedSerializableMailMessage;
		//	using(MailMessage mailMessage = new MailMessage("from@company.com", "first@company.com"))
		//	{
		//		using(MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("Test")))
		//		{
		//			mailMessage.Attachments.Add(new Attachment(memoryStream, new ContentType(MediaTypeNames.Text.Html)));
		//			mailMessage.Body = body;
		//			mailMessage.BodyEncoding = bodyEncoding;
		//			mailMessage.Headers.Add("Header", "HeaderValue");
		//			mailMessage.IsBodyHtml = isBodyHtml;
		//			mailMessage.Priority = priority;
		//			mailMessage.Subject = subject;
		//			mailMessage.To.Add("second@company.com");
		//			mailMessage.To.Add("third@company.com");
		//			Serializable<MailMessage> serializableMailMessage = new Serializable<MailMessage>(mailMessage);
		//			binarySerializedSerializableMailMessage = serializableMailMessage.SerializeBinary();
		//			// ReSharper disable PossibleNullReferenceException
		//			Assert.IsNotNull(mailMessage);
		//			Assert.AreEqual(1, mailMessage.Attachments.Count);
		//			Assert.AreEqual(new ContentType(MediaTypeNames.Text.Html).MediaType, mailMessage.Attachments[0].ContentType.MediaType);
		//			Assert.AreEqual(typeof(MemoryStream), mailMessage.Attachments[0].ContentStream.GetType());
		//			Assert.AreEqual("Test", Encoding.UTF8.GetString((mailMessage.Attachments[0].ContentStream as MemoryStream).ToArray()));
		//			Assert.AreEqual(body, mailMessage.Body);
		//			Assert.AreEqual(bodyEncoding, mailMessage.BodyEncoding);
		//			Assert.AreEqual("from@company.com", mailMessage.From.Address);
		//			Assert.AreEqual(1, mailMessage.Headers.Count);
		//			Assert.AreEqual("Header", mailMessage.Headers.Keys[0]);
		//			Assert.AreEqual("HeaderValue", mailMessage.Headers[0]);
		//			Assert.AreEqual(isBodyHtml, mailMessage.IsBodyHtml);
		//			Assert.AreEqual(priority, mailMessage.Priority);
		//			Assert.AreEqual(subject, mailMessage.Subject);
		//			Assert.AreEqual(3, mailMessage.To.Count);
		//			Assert.AreEqual("first@company.com", mailMessage.To[0].Address);
		//			Assert.AreEqual("second@company.com", mailMessage.To[1].Address);
		//			Assert.AreEqual("third@company.com", mailMessage.To[2].Address);
		//			// ReSharper restore PossibleNullReferenceException
		//		}
		//	}
		//	Serializable<MailMessage> deserializedSerializableMailMessage = (Serializable<MailMessage>) ObjectExtension.DeserializeBinary(binarySerializedSerializableMailMessage);
		//	using(MailMessage deserializedMailMessage = deserializedSerializableMailMessage.Instance)
		//	{
		//		// ReSharper disable PossibleNullReferenceException
		//		Assert.IsNotNull(deserializedMailMessage);
		//		Assert.AreEqual(1, deserializedMailMessage.Attachments.Count);
		//		Assert.AreEqual(new ContentType(MediaTypeNames.Text.Html).MediaType, deserializedMailMessage.Attachments[0].ContentType.MediaType);
		//		Assert.AreEqual(typeof(MemoryStream), deserializedMailMessage.Attachments[0].ContentStream.GetType());
		//		Assert.AreEqual("Test", Encoding.UTF8.GetString((deserializedMailMessage.Attachments[0].ContentStream as MemoryStream).ToArray()));
		//		Assert.AreEqual(body, deserializedMailMessage.Body);
		//		Assert.AreEqual(bodyEncoding, deserializedMailMessage.BodyEncoding);
		//		Assert.AreEqual("from@company.com", deserializedMailMessage.From.Address);
		//		Assert.AreEqual(1, deserializedMailMessage.Headers.Count);
		//		Assert.AreEqual("Header", deserializedMailMessage.Headers.Keys[0]);
		//		Assert.AreEqual("HeaderValue", deserializedMailMessage.Headers[0]);
		//		Assert.AreEqual(isBodyHtml, deserializedMailMessage.IsBodyHtml);
		//		Assert.AreEqual(priority, deserializedMailMessage.Priority);
		//		Assert.AreEqual(subject, deserializedMailMessage.Subject);
		//		Assert.AreEqual(3, deserializedMailMessage.To.Count);
		//		Assert.AreEqual("first@company.com", deserializedMailMessage.To[0].Address);
		//		Assert.AreEqual("second@company.com", deserializedMailMessage.To[1].Address);
		//		Assert.AreEqual("third@company.com", deserializedMailMessage.To[2].Address);
		//		// ReSharper restore PossibleNullReferenceException
		//	}
		//}

		//[TestMethod]
		//[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "TEMPORARY")]
		//[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "TEST")]
		//public void System_Net_Mail_MailMessage_ShouldBeSerializable_TEMPORARY_TEST()
		//{
		//	string binarySerializedSerializableMailMessage;

		//	using(MailMessage mailMessage = new MailMessage())
		//	{
		//		mailMessage.Headers.Add("Header", "HeaderValue");

		//		Serializable<MailMessage> serializableMailMessage = new Serializable<MailMessage>(mailMessage);
		//		binarySerializedSerializableMailMessage = serializableMailMessage.SerializeBinary();

		//		Assert.AreEqual("Header", mailMessage.Headers.Keys[0]);
		//		Assert.AreEqual("HeaderValue", mailMessage.Headers[0]);
		//	}

		//	Serializable<MailMessage> deserializedSerializableMailMessage = (Serializable<MailMessage>) ObjectExtension.DeserializeBinary(binarySerializedSerializableMailMessage);

		//	using(MailMessage deserializedMailMessage = deserializedSerializableMailMessage.Instance)
		//	{
		//		Assert.AreEqual("Header", deserializedMailMessage.Headers.Keys[0]);
		//		Assert.AreEqual("HeaderValue", deserializedMailMessage.Headers[0]);
		//	}
		//}

		//[TestMethod]
		//public void System_Web_HttpContext_ShouldBeSerializable()
		//{
		//	Assert.Inconclusive("Implementation needed.");
		//}

		//[TestMethod]
		//public void System_Web_HttpPostedFile_ShouldBeSerializable()
		//{
		//	Assert.Inconclusive("Implementation needed.");
		//}

		//[TestMethod]
		//public void System_Web_HttpRequest_ShouldBeSerializable()
		//{
		//	const string filename = "Default.html";
		//	const string relativePath = "/" + filename;
		//	HttpRequest httpRequest = new HttpRequest(filename, "http://localhost" + relativePath, string.Empty);
		//	Assert.AreEqual(relativePath, httpRequest.FilePath);
		//	Assert.AreEqual(relativePath, httpRequest.RawUrl);
		//	Assert.AreEqual("http://localhost" + relativePath, httpRequest.Url.ToString());
		//	Serializable<HttpRequest> serializableHttpRequest = new Serializable<HttpRequest>(httpRequest);
		//	string binarySerializedSerializableHttpRequest = serializableHttpRequest.SerializeBinary();
		//	Serializable<HttpRequest> deserializedSerializableHttpRequest = (Serializable<HttpRequest>) ObjectExtension.DeserializeBinary(binarySerializedSerializableHttpRequest);
		//	HttpRequest deserializedHttpRequest = deserializedSerializableHttpRequest.Instance;
		//	Assert.AreEqual(httpRequest.FilePath, deserializedHttpRequest.FilePath);
		//	Assert.AreEqual(httpRequest.RawUrl, deserializedHttpRequest.RawUrl);
		//	Assert.AreEqual(httpRequest.Url.ToString(), deserializedHttpRequest.Url.ToString());
		//}

		#endregion
	}

	public class Unserializable
	{}
}