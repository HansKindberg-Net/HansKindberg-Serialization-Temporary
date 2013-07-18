using System;

namespace HansKindberg.Serialization
{
	public interface ITypeValidator
	{
		#region Methods

		void ValidateThatTheTypeIsADelegate(Type type);

		#endregion
	}
}