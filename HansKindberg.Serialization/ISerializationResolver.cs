using System;
using System.Collections.Generic;
using System.Reflection;

namespace HansKindberg.Serialization
{
	public interface ISerializationResolver
	{
		#region Methods

		object CreateUninitializedObject(Type type);
		IEnumerable<FieldInfo> GetFieldsToSerialize(Type type);
		bool IsSerializable(Type type);

		#endregion
	}
}