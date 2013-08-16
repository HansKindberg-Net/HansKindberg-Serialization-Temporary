﻿using System;
using System.Reflection;

namespace HansKindberg.Serialization
{
	[Serializable]
	public class SerializableField : Serializable<object>
	{
		#region Fields

		private readonly FieldInfo _fieldInformation;

		#endregion

		#region Constructors

		public SerializableField(FieldInfo fieldInformation, object instance) : this(fieldInformation, instance, SerializableResolverLocator.Instance.SerializableResolver) {}

		protected internal SerializableField(FieldInfo fieldInformation, object instance, ISerializableResolver serializableResolver) : base(instance, serializableResolver)
		{
			if(fieldInformation == null)
				throw new ArgumentNullException("fieldInformation");

			this._fieldInformation = fieldInformation;
		}

		#endregion

		#region Properties

		public virtual FieldInfo FieldInformation
		{
			get { return this._fieldInformation; }
		}

		#endregion
	}
}