using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Serialization.Tests
{
	[TestClass]
	public class SerializableDelegateTest
	{
		#region Methods

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Constructor_WithDelegateAndSerializableDelegateResolverParameters_IfTheDelegateParameterIsNotOfTypeDelegate_ShouldThrowAnArgumentException()
		{
			try
			{
				// ReSharper disable ObjectCreationAsStatement
				new SerializableDelegate<object>(new object(), null);
				// ReSharper restore ObjectCreationAsStatement
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.Message.StartsWith("The type \"System.Object\" must be a delegate (System.Delegate).", StringComparison.Ordinal) && argumentException.ParamName == "delegate")
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Constructor_WithDelegateParameter_IfTheDelegateParameterIsNotOfTypeDelegate_ShouldThrowAnArgumentException()
		{
			try
			{
				// ReSharper disable ObjectCreationAsStatement
				new SerializableDelegate<object>(new object());
				// ReSharper restore ObjectCreationAsStatement
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.Message.StartsWith("The type \"System.Object\" must be a delegate (System.Delegate).", StringComparison.Ordinal) && argumentException.ParamName == "delegate")
					throw;
			}
		}

		#endregion
	}
}