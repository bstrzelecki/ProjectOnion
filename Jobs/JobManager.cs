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
			QueueBuilder.jobOverride = null;
			BuildController.buildMode = BuildMode.Area;
		}
		static bool CancelValidation(Vector2 v)
		{
			bool isJobOnTile = false;
			foreach (Job t in MainScene.world.GetTile(v).job)
			{
				if (t != null) { isJobOnTile = true; break; }
			}
			return isJobOnTile;
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
			Job j = new Job(t, new DeconstructJobEvent(t), (t.mountedObject == null || !t.IsInmovable), 1, JobLayer.Deconstruct);

			return j;
		}
	}
}
