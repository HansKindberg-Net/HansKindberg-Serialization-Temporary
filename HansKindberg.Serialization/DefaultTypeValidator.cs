using System;
using System.Globalization;

namespace HansKindberg.Serialization
{
	public class DefaultTypeValidator : ITypeValidator
	{
		#region Methods

		public virtual void ValidateThatTheTypeIsADelegate(Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			if(!typeof(Delegate).IsAssignableFrom(type))
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The type \"{0}\" must be a delegate ({1}).", type.FullName, typeof(Delegate)), "type");
		}

		#endregion
	}
}