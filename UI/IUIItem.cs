using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	interface IUIItem
	{
		void OnClick();
		MountedObject GetObject();
		string GetDisplayName();
	}
}
