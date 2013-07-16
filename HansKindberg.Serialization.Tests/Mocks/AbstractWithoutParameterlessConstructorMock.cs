namespace HansKindberg.Serialization.Tests.Mocks
{
	public abstract class AbstractWithoutParameterlessConstructorMock
	{
		#region Fields

		private readonly object _firstConstructorParameter;
		private readonly string _secondConstructorParameter;

		#endregion

		#region Constructors

		protected AbstractWithoutParameterlessConstructorMock(object firstConstructorParameter, string secondConstructorParameter)
		{
			this._firstConstructorParameter = firstConstructorParameter;
			this._secondConstructorParameter = secondConstructorParameter;
		}

		#endregion

		#region Properties

		public virtual object FirstConstructorParameter
		{
			get { return this._firstConstructorParameter; }
		}

		public virtual string SecondConstructorParameter
		{
			get { return this._secondConstructorParameter; }
		}

		#endregion
	}
}