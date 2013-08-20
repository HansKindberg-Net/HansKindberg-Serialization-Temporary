using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HansKindberg.Serialization.InversionOfControl;

namespace HansKindberg.Serialization
{
	[Serializable]
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public class SerializableDelegate : Serializable<object>
	{
		#region Fields

		private readonly MethodInfo _methodInformation;

		#endregion

		#region Constructors

		public SerializableDelegate(MethodInfo methodInformation, object target) : this(methodInformation, target, ServiceLocator.Instance.GetService<ISerializationResolver>()) {}

		protected internal SerializableDelegate(MethodInfo methodInformation, object target, ISerializationResolver serializationResolver) : base(target, serializationResolver)
		{
			if(methodInformation == null)
				throw new ArgumentNullException("methodInformation");

			this._methodInformation = methodInformation;
		}

		#endregion

		#region Properties

		public virtual MethodInfo MethodInformation
		{
			get { return this._methodInformation; }
		}

		#endregion
	}
}