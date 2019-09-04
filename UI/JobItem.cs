using System;

namespace ProjectOnion
{
	class JobItem : IUIItem
	{
		string name;
		Action OnClickAction;
		public JobItem(string name, Action OnClickAction)
		{
			this.name = name;
			this.OnClickAction = OnClickAction;
		}
		public string GetDisplayName()
		{
			return name;
		}

		public MountedObject GetObject()
		{
			return null;
		}

		public void OnClick()
		{
			OnClickAction?.Invoke();
		}
	}
}
