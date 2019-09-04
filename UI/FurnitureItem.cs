using System;

namespace ProjectOnion
{
	class FurnitureItem : IUIItem
	{
		MountedObject obj;
		Action OnClickAction;
		public FurnitureItem(MountedObject obj, Action action)
		{
			this.obj = obj;
			OnClickAction = action;
		}
		public string GetDisplayName()
		{
			return obj.displayName;
		}

		public MountedObject GetObject()
		{
			return obj;
		}

		public void OnClick()
		{
			OnClickAction?.Invoke();
		}
	}
}
