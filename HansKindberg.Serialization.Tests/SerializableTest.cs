using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using HansKindberg.Serialization.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HansKindberg.Serialization.Tests
{
	[TestClass]
	public class SerializableTest
	{
		#region Fields

		private const string _serializableResolverSerializationInformationName = "SerializableResolver";

		#endregion

		#region Methods

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		[SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "HansKindberg.Serialization.Tests.Mocks.SerializableMock`1<System.Object>")]
		public void Constructor_WithSerializationParameters_IfTheSerializationInfoParameterValueDoesNotContainASerializableResolver_ShouldThrowAnArgumentException()
		{
			try
			{
				// ReSharper disable ObjectCreationAsStatement
				new SerializableMock<object>(new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>()), new StreamingContext());
				// ReSharper restore ObjectCreationAsStatement
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.Message.StartsWith("The serializable resolver could not be retrieved from the serialization-information.", StringComparison.Ordinal) && argumentException.ParamName == "info")
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "HansKindberg.Serialization.Tests.Mocks.SerializableMock`1<System.Object>")]
		public void Constructor_WithSerializationParameters_IfTheSerializationInfoParameterValueIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				// ReSharper disable ObjectCreationAsStatement
				new SerializableMock<object>(null, new StreamingContext());
				// ReSharper restore ObjectCreationAsStatement
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName == "info")
					throw;
			}
		}

		[TestMethod]
		public void Constructor_WithSerializationParameters_ShouldCallGetInstanceOfTheSerializableResolver()
		{
			object instance = new object();
			SerializationInfo serializationInformation = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			StreamingContext streamingContext = new StreamingContext();
			string index = string.Empty;
			Mock<ISerializableResolver> serializableResolverMock = new Mock<ISerializableResolver>();
			serializableResolverMock.Setup(serializableResolver => serializableResolver.InstanceFromSerializationInformation<object>(serializationInformation, streamingContext, index)).Returns(instance);
			serializationInformation.AddValue(_serializableResolverSerializationInformationName, serializableResolverMock.Object);

			serializableResolverMock.Verify(serializableResolver => serializableResolver.InstanceFromSerializationInformation<object>(serializationInformation, streamingContext, index), Times.Never());

			Assert.AreEqual(instance, new SerializableMock<object>(serializationInformation, new StreamingContext()).Instance);

			serializableResolverMock.Verify(serializableResolver => serializableResolver.InstanceFromSerializationInformation<object>(serializationInformation, streamingContext, index), Times.Once());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "HansKindberg.Serialization.Serializable`1<System.Object>")]
		public void Constructor_WithTwoParameters_IfTheInstanceParameterValueIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				// ReSharper disable ObjectCreationAsStatement
				new Serializable<object>(null, Mock.Of<ISerializableResolver>());
				// ReSharper restore ObjectCreationAsStatement
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName == "instance")
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "HansKindberg.Serialization.Serializable`1<System.Object>")]
		public void Constructor_WithTwoParameters_IfTheSerializableResolverParameterValueIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				// ReSharper disable ObjectCreationAsStatement
				new Serializable<object>(new object(), null);
				// ReSharper restore ObjectCreationAsStatement
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName == "serializableResolver")
					throw;
			}
		}

		[TestMethod]
		public void Constructor_WithTwoParameters_ShouldSetTheInstanceProperty()
		{
			object instance = new object();
			Assert.AreEqual(instance, new Serializable<object>(instance, Mock.Of<ISerializableResolver>()).Instance);
		}

		[TestMethod]
		public void Constructor_WithTwoParameters_ShouldSetTheSerializableResolverProperty()
		{
			ISerializableResolver serializableResolver = Mock.Of<ISerializableResolver>();
			Assert.AreEqual(serializableResolver, new Serializable<object>(new object(), serializableResolver).SerializableResolver);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetObjectData_IfTheSerializationInfoParameterValueIsNull_ShouldThrowAnArgumentNullException()
		{
			Serializable<object> serializable = new Serializable<object>(new object(), Mock.Of<ISerializableResolver>());
			try
			{
				// ReSharper disable AssignNullToNotNullAttribute
				serializable.GetObjectData(null, new StreamingContext());
				// ReSharper restore AssignNullToNotNullAttribute
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName == "info")
					throw;
			}
		}

		[TestMethod]
		public void GetObjectData_ShouldCallSetInstanceOfTheSerializableResolver()
		{
			// ReSharper disable ImplicitlyCapturedClosure

			const string instanceSerializationInformationName = "Instance";
			object instance = new object();
			SerializationInfo serializationInformation = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			StreamingContext streamingContext = new StreamingContext();
			string index = string.Empty;
			Mock<ISerializableResolver> serializableResolverMock = new Mock<ISerializableResolver>();
			serializableResolverMock.Setup(serializableResolver => serializableResolver.InstanceToSerializationInformation(instance, serializationInformation, streamingContext, index)).Callback(() => serializationInformation.AddValue(instanceSerializationInformationName, instance));

			serializableResolverMock.Verify(serializableResolver => serializableResolver.InstanceToSerializationInformation(instance, serializationInformation, streamingContext, index), Times.Never());

			new Serializable<object>(instance, serializableResolverMock.Object).GetObjectData(serializationInformation, new StreamingContext());
			Assert.AreEqual(instance, serializationInformation.GetValue(instanceSerializationInformationName, typeof(object)));

			serializableResolverMock.Verify(serializableResolver => serializableResolver.InstanceToSerializationInformation(instance, serializationInformation, streamingContext, index), Times.Once());

			// ReSharper restore ImplicitlyCapturedClosure
		}

		[TestMethod]
		public void PrerequisiteTest_MockOfISerializableResolver_GetType_IsSerializable_ShouldReturnTrue()
		{
			Assert.IsTrue(Mock.Of<ISerializableResolver>().GetType().IsSerializable);
		}

		#endregion
	}
}