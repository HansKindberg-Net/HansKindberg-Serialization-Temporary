using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// Makes delegates serializable where possible.
	/// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
	/// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public class SerializableDelegate<T> : Serializable<T>
	{
		#region Constructors

		public SerializableDelegate(T instance, ISerializableDelegateResolver serializableDelegateResolver) : base(instance, serializableDelegateResolver)
		{
			serializableDelegateResolver.ValidateDelegateType(typeof(T));
		}

		protected SerializableDelegate(SerializationInfo info, StreamingContext context) : base(info, context) {}

		#endregion
	}
}