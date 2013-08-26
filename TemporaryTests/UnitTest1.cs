using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;
using HansKindberg.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TemporaryTests.Helpers.Extensions;

namespace TemporaryTests
{
	[TestClass]
	public class UnitTest1
	{
		//[TestMethod]
		//public void Test()
		//{
		//	//Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
		//	//Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
		//	//Serializable<Context> serializableContext = new Serializable<Context>(new Context());
		//	//string binarySerializedSerializableContext = serializableContext.SerializeBinary();
		//	//Assert.IsNotNull(binarySerializedSerializableContext);
		//	//object[] array = new object[] { new Serializable<int>(10), 99 };
		//	//Serializable<object[]> serializableArray = new Serializable<object[]>(array);
		//	//string binaryDeserializedSerializableArray = serializableArray.SerializeBinary();
		//	//Assert.IsNotNull(binaryDeserializedSerializableArray);
		//}
		//[TestMethod]
		//public void _REMOVE_LATER_System_Web_HttpContext_ShouldBeSerializable()
		//{
		//	Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
		//	Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
		//	const string filename = "Default.html";
		//	const string relativePath = "/" + filename;
		//	List<string> binarySerializedSerializableHttpContextFields = new List<string>();
		//	using (StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture))
		//	{
		//		HttpContext httpContext = new HttpContext(new HttpRequest(filename, "http://localhost" + relativePath, string.Empty), new HttpResponse(stringWriter));
		//		Serializable<HttpContext> serializableHttpContext = new Serializable<HttpContext>(httpContext);
		//		MethodInfo createSerializableFieldsMethod = typeof(Serializable<HttpContext>).GetMethod("CreateSerializableFields", BindingFlags.Instance | BindingFlags.NonPublic);
		//		int i = 0;
		//		foreach (SerializableField serializableField in (IEnumerable<SerializableField>)createSerializableFieldsMethod.Invoke(serializableHttpContext, null))
		//		{
		//			i++;
		//			//if (i > 3)
		//			//	break;
		//			if(serializableField.FieldInformation.DeclaringType.FullName == "System.Web.HttpContext")
		//			{
		//				if(serializableField.FieldInformation.Name == "_request")
		//					continue;
		//				if (serializableField.FieldInformation.Name == "_response")
		//					continue;
		//				if (serializableField.FieldInformation.Name == "_CookielessHelper")
		//					continue;
		//			}
		//			try
		//			{
		//				binarySerializedSerializableHttpContextFields.Add(serializableField.SerializeBinary());
		//			}
		//			catch(Exception exception)
		//			{
		//				Assert.Fail("i == " + i.ToString() + ", field = " + serializableField.FieldInformation.DeclaringType.FullName + serializableField.FieldInformation.Name + ", exception = " + exception.Message);
		//				return;
		//			}
		//		}
		//	}
		//	ISerializationResolver serializationResolver = ServiceLocator.Instance.GetService<ISerializationResolver>();
		//	HttpContext deserializedHttpContext = (HttpContext)serializationResolver.CreateUninitializedObject(typeof(HttpContext));
		//	foreach (string binarySerializedSerializableHttpContextField in binarySerializedSerializableHttpContextFields)
		//	{
		//		SerializableField serializableField = (SerializableField)ObjectExtension.DeserializeBinary(binarySerializedSerializableHttpContextField);
		//		serializableField.FieldInformation.SetValue(deserializedHttpContext, serializableField.Instance);
		//	}
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
		//	Assert.IsNotNull(binarySerializedSerializableHttpRequest);
		//	Serializable<HttpRequest> deserializedSerializableHttpRequest = (Serializable<HttpRequest>)ObjectExtension.DeserializeBinary(binarySerializedSerializableHttpRequest);
		//	HttpRequest deserializedHttpRequest = deserializedSerializableHttpRequest.Instance;
		//	Assert.AreEqual(httpRequest.FilePath, deserializedHttpRequest.FilePath);
		//	Assert.AreEqual(httpRequest.RawUrl, deserializedHttpRequest.RawUrl);
		//	Assert.AreEqual(httpRequest.Url.ToString(), deserializedHttpRequest.Url.ToString());
		//}
		//[TestMethod]
		//public void Test()
		//{
		//	System.IO.Compression.GZipStream
		//		using(GZipStream stream = new GZipStream(())
		//	{
		//		new BinaryFormatter().Serialize(memoryStream, value);
		//		return Convert.ToBase64String(memoryStream.ToArray());
		//	}
		//	ISerializationResolver serializationResolver = ServiceLocator.Instance.GetService<ISerializationResolver>();
		//	Assert.AreEqual(68, serializationResolver.GetFieldsToSerialize(typeof(HttpRequest)).Count());
		//}

		#region Methods

		//[TestMethod]
		//public void System_Web_HttpContext_ShouldBeSerializable()
		//{
		//	Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
		//	Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");

		//	const string filename = "Default.html";
		//	const string relativePath = "/" + filename;
		//	string binarySerializedSerializableHttpContext;

		//	using(StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture))
		//	{
		//		HttpContext httpContext = new HttpContext(new HttpRequest(filename, "http://localhost" + relativePath, string.Empty), new HttpResponse(stringWriter));
		//		Serializable<HttpContext> serializableHttpContext = new Serializable<HttpContext>(httpContext);
		//		binarySerializedSerializableHttpContext = serializableHttpContext.SerializeBinary();
		//	}

		//	Serializable<HttpContext> deserializedSerializableHttpContext = (Serializable<HttpContext>) ObjectExtension.DeserializeBinary(binarySerializedSerializableHttpContext);
		//	HttpContext deserializedHttpContext = deserializedSerializableHttpContext.Instance;
		//	Assert.IsNotNull(deserializedHttpContext);
		//	Assert.AreEqual(relativePath, deserializedHttpContext.Request.FilePath);
		//}

		#endregion

		//[TestMethod]
		//public void Unserializable_ShouldNotBeSerializable()
		//{
		//	Unserializable[] unserializableArray = new[] {new Unserializable(), new Unserializable(), new Unserializable()};
		//	Serializable<Unserializable[]> serializableUnserializableArray = new Serializable<Unserializable[]>(unserializableArray);
		//	string binarySerializedUnserializableArray = serializableUnserializableArray.SerializeBinary();
		//	Serializable<Unserializable[]> deserializedSerializableUnserializableArray = (Serializable<Unserializable[]>) ObjectExtension.DeserializeBinary(binarySerializedUnserializableArray);
		//	Unserializable[] deserializedUnserializableArray = deserializedSerializableUnserializableArray.Instance;
		//	Assert.AreEqual(3, deserializedUnserializableArray.Length);
		//	Assert.IsNotNull(deserializedUnserializableArray[0]);
		//	Assert.IsNotNull(deserializedUnserializableArray[1]);
		//	Assert.IsNotNull(deserializedUnserializableArray[2]);
		//	//Assert.AreEqual(value, ObjectExtension.DeserializeBinary(binarySerializedObject));
		//}
	}

	public class Unserializable {}

	public class Context
	{
		#region Fields

		private readonly Request _request;

		#endregion

		#region Constructors

		public Context()
		{
			this._request = new Request(this);
		}

		#endregion

		#region Properties

		public virtual Request Request
		{
			get { return this._request; }
		}

		#endregion
	}

	public class Request
	{
		#region Fields

		private readonly Context _context;

		#endregion

		#region Constructors

		public Request(Context context)
		{
			this._context = context;
		}

		#endregion

		#region Properties

		public virtual Context Context
		{
			get { return this._context; }
		}

		#endregion
	}
}