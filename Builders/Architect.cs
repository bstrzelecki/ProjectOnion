using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	class Architect
	{
		public static void SetBuildObject(BuildMode buildMode, BuildType buildType, NewFurniture mountedObject = null)
		{
			QueueBuilder.buildType = buildType;
			QueueBuilder.buildMode = buildMode;
			QueueBuilder.PlaceAction = null;
			BuildController.buildMode = buildMode;

			if (buildType == BuildType.Floor)
			{
				QueueBuilder.TileValidator = (v) => true;
			}
			if (buildType == BuildType.Furniture)
			{
				QueueBuilder.TileValidator = (v) => MainScene.world.GetTile(v).IsFloor;
			}
			if (mountedObject != null)
				QueueBuilder.toBuild = mountedObject;
		}
	}
}
