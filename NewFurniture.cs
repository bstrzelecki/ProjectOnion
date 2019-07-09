using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	internal abstract class NewFurniture
	{
		protected MountedObject obj;

		protected abstract void SetProperties(); 

		public virtual MountedObject GetFurniture()
		{
			obj = new MountedObject();
			SetProperties();
			return obj;
		}
	}
}
