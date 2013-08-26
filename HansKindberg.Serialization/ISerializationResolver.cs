using System;
using System.Collections.Generic;
using System.Reflection;

namespace HansKindberg.Serialization
{
	public interface ISerializationResolver
	{
		#region Methods

		object CreateUninitializedObject(Type type);
		IEnumerable<FieldInfo> GetFieldsForSerialization(Type type);
		bool IsSerializable(object instance);
		SerializationResult TrySerialize(object instance);

		#endregion
	}
}