using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// This class is mainly for internal use and is not intended to be used in your code. Use <see cref="Serializable&lt;T&gt;" /> instead.
	/// </summary>
	[Serializable]
	public class SerializableArray : GenericSerializable<Array>
	{
		protected SerializableArray(Array array) : base(array) {}


		protected internal override object CreateSerializableInstance()
		{
			object[] serializableArray = new object[this.Instance.Length];

			for (int i = 0; i < this.Instance.Length; i++)
			{
				object item = this.Instance.GetValue(i);

				if(this.IsSerializable(item))
				{
					serializableArray[i] = item;
				}
				else
				{
					Serializable serializable = this.CreateSerializable(item);
					serializable.PrepareForSerialization();
					serializableArray[i] = serializable;
				}
			}

			return serializableArray;
		}

		protected internal override object CreateDeserializedInstance(ISerializationResolver serializationResolver)
		{
			return null;

			//Array serializableArray = (Array)this.SerializableInstance;

			//Array array = (Array)Activator.CreateInstance(this.InstanceType, new object[] { serializableArray.Length });

			//for (int i = 0; i < array.Length; i++)
			//{
			//	object item = serializableArray.GetValue(i);

			//	if (item == null)
			//		continue;

			//	Serializable itemAsSerializable = item as Serializable;

			//	array.SetValue(itemAsSerializable != null ? itemAsSerializable.Instance : item, i);
			//}

			//return array;
		}
	}
}
