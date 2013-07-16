using System;
using System.Runtime.Serialization;
using Castle.DynamicProxy;
using HansKindberg.Serialization.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Serialization.Tests
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
		public void CreateUninitializedObject_WithGenericParameter_IfTheGenericParameterIsAnAbstractType_ShouldReturnAnObjectOfThatType()
		{
			AbstractMock abstractMock = CreateDefaultSerializableFactory().CreateUninitializedObject<AbstractMock>();
			Assert.IsNotNull(abstractMock);

			AbstractWithoutParameterlessConstructorMock abstractWithoutParameterlessConstructorMock = CreateDefaultSerializableFactory().CreateUninitializedObject<AbstractWithoutParameterlessConstructorMock>();
			Assert.IsNotNull(abstractWithoutParameterlessConstructorMock);
		}

		[TestMethod]
		public void CreateUninitializedObject_WithGenericParameter_IfTheGenericParameterIsAnInterface_ShouldReturnAnObjectOfThatType()
		{
			IMock mock = CreateDefaultSerializableFactory().CreateUninitializedObject<IMock>();
			Assert.IsNotNull(mock);
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void CreateUninitializedObject_WithGenericParameter_IfTheGenericParameterIsOfATypeThatInheritsFromContextBoundObject_ShouldThrowANotSupportedException()
		{
			CreateDefaultSerializableFactory().CreateUninitializedObject<ContextBoundObjectMock>();
		}

		[TestMethod]
		public void CreateUninitializedObject_WithGenericParameter_IfTheGenericParameterIsOfATypeThatInheritsFromMarshalByRefObject_ShouldReturnAnObjectOfThatType()
		{
			MarshalByRefObjectMock marshalByRefObjectMock = CreateDefaultSerializableFactory().CreateUninitializedObject<MarshalByRefObjectMock>();
			Assert.IsNotNull(marshalByRefObjectMock);
		}

		[TestMethod]
		public void CreateUninitializedObject_WithGenericParameter_IfTheGenericParameterIsOfTypeString_ShouldReturnAnEmptyString()
		{
			Assert.AreEqual(string.Empty, CreateDefaultSerializableFactory().CreateUninitializedObject<string>());
		}

		[TestMethod]
		public void CreateUninitializedObject_WithTypeParameter_IfTheTypeParameterValueIsAnAbstractType_ShouldReturnAnObjectOfThatType()
		{
			AbstractMock abstractMock = (AbstractMock) CreateDefaultSerializableFactory().CreateUninitializedObject(typeof(AbstractMock));
			Assert.IsNotNull(abstractMock);

			AbstractWithoutParameterlessConstructorMock abstractWithoutParameterlessConstructorMock = (AbstractWithoutParameterlessConstructorMock) CreateDefaultSerializableFactory().CreateUninitializedObject(typeof(AbstractWithoutParameterlessConstructorMock));
			Assert.IsNotNull(abstractWithoutParameterlessConstructorMock);
		}

		[TestMethod]
		public void CreateUninitializedObject_WithTypeParameter_IfTheTypeParameterValueIsAnInterface_ShouldReturnAnObjectOfThatType()
		{
			IMock mock = (IMock) CreateDefaultSerializableFactory().CreateUninitializedObject(typeof(IMock));
			Assert.IsNotNull(mock);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateUninitializedObject_WithTypeParameter_IfTheTypeParameterValueIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				CreateDefaultSerializableFactory().CreateUninitializedObject(null);
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName == "type")
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void CreateUninitializedObject_WithTypeParameter_IfTheTypeParameterValueIsOfATypeThatInheritsFromContextBoundObject_ShouldThrowANotSupportedException()
		{
			CreateDefaultSerializableFactory().CreateUninitializedObject(typeof(ContextBoundObjectMock));
		}

		[TestMethod]
		public void CreateUninitializedObject_WithTypeParameter_IfTheTypeParameterValueIsOfATypeThatInheritsFromMarshalByRefObject_ShouldReturnAnObjectOfThatType()
		{
			MarshalByRefObjectMock marshalByRefObjectMock = (MarshalByRefObjectMock) CreateDefaultSerializableFactory().CreateUninitializedObject(typeof(MarshalByRefObjectMock));
			Assert.IsNotNull(marshalByRefObjectMock);
		}

		[TestMethod]
		public void CreateUninitializedObject_WithTypeParameter_IfTheTypeParameterValueIsOfTypeString_ShouldReturnAnEmptyString()
		{
			Assert.AreEqual(string.Empty, CreateDefaultSerializableFactory().CreateUninitializedObject(typeof(string)));
		}

		[TestMethod]
		[ExpectedException(typeof(MemberAccessException))]
		public void PrerequisiteTest_FormatterServices_GetUninitializedObject_IfTheTypeParameterValueIsAnAbstractType_ShouldThrowAMemberAccessException()
		{
			FormatterServices.GetUninitializedObject(typeof(AbstractMock));
		}

		[TestMethod]
		[ExpectedException(typeof(MemberAccessException))]
		public void PrerequisiteTest_FormatterServices_GetUninitializedObject_IfTheTypeParameterValueIsAnInterface_ShouldThrowAMemberAccessException()
		{
			FormatterServices.GetUninitializedObject(typeof(IMock));
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void PrerequisiteTest_FormatterServices_GetUninitializedObject_IfTheTypeParameterValueIsOfATypeThatInheritsFromContextBoundObject_ShouldThrowANotSupportedException()
		{
			FormatterServices.GetUninitializedObject(typeof(ContextBoundObjectMock));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void PrerequisiteTest_FormatterServices_GetUninitializedObject_IfTheTypeParameterValueIsOfTypeString_ShouldThrowAnArgumentException()
		{
			FormatterServices.GetUninitializedObject(typeof(string));
		}

		#endregion
	}
}