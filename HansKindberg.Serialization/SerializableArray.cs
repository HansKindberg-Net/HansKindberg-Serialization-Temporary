using System;

namespace HansKindberg.Serialization
{
	[Serializable]
	public class SerializableArray : Serializable
	{
		#region Constructors

		public SerializableArray(Array array) : base(array, SerializableResolverLocator.Instance.SerializableResolver) {}
		protected internal SerializableArray(Array array, ISerializableResolver serializableResolver) : base(array, serializableResolver) {}

		#endregion

		#region Properties

		public virtual Array Array
		{
			get { return (Array) this.InstanceInternal; }
		}

		#endregion

		#region Methods

		protected internal override object CreateDeserializedInstance()
		{
			throw new NotImplementedException();
		}

		protected internal override object CreateSerializableInstance()
		{
			if(this.IsSerializable(this.Array))
				return this.Array;

			object[] serializableArray = new object[this.Array.Length];

			for(int i = 0; i < this.Array.Length; i++)
			{
				object item = this.Array.GetValue(i);

				if(this.IsSerializable(item))
					serializableArray[i] = item;
				else
					serializableArray[i] = new Serializable<object>(item, this.SerializableResolver);
			}

			return serializableArray;
		}

		#endregion
	}
}