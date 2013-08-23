using System;
using System.Reflection;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// Used to serialize field values. This class is mainly for internal use and is not intended to be used in your code. Use <see cref="Serializable&lt;T&gt;" /> instead.
	/// </summary>
	[Serializable]
	public class SerializableField : Serializable
	{
		#region Fields

		private readonly FieldInfo _fieldInformation;

		#endregion

		#region Constructors

		public SerializableField(FieldInfo fieldInformation, object instance) : base(instance)
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
	}
}