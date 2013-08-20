using System.Web;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Serialization.IntegrationTests
{
	[TestClass]
	public class DefaultSerializationResolverTest
	{
		#region Methods

		private static DefaultSerializationResolver CreateDefaultSerializationResolver()
		{
			return new DefaultSerializationResolver(new DefaultProxyBuilder());
		}

		[TestMethod]
		public void CreateUninitializedObject_IfTheGenericParameterIsOfType_System_Web_HttpContext_ShouldReturnAnObjectOfType_System_Web_HttpContext()
		{
			HttpContext httpContext = CreateDefaultSerializationResolver().CreateUninitializedObject(typeof(HttpContext)) as HttpContext;
			Assert.IsNotNull(httpContext);
		}

		[TestMethod]
		public void CreateUninitializedObject_IfTheGenericParameterIsOfType_System_Web_HttpRequest_ShouldReturnAnObjectOfType_System_Web_HttpRequest()
		{
			HttpRequest httpRequest = CreateDefaultSerializationResolver().CreateUninitializedObject(typeof(HttpRequest)) as HttpRequest;
			Assert.IsNotNull(httpRequest);
		}

		[TestMethod]
		public void CreateUninitializedObject_IfTheGenericParameterIsOfType_System_Web_HttpResponse_ShouldReturnAnObjectOfType_System_Web_HttpResponse()
		{
			HttpResponse httpResponse = CreateDefaultSerializationResolver().CreateUninitializedObject(typeof(HttpResponse)) as HttpResponse;
			Assert.IsNotNull(httpResponse);
		}

		#endregion
	}
}