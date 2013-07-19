using System;
using System.Runtime.Serialization;
using Castle.DynamicProxy;
using HansKindberg.Serialization.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Serialization.Tests
{
	[TestClass]
	public class DefaultSerializableResolverTest
	{
		#region Methods

		[TestMethod]
		public void Constructor_ShouldSetTheProxyBuilderProperty()
		{
			IProxyBuilder proxyBuilder = new DefaultProxyBuilder();
			Assert.AreEqual(proxyBuilder, new DefaultSerializableResolver(proxyBuilder).ProxyBuilder);
		}

		private static DefaultSerializableResolver CreateDefaultSerializableResolver()
		{
			return new DefaultSerializableResolver(new DefaultProxyBuilder());
		}

		[TestMethod]
		public void CreateUninitializedObject_WithGenericParameter_IfTheGenericParameterIsAnAbstractType_ShouldReturnAnObjectOfThatType()
		{
			AbstractMock abstractMock = CreateDefaultSerializableResolver().CreateUninitializedObject<AbstractMock>();
			Assert.IsNotNull(abstractMock);

			AbstractWithoutParameterlessConstructorMock abstractWithoutParameterlessConstructorMock = CreateDefaultSerializableResolver().CreateUninitializedObject<AbstractWithoutParameterlessConstructorMock>();
			Assert.IsNotNull(abstractWithoutParameterlessConstructorMock);
		}

		[TestMethod]
		public void CreateUninitializedObject_WithGenericParameter_IfTheGenericParameterIsAnInterface_ShouldReturnAnObjectOfThatType()
		{
			IMock mock = CreateDefaultSerializableResolver().CreateUninitializedObject<IMock>();
			Assert.IsNotNull(mock);
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void CreateUninitializedObject_WithGenericParameter_IfTheGenericParameterIsOfATypeThatInheritsFromContextBoundObject_ShouldThrowANotSupportedException()
		{
			CreateDefaultSerializableResolver().CreateUninitializedObject<ContextBoundObjectMock>();
		}

		[TestMethod]
		public void CreateUninitializedObject_WithGenericParameter_IfTheGenericParameterIsOfATypeThatInheritsFromMarshalByRefObject_ShouldReturnAnObjectOfThatType()
		{
			MarshalByRefObjectMock marshalByRefObjectMock = CreateDefaultSerializableResolver().CreateUninitializedObject<MarshalByRefObjectMock>();
			Assert.IsNotNull(marshalByRefObjectMock);
		}

		[TestMethod]
		public void CreateUninitializedObject_WithGenericParameter_IfTheGenericParameterIsOfTypeString_ShouldReturnAnEmptyString()
		{
			Assert.AreEqual(string.Empty, CreateDefaultSerializableResolver().CreateUninitializedObject<string>());
		}

		[TestMethod]
		public void CreateUninitializedObject_WithTypeParameter_IfTheTypeParameterValueIsAnAbstractType_ShouldReturnAnObjectOfThatType()
		{
			AbstractMock abstractMock = (AbstractMock) CreateDefaultSerializableResolver().CreateUninitializedObject(typeof(AbstractMock));
			Assert.IsNotNull(abstractMock);

			AbstractWithoutParameterlessConstructorMock abstractWithoutParameterlessConstructorMock = (AbstractWithoutParameterlessConstructorMock) CreateDefaultSerializableResolver().CreateUninitializedObject(typeof(AbstractWithoutParameterlessConstructorMock));
			Assert.IsNotNull(abstractWithoutParameterlessConstructorMock);
		}

		[TestMethod]
		public void CreateUninitializedObject_WithTypeParameter_IfTheTypeParameterValueIsAnInterface_ShouldReturnAnObjectOfThatType()
		{
			IMock mock = (IMock) CreateDefaultSerializableResolver().CreateUninitializedObject(typeof(IMock));
			Assert.IsNotNull(mock);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateUninitializedObject_WithTypeParameter_IfTheTypeParameterValueIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				CreateDefaultSerializableResolver().CreateUninitializedObject(null);
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
			CreateDefaultSerializableResolver().CreateUninitializedObject(typeof(ContextBoundObjectMock));
		}

		[TestMethod]
		public void CreateUninitializedObject_WithTypeParameter_IfTheTypeParameterValueIsOfATypeThatInheritsFromMarshalByRefObject_ShouldReturnAnObjectOfThatType()
		{
			MarshalByRefObjectMock marshalByRefObjectMock = (MarshalByRefObjectMock) CreateDefaultSerializableResolver().CreateUninitializedObject(typeof(MarshalByRefObjectMock));
			Assert.IsNotNull(marshalByRefObjectMock);
		}

		[TestMethod]
		public void CreateUninitializedObject_WithTypeParameter_IfTheTypeParameterValueIsOfTypeString_ShouldReturnAnEmptyString()
		{
			Assert.AreEqual(string.Empty, CreateDefaultSerializableResolver().CreateUninitializedObject(typeof(string)));
		}

		[TestMethod]
		public void MoreTestsNeeded()
		{
			Assert.Inconclusive("More tests needed for {0}.", typeof(DefaultSerializableResolver));
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