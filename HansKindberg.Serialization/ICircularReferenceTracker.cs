using System;
using System.Collections.Generic;

namespace HansKindberg.Serialization
{
	public interface ICircularReferenceTracker
	{
		#region Properties

		IEnumerable<Guid> References { get; }

		#endregion

		#region Methods

		void AddReference(Guid id);
		void Clear();
		object GetTrackedInstance(Guid id);
		Guid? GetTrackedInstanceId(object instance);
		void TrackInstanceIfNecessary(Serializable serializable);

		#endregion
	}
}