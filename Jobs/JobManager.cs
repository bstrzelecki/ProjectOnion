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
			QueueBuilder.PlaceAction = (v) => {
				foreach (var j in JobQueue.GetPendingJobs().Keys)
				{
					foreach (var job in JobQueue.GetPendingJobs()[j])
					{
						if (job.tile == MainScene.world.GetTile(v))
						{
							JobQueue.CancelJob(job);
							break;
						}
					}
				}
				foreach (var j in JobQueue.GetActiveJobs().Keys)
				{
					foreach (Job job in JobQueue.GetActiveJobs()[j])
					{
						if (job.tile == MainScene.world.GetTile(v)) 
						{
							JobQueue.CancelJob(job);
							break;
						}
					}
				}

			};
			QueueBuilder.jobOverride = null;
			BuildController.buildMode = BuildMode.Area;
		}
		static bool CancelValidation(Vector2 v)
		{
			foreach(var j in JobQueue.GetPendingJobs().Keys)
			{
				foreach(var job in JobQueue.GetPendingJobs()[j])
				{
					if (job.tile == MainScene.world.GetTile(v)) return true;
				}
			}
			foreach (var j in JobQueue.GetActiveJobs().Keys)
			{
				foreach (Job job in JobQueue.GetActiveJobs()[j])
				{
					if (job.tile == MainScene.world.GetTile(v)) return true;
				}
			}
			return false;
		}
		public static void SetDeconstructJob()
		{
			QueueBuilder.buildMode = BuildMode.Area;
			QueueBuilder.buildType = BuildType.Furniture;
			QueueBuilder.toBuild = null;
			QueueBuilder.TileValidator = DeconstructValidation;
			QueueBuilder.PlaceAction = null;
			QueueBuilder.jobOverride = DeconstructCreator;
			BuildController.buildMode = BuildMode.Area;
		}
		public static void SetZonePainterJob()
		{
			QueueBuilder.buildMode = BuildMode.Area;
			QueueBuilder.buildType = BuildType.Furniture;
			QueueBuilder.toBuild = null;
			QueueBuilder.TileValidator = ZonePaintValidation;
			QueueBuilder.PlaceAction = ZonePlaceAction;
			QueueBuilder.jobOverride = null;
			BuildController.buildMode = BuildMode.Area;
		}
		static void ZonePlaceAction(Vector2 v)
		{
			Tile t = MainScene.world.GetTile(v);
			ZoneManager.AddZone(t);
		}
		static bool ZonePaintValidation(Vector2 v)
		{
			Tile t = MainScene.world.GetTile(v);
			if (t.IsFloor && t.mountedObject == null && !ZoneManager.IsZoneOnTile(t))
				return true;
			return false;
		}
		static bool DeconstructValidation(Vector2 v)
		{
			Tile t = MainScene.world.GetTile(v);
			if (t.IsFloor)
				return true;
			if (t.mountedObject != null)
				return true;
			return false;
		}
		static Job DeconstructCreator(Vector2 v)
		{
			Tile t = MainScene.world.GetTile(v);
			Job j = new DeconstructJob(t);

			return j;
		}
	}
}
