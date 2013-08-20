using System;

namespace HansKindberg.Serialization.InversionOfControl
{
	public interface IServiceLocator : IServiceProvider
	{
		#region Methods

		T GetService<T>();

		#endregion
	}
}