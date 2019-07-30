using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	interface ITagCompound
	{
		void OnLoad(TagCompound compound);
		void OnSave(TagCompound compound);
	}
}
