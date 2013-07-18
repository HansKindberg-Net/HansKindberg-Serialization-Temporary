using System;
using System.Runtime.Serialization;

namespace HansKindberg.Serialization.Extensions
{
	public static class SerializationInfoExtension
	{
		#region Methods

		public static bool Exists(this SerializationInfo serializationInfo, string name)
		{
			if(serializationInfo == null)
				throw new ArgumentNullException("serializationInfo");

			if(name == null)
				return false;

			foreach(SerializationEntry entry in serializationInfo)
			{
				if(entry.Name.Equals(name))
					return true;
			}

			return false;
		}

		public static T TryGetValue<T>(this SerializationInfo serializationInfo, string name)
		{
			return serializationInfo.TryGetValue(name, default(T));
		}

		public static T TryGetValue<T>(this SerializationInfo serializationInfo, string name, T defaultValue)
		{
			if(serializationInfo == null)
				throw new ArgumentNullException("serializationInfo");

			T value;
			if(!serializationInfo.TryGetValue(name, out value))
				value = defaultValue;

			return value;
		}

		public static bool TryGetValue<T>(this SerializationInfo serializationInfo, string name, out T value)
		{
			if(serializationInfo == null)
				throw new ArgumentNullException("serializationInfo");

			value = default(T);

			if(!serializationInfo.Exists(name))
				return false;

			object untypedValue = serializationInfo.GetValue(name, typeof(object));

			if(!(untypedValue is T))
				return false;

			value = (T) untypedValue;

			return true;
		}

		#endregion
	}
}