namespace ProjectOnion
{
	interface ISerializable
	{
		void OnLoad(TagCompound compound);
		void OnSave(TagCompound compound);
	}
}
