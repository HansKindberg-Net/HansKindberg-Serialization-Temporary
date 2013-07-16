using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HansKindberg.Serialization.Tests
{
	[TestClass]
	public class SerializableFactoryTest
	{
		#region Methods

		[TestMethod]
		public void Instance_IfInstanceIsFirstSetAndThenSetToNull_ShouldReturnADefaultSerializableFactoryWithADefaultProxyBuilder()
		{
			ISerializableFactory serializableFactory = Mock.Of<ISerializableFactory>();
			SerializableFactory.Instance = serializableFactory;
			Assert.AreEqual(serializableFactory, SerializableFactory.Instance);
			SerializableFactory.Instance = null;
			DefaultSerializableFactory defaultSerializableFactory = (DefaultSerializableFactory) SerializableFactory.Instance;
			Assert.IsNotNull(defaultSerializableFactory);
			Assert.IsTrue(defaultSerializableFactory.ProxyBuilder is DefaultProxyBuilder);
		}

		[TestMethod]
		public void Instance_IfInstanceIsSet_ShouldReturnTheSetInstance()
		{
			ISerializableFactory serializableFactory = Mock.Of<ISerializableFactory>();
			SerializableFactory.Instance = serializableFactory;
			Assert.AreEqual(serializableFactory, SerializableFactory.Instance);
		}

		[TestMethod]
		public void Instance_ShouldReturn_ShouldReturnADefaultSerializableFactoryWithADefaultProxyBuilder()
		{
			DefaultSerializableFactory defaultSerializableFactory = (DefaultSerializableFactory) SerializableFactory.Instance;
			Assert.IsNotNull(defaultSerializableFactory);
			Assert.IsTrue(defaultSerializableFactory.ProxyBuilder is DefaultProxyBuilder);
		}

		#endregion
	}
}