using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HansKindberg.Serialization
{
	[Serializable]
	public class Serializable : ISerializable
	{
		#region Constructors

		protected Serializable(SerializationInfo info, StreamingContext context) {}

		#endregion

		#region Methods

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}