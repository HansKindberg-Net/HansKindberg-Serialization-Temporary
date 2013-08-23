using System;

namespace HansKindberg.Serialization.IntegrationTests.Mocks
{
	public class CircularReferenceInnerMock
	{
		#region Fields

		private readonly CircularReferenceOuterMock _circularReferenceOuter;
		private readonly string _text;

		#endregion

		#region Constructors

		public CircularReferenceInnerMock(CircularReferenceOuterMock circularReferenceOuter)
		{
			if(circularReferenceOuter == null)
				throw new ArgumentNullException("circularReferenceOuter");

			this._circularReferenceOuter = circularReferenceOuter;
			this._text = "Text from CircularReferenceOuterMock.";
		}

		#endregion

		#region Properties

		public virtual CircularReferenceOuterMock CircularReferenceOuter
		{
			get { return this._circularReferenceOuter; }
		}

		public virtual string Text
		{
			get { return this._text; }
		}

		#endregion
	}
}