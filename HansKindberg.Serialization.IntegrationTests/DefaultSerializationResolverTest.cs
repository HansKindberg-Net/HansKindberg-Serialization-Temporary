//using System.Web;
//using Castle.DynamicProxy;
//using HansKindberg.Serialization.IntegrationTests.Helpers.Extensions;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace HansKindberg.Serialization.IntegrationTests
//{
//	[TestClass]
//	public class DefaultSerializationResolverTest
//	{
//		#region Methods

//		private static DefaultSerializableResolver CreateDefaultSerializableResolver()
//		{
//			return new DefaultSerializableResolver(new DefaultProxyBuilder());
//		}

//		[TestMethod]
//		public void CreateUninitializedObject_IfTheGenericParameterIsOfTypeHttpContext_ShouldReturnAnObjectOfTypeHttpContext()
//		{
//			HttpContext httpContext = CreateDefaultSerializableResolver().CreateUninitializedObject<HttpContext>();
//			Assert.IsNotNull(httpContext);
//		}

//		[TestMethod]
//		public void CreateUninitializedObject_IfTheGenericParameterIsOfTypeHttpRequest_ShouldReturnAnObjectOfTypeHttpRequest()
//		{
//			HttpRequest httpRequest = CreateDefaultSerializableResolver().CreateUninitializedObject<HttpRequest>();
//			Assert.IsNotNull(httpRequest);
//		}

//		[TestMethod]
//		public void CreateUninitializedObject_IfTheGenericParameterIsOfTypeHttpResponse_ShouldReturnAnObjectOfTypeHttpResponse()
//		{
//			HttpResponse httpResponse = CreateDefaultSerializableResolver().CreateUninitializedObject<HttpResponse>();
//			Assert.IsNotNull(httpResponse);
//		}

//		[TestMethod]
//		public void ShouldBeSerializable()
//		{
//			DefaultSerializableResolver defaultSerializableResolver = CreateDefaultSerializableResolver();
//			string binarySerializedDefaultSerializableResolver = defaultSerializableResolver.SerializeBinary();
//			DefaultSerializableResolver deserializedDefaultSerializableResolver = (DefaultSerializableResolver) ObjectExtension.DeserializeBinary(binarySerializedDefaultSerializableResolver);
//			Assert.IsNotNull(deserializedDefaultSerializableResolver);
//			Assert.AreNotEqual(defaultSerializableResolver, deserializedDefaultSerializableResolver);
//		}

//		#endregion
//	}
//}