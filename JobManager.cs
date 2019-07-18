using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	class JobManager
	{
		public static void SetCancelJobMode()
		{
			QueueBuilder.buildMode = BuildMode.Area;
			QueueBuilder.buildType = BuildType.Other;
			QueueBuilder.toBuild = null;
			QueueBuilder.TileValidator = CancelValidation;
			QueueBuilder.PlaceAction = (v) => { foreach (Job j in MainScene.world.GetTile(v).job) j?.Cancel(); };
			BuildController.buildMode = BuildMode.Area;
		}
		static bool CancelValidation(Vector2 v)
		{
			bool isJobOnTile = false;
			foreach(Job t in MainScene.world.GetTile(v).job)
			{
				if (t != null) { isJobOnTile = true;  break; }
			}
			return isJobOnTile;
		}
	}
}
