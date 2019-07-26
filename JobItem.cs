using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	class JobItem : IUIItem
	{
		string name;
		Action OnClickAction;
		public JobItem(string name, Action OnClickAction)
		{
			this.name = name;
			this.OnClickAction = OnClick;
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
