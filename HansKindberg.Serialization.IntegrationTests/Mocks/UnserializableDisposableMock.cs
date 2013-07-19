using System;
using System.Diagnostics.CodeAnalysis;

namespace HansKindberg.Serialization.IntegrationTests.Mocks
{
	public class UnserializableDisposableMock : IDisposable
	{
		#region Fields

		public const string DisposedPropertyValue = "Disposed";

		#endregion

		#region Events

		public event EventHandler<UnserializableDisposableMockEventArgs> Disposing;

		#endregion

		#region Properties

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Property")]
		public virtual string Property { get; set; }

		#endregion

		#region Methods

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		[SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "HansKindberg.Serialization.IntegrationTests.Mocks.DisposeEventArgs.#ctor(System.String)")]
		protected virtual void Dispose(bool disposing)
		{
			if(!disposing)
				return;

			this.Property = DisposedPropertyValue;

			if(this.Disposing != null)
				this.Disposing(this, new UnserializableDisposableMockEventArgs(this));
		}

		#endregion
	}
}