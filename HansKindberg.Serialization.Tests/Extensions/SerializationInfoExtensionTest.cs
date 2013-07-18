using System;
using System.Runtime.Serialization;
using HansKindberg.Serialization.Extensions;
using HansKindberg.Serialization.Tests.Extensions.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HansKindberg.Serialization.Tests.Extensions
{
	[TestClass]
	public class SerializationInfoExtensionTest
	{
		#region Methods

		[TestMethod]
		public void Exists_IfTheNameParameterValueDoesNotExist_ShouldReturnFalse()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			Assert.IsFalse(serializationInfo.Exists("Other"));
		}

		[TestMethod]
		public void Exists_IfTheNameParameterValueExists_ShouldReturnTrue()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			Assert.IsTrue(serializationInfo.Exists("Test"));
		}

		[TestMethod]
		public void Exists_IfTheNameParameterValueIsNull_ShouldReturnFalse()
		{
			Assert.IsFalse(new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>()).Exists(null));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Exists_IfTheSerializationInfoParameterValueIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				((SerializationInfo) null).Exists(string.Empty);
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName == "serializationInfo")
					throw;
			}
		}

		[TestMethod]
		public void Exists_ShouldBeCaseSensitiveRegardingTheNameParameter()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			Assert.IsTrue(serializationInfo.Exists("Test"));
			Assert.IsFalse(serializationInfo.Exists("test"));
		}

		[TestMethod]
		public void Prerequisite_SerializationInfo_GetValue_ShouldBeCaseSensitiveRegardingTheNameParameter()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			Assert.IsNotNull(serializationInfo.GetValue("Test", typeof(object)));

			Exception exception = null;

			try
			{
				serializationInfo.GetValue("test", typeof(object));
			}
			catch(SerializationException serializationException)
			{
				exception = serializationException;
			}

			if(exception == null)
				Assert.Fail("The GetValue method should be case sensitive regarding the name parameter.");
		}

		[TestMethod]
		public void TryGetValue_GenericWithOneParameter_IfTheNameParameterValueDoesNotExist_ShouldReturnTheDefaultOfT()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			Assert.IsNull(serializationInfo.TryGetValue<object>("Other"));
		}

		[TestMethod]
		public void TryGetValue_GenericWithOneParameter_IfTheNameParameterValueExistButIsNotOfTypeT_ShouldReturnTheDefaultOfT()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			Assert.AreEqual(0, serializationInfo.TryGetValue<int>("Test"));
		}

		[TestMethod]
		public void TryGetValue_GenericWithOneParameter_IfTheNameParameterValueExists_ShouldReturnTheValue()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			Assert.IsNotNull(serializationInfo.TryGetValue<object>("Test"));
		}

		[TestMethod]
		public void TryGetValue_GenericWithOneParameter_IfTheNameParameterValueIsNull_ShouldReturnTheDefaultOfT()
		{
			Assert.IsNull(new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>()).TryGetValue<object>(null));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TryGetValue_GenericWithOneParameter_IfTheSerializationInfoParameterValueIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				((SerializationInfo) null).TryGetValue<object>(string.Empty);
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName == "serializationInfo")
					throw;
			}
		}

		[TestMethod]
		public void TryGetValue_GenericWithOneParameter_ShouldBeCaseSensitiveRegardingTheNameParameter()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			Assert.IsNotNull(serializationInfo.TryGetValue<object>("Test"));
			Assert.IsNull(serializationInfo.TryGetValue<object>("test"));
		}

		[TestMethod]
		public void TryGetValue_GenericWithTwoParameters_IfTheNameParameterValueDoesNotExist_ShouldReturnTheDefaultValue()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			Assert.IsNull(serializationInfo.TryGetValue<object>("Other", null));
			Assert.IsNotNull(serializationInfo.TryGetValue("Other", new object()));

			serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new SerializationInformationValueMock());
			Assert.IsNull(serializationInfo.TryGetValue<SerializationInformationValueMock>("Other", null));
			Assert.IsNotNull(serializationInfo.TryGetValue("Other", new SerializationInformationValueMock()));
		}

		[TestMethod]
		public void TryGetValue_GenericWithTwoParameters_IfTheNameParameterValueExistButIsNotOfTypeT_ShouldReturnTheDefaultValue()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			Assert.AreEqual(10, serializationInfo.TryGetValue("Test", 10));

			serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new SerializationInformationValueMock());
			Assert.AreEqual(10, serializationInfo.TryGetValue("Test", 10));
		}

		[TestMethod]
		public void TryGetValue_GenericWithTwoParameters_IfTheNameParameterValueExists_ShouldReturnTheValue()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			Assert.IsNotNull(serializationInfo.TryGetValue<object>("Test", null));

			serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new SerializationInformationValueMock());
			Assert.IsNotNull(serializationInfo.TryGetValue<SerializationInformationValueMock>("Test", null));
		}

		[TestMethod]
		public void TryGetValue_GenericWithTwoParameters_IfTheNameParameterValueIsNull_ShouldReturnTheDefaultValue()
		{
			SerializationInformationValueMock value = new SerializationInformationValueMock();
			Assert.AreEqual(value, new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>()).TryGetValue(null, value));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TryGetValue_GenericWithTwoParameters_IfTheSerializationInfoParameterValueIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				((SerializationInfo) null).TryGetValue<object>(string.Empty, null);
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName == "serializationInfo")
					throw;
			}
		}

		[TestMethod]
		public void TryGetValue_GenericWithTwoParameters_ShouldBeCaseSensitiveRegardingTheNameParameter()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			Assert.IsNotNull(serializationInfo.TryGetValue<object>("Test", null));
			Assert.IsNull(serializationInfo.TryGetValue<object>("test", null));

			serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new SerializationInformationValueMock());
			Assert.IsNotNull(serializationInfo.TryGetValue<SerializationInformationValueMock>("Test", null));
			Assert.IsNull(serializationInfo.TryGetValue<SerializationInformationValueMock>("test", null));
		}

		[TestMethod]
		public void TryGetValue_WithBooleanReturnValue_IfTheNameParameterValueDoesNotExist_ShouldReturnFalseAndTheOutParameterShouldBetheDefaultOfT()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			object value;
			Assert.IsFalse(serializationInfo.TryGetValue("Other", out value));
			Assert.IsNull(value);
		}

		[TestMethod]
		public void TryGetValue_WithBooleanReturnValue_IfTheNameParameterValueExistButIsNotOfTypeT_ShouldReturnFalseAndTheOutParameterShouldBetheDefaultOfT()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			int value;
			Assert.IsFalse(serializationInfo.TryGetValue("Test", out value));
			Assert.AreEqual(0, value);
		}

		[TestMethod]
		public void TryGetValue_WithBooleanReturnValue_IfTheNameParameterValueExists_ShouldReturnTrueAndTheOutParameterShouldBeSetToTheValue()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			object value;
			Assert.IsTrue(serializationInfo.TryGetValue("Test", out value));
			Assert.IsNotNull(value);
		}

		[TestMethod]
		public void TryGetValue_WithBooleanReturnValue_IfTheNameParameterValueIsNull_ShouldReturnFalseAndTheOutParameterShouldBetheDefaultOfT()
		{
			object value;
			Assert.IsFalse(new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>()).TryGetValue(null, out value));
			Assert.IsNull(value);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TryGetValue_WithBooleanReturnValue_IfTheSerializationInfoParameterValueIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				object value;
				((SerializationInfo) null).TryGetValue(string.Empty, out value);
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName == "serializationInfo")
					throw;
			}
		}

		[TestMethod]
		public void TryGetValue_WithBooleanReturnValue_ShouldBeCaseSensitiveRegardingTheNameParameter()
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(object), Mock.Of<IFormatterConverter>());
			serializationInfo.AddValue("Test", new object());
			object value;
			Assert.IsTrue(serializationInfo.TryGetValue("Test", out value));
			Assert.IsNotNull(value);
			Assert.IsFalse(serializationInfo.TryGetValue("test", out value));
			Assert.IsNull(value);
		}

		#endregion
	}
}