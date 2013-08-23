using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace HansKindberg.Serialization
{
	/// <summary>
	/// Used to serialize delegates. This class is mainly for internal use and is not intended to be used in your code. Use <see cref="Serializable&lt;T&gt;" /> instead.
	/// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public class SerializableDelegate : Serializable
	{
		#region Fields

		private readonly MethodInfo _methodInformation;

		#endregion

		#region Constructors

		public SerializableDelegate(MethodInfo methodInformation, object target) : base(target)
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