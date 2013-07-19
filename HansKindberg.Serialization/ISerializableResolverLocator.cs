namespace HansKindberg.Serialization
{
	public interface ISerializableResolverLocator
	{
		#region Properties

		ISerializableArrayResolver SerializableArrayResolver { get; }
		ISerializableDelegateResolver SerializableDelegateResolver { get; }
		ISerializableResolver SerializableResolver { get; }

		#endregion
	}
}