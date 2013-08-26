//using System;
//using System.Collections.Generic;

//namespace HansKindberg.Serialization
//{
//	/// <summary>
//	/// Used to serialize arrays. This class is mainly for internal use and is not intended to be used in your code. Use <see cref="Serializable&lt;T&gt;" /> instead.
//	/// </summary>
//	[Serializable]
//	public class SerializableArray : GenericSerializable<Array>
//	{
//		#region Constructors

//		protected internal SerializableArray(Array array, ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker, bool investigateSerializability, IList<SerializationResult> investigationResult) : base(array, serializationResolver, circularReferenceTracker, investigateSerializability, investigationResult) {}

//		#endregion

//		#region Methods

//		protected internal override object CreateDeserializedInstance(ISerializationResolver serializationResolver)
//		{
//			Array serializableArray = (Array)this.SerializableInstance;

//			Array array = (Array)Activator.CreateInstance(this.InstanceType, new object[] { serializableArray.Length });

//			for (int i = 0; i < array.Length; i++)
//			{
//				object item = serializableArray.GetValue(i);

//				if (item == null)
//					continue;

//				Serializable itemAsSerializable = item as Serializable;

//				array.SetValue(itemAsSerializable != null ? itemAsSerializable.Instance : item, i);
//			}

//			return array;
//		}

//		protected internal override object CreateSerializableInstance()
//		{
//			if(this.InstanceIsSerializable)
//				return this.Instance;

//			object[] serializableArray = new object[this.Instance.Length];

//			for(int i = 0; i < this.Instance.Length; i++)
//			{
//				object item = this.Instance.GetValue(i);

//				if(this.IsSerializable(item))
//					serializableArray[i] = item;
//				else
//					serializableArray[i] = this.CreateSerializable(item);
//			}

//			return serializableArray;
//		}

//		#endregion
//	}
//}