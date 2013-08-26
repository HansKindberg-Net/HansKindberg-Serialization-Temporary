using System;
using System.Collections.Generic;

namespace HansKindberg.Serialization
{
	[Serializable]
	public abstract class GenericSerializable<T> : Serializable
	{
		#region Constructors

		protected GenericSerializable(T instance, ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker, bool investigateSerializability, IList<SerializationResult> investigationResult) : base(instance, serializationResolver, circularReferenceTracker, investigateSerializability, investigationResult) {}

		#endregion

		#region Properties

		public new virtual T Instance
		{
			get { return (T) base.Instance; }
			protected internal set { base.Instance = value; }
		}

		public override bool InstanceIsSerializable
		{
			get
			{
				if(!this.InstanceIsSerializableInternal.HasValue)
					this.InstanceIsSerializableInternal = this.IsSerializable(this.Instance);

				return this.InstanceIsSerializableInternal.Value;
			}
		}

		#endregion
	}
}