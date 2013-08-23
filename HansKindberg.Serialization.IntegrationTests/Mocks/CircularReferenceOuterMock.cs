namespace HansKindberg.Serialization.IntegrationTests.Mocks
{
	public class CircularReferenceOuterMock
	{
		#region Fields

		private readonly CircularReferenceInnerMock _circularReferenceInner;
		private readonly string _text;

		#endregion

		#region Constructors

		public CircularReferenceOuterMock()
		{
			this._circularReferenceInner = new CircularReferenceInnerMock(this);
			this._text = "Text from CircularReferenceInnerMock.";
		}

		#endregion

		#region Properties

		public virtual CircularReferenceInnerMock CircularReferenceInner
		{
			get { return this._circularReferenceInner; }
		}

		public virtual string Text
		{
			get { return this._text; }
		}

		#endregion
	}
}