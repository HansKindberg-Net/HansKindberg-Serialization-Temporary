namespace HansKindberg.Serialization
{
	public interface ISerializableFactory
	{
		#region Methods

		T CreateUninitializedObject<T>();

		#endregion
	}
}