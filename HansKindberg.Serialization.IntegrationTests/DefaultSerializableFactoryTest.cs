using System.Web;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Serialization.IntegrationTests
{
	[TestClass]
	public class DefaultSerializableFactoryTest
	{
		#region Methods

		private static DefaultSerializableFactory CreateDefaultSerializableFactory()
		{
			return new DefaultSerializableFactory(new DefaultProxyBuilder());
		}

		[TestMethod]
		public void CreateUninitializedObject_IfTheGenericParameterIsOfTypeHttpContext_ShouldReturnAnObjectOfTypeHttpContext()
		{
			HttpContext httpContext = CreateDefaultSerializableFactory().CreateUninitializedObject<HttpContext>();
			Assert.IsNotNull(httpContext);
		}

		#endregion
	}
}