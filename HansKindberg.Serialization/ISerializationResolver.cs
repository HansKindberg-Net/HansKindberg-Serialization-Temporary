using System;
using System.Collections.Generic;
using System.Reflection;

namespace HansKindberg.Serialization
{
	public interface ISerializationResolver
	{
		#region Properties

		bool DecideIfAnInstanceIsSerializableByActuallySerializingIt { get; set; }
		IList<SerializationFailure> SerializationFailures { get; }

		#endregion

		#region Methods

		object CreateUninitializedObject(Type type);
		IEnumerable<FieldInfo> GetFieldsForSerialization(Type type);
		bool IsSerializable(object instance);

		#endregion
	}
}