using System;
using System.Collections.Generic;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// This class is mainly for internal use and is not intended to be used in your code. Use <see cref="Serializable&lt;T&gt;" /> instead.
	/// </summary>
	[Serializable]
	public class SerializableInternal : GenericSerializable<object>
	{
		#region Constructors

		protected internal SerializableInternal(object instance, ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker, bool investigateSerializability, IList<SerializationResult> investigationResult) : base(instance, serializationResolver, circularReferenceTracker, investigateSerializability, investigationResult) {}

		#endregion
	}
}