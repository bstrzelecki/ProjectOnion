using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	class Wall : NewFurniture
	{
		protected override void SetProperties()
		{
			obj.sprite = new MultiSprite("wall");
			obj.moveCost = float.MaxValue;
		}
	}
}
