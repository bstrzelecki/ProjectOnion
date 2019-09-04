namespace ProjectOnion
{
	interface IUIItem
	{
		void OnClick();
		MountedObject GetObject();
		string GetDisplayName();
	}
}
