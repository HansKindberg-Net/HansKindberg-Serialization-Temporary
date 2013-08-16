namespace HansKindberg.Serialization
{
	public interface ISerializableResolverLocator
	{
		#region Properties

		ISerializableResolver SerializableResolver { get; }

		#endregion
	}
}