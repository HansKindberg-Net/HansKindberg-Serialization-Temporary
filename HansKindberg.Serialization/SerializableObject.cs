//using System;
//using System.Collections.Generic;

//namespace HansKindberg.Serialization
//{
//	/// <summary>
//	/// This class is mainly for internal use and is not intended to be used in your code. Use <see cref="Serializable&lt;T&gt;" /> instead.
//	/// </summary>
//	[Serializable]
//	public class SerializableObject : GenericSerializable<object>
//	{
//		#region Constructors

//		protected internal SerializableObject(object instance, ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker, bool investigateSerializability, IList<SerializationResult> investigationResult) : base(instance, serializationResolver, circularReferenceTracker, investigateSerializability, investigationResult) {}

//		#endregion

//		#region Methods

//		protected internal override object CreateDeserializedInstance(ISerializationResolver serializationResolver)
//		{
//			if (serializationResolver == null)
//				throw new ArgumentNullException("serializationResolver");

//			object instance = serializationResolver.CreateUninitializedObject(this.InstanceType);

//			foreach (SerializableField serializableField in (IEnumerable<SerializableField>)this.SerializableInstance)
//			{
//				serializableField.FieldInformation.SetValue(instance, serializableField.Instance);
//			}

//			return instance;
//		}

//		protected internal override object CreateSerializableInstance()
//		{
//			if(this.InstanceIsSerializable)
//				return this.Instance;

//			var serializableFields = new List<SerializableField>();

//			if(this.Instance != null)
//			{
//				// ReSharper disable LoopCanBeConvertedToQuery
//				foreach(var fieldInformation in this.SerializationResolver.GetFieldsForSerialization(this.Instance.GetType()))
//				{
//					object fieldValue = fieldInformation.GetValue(this.Instance);

//					if(fieldValue == null)
//						continue;

//					serializableFields.Add(new SerializableField(fieldInformation, fieldValue, this.SerializationResolver, this.CircularReferenceTracker, this.InvestigateSerializabilityInternal, this.InvestigationResultInternal));
//				}
//				// ReSharper restore LoopCanBeConvertedToQuery
//			}

//			return serializableFields.ToArray();
//		}

//		#endregion
//	}
//}