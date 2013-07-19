using System;

namespace HansKindberg.Serialization.IntegrationTests.Mocks
{
	public class UnserializableDisposableMockEventArgs : EventArgs
	{
		#region Fields

		private readonly UnserializableDisposableMock _unserializableDisposableMock;

		#endregion

		#region Constructors

		public UnserializableDisposableMockEventArgs(UnserializableDisposableMock unserializableDisposableMock)
		{
			this._unserializableDisposableMock = unserializableDisposableMock;
		}

		#endregion

		#region Properties

		public virtual UnserializableDisposableMock UnserializableDisposableMock
		{
			get { return this._unserializableDisposableMock; }
		}

		#endregion
	}
}