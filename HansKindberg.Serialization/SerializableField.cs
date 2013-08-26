using System;
using System.Collections.Generic;
using System.Reflection;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// Used to serialize field values. This class is mainly for internal use and is not intended to be used in your code. Use <see cref="Serializable&lt;T&gt;" /> instead.
	/// </summary>
	[Serializable]
	public class SerializableField : GenericSerializable<object>
	{
		#region Fields

		private readonly FieldInfo _fieldInformation;

		#endregion

		#region Constructors

		protected internal SerializableField(FieldInfo fieldInformation, object instance, ISerializationResolver serializationResolver, ICircularReferenceTracker circularReferenceTracker, bool investigateSerializability, IList<SerializationResult> investigationResult) : base(instance, serializationResolver, circularReferenceTracker, investigateSerializability, investigationResult)
		{
			if(fieldInformation == null)
				throw new ArgumentNullException("fieldInformation");

			this._fieldInformation = fieldInformation;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Information about the field.
		/// </summary>
		public virtual FieldInfo FieldInformation
		{
			get { return this._fieldInformation; }
		}

		#endregion









		protected internal override object CreateSerializableInstance()
		{
			var serializableObject = new SerializableObject(this.Instance, this.SerializationResolver, this.CircularReferenceTracker, this.InvestigateSerializabilityInternal, this.InvestigationResultInternal);
			serializableObject.PrepareForSerialization();
			return serializableObject;
		}

		protected internal override object CreateDeserializedInstance(ISerializationResolver serializationResolver)
		{
			return null;

			//if (serializationResolver == null)
			//	throw new ArgumentNullException("serializationResolver");

			//object instance = serializationResolver.CreateUninitializedObject(this.InstanceType);

			//foreach (SerializableField serializableField in (IEnumerable<SerializableField>)this.SerializableInstance)
			//{
			//	serializableField.FieldInformation.SetValue(instance, serializableField.ins);
			//	//deserializedField.FieldInformation.SetValue(instance, deserializedField.Instance);
			//}

			//return instance;
		}









	}
}