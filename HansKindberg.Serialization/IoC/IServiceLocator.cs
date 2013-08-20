using System;

namespace HansKindberg.Serialization.IoC
{
	public interface IServiceLocator : IServiceProvider
	{
		#region Methods

		T GetService<T>();

		#endregion
	}
}