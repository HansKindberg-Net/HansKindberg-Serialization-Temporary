using System;

namespace HansKindberg.Serialization
{
	public interface ISerializableDelegateResolver : ISerializableResolver
	{
		#region Methods

		void ValidateDelegateType(Type delegateType);

		#endregion
	}
}