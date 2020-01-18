using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBBSlib.MonoGame;

namespace ProjectOnion
{
	class Fabricator : NewFurniture
	{
		protected override void SetProperties()
		{
			obj.displayName = "Fabricator";
			obj.registryName = "fabricator";
			obj.category = "Production";
			obj.sprite = new Sprite("fabricator");
			obj.characterCanEnter = false;
			obj.moveCost = new TimeUnit(0f);
			obj.pfMoveCost = 4;
			obj.objectEvents = new DoorEvent(obj);
			obj.objectUseEvent = new DoorUse(obj);
			obj.objectUpdate = new DoorEvent(obj);
			obj.flags.Add("wall");
			obj.resources = new ItemStack[] { new ItemStack("METAL", 20) };
			OnClickAction = () => Architect.SetBuildObject(BuildMode.Single, BuildType.Furniture, new Fabricator());
		}
	}

}
