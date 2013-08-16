using System;
using System.Diagnostics.CodeAnalysis;

namespace HansKindberg.Serialization
{
	[Serializable]
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public class SerializableDelegate : Serializable
	{
		#region Constructors

		public SerializableDelegate(Delegate @delegate) : base(@delegate, SerializableResolverLocator.Instance.SerializableResolver) {}
		protected internal SerializableDelegate(Delegate @delegate, ISerializableResolver serializableResolver) : base(@delegate, serializableResolver) {}

		#endregion

		#region Properties

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Delegate")]
		public virtual Delegate Delegate
		{
			get { return (Delegate) this.InstanceInternal; }
		}

		#endregion

		#region Methods

		protected internal override object CreateDeserializedInstance()
		{
			throw new NotImplementedException();
		}

		protected internal override object CreateSerializableInstance()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}