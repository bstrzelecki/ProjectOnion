using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal class JobEvents
	{
	}
	class FurniturePlaceJobEvent : IJobEvents
	{
		private Tile tile;
		private MountedObject mountedObject;
		private BlueprintData bp;
		private Job job;
		public FurniturePlaceJobEvent(Tile tile, MountedObject mountedObject)
		{
			this.tile = tile;
			this.mountedObject = mountedObject;
			bp = new BlueprintData(new Vector2(tile.X, tile.Y), mountedObject.sprite, new Sprite("bp"));
		}
		public BlueprintData GetBlueprintData()
		{
			return bp;
		}

		public void OnJobCanceled()
		{
			if (job == null) return;
			GameMain.UnregisterRenderer(job);
		}

		public void OnJobCompleted()
		{
			tile.PlaceObject(mountedObject);
			if (job == null) return;
			GameMain.UnregisterRenderer(job);
		}

		public void OnJobSuspended()
		{

		}
	}
	internal class FloorPlaceJobEvent : IJobEvents
	{
		private Tile tile;
		private Sprite sprite;
		private float movementCostReduction;
		private Job job;

		public FloorPlaceJobEvent(Tile tile, Sprite sprite, float movementCostReduction = 0f)
		{
			this.tile = tile;
			this.sprite = sprite;
			this.movementCostReduction = movementCostReduction;
			bp = new BlueprintData(new Vector2(tile.X, tile.Y), sprite, new Sprite("bp"));
		}

		private BlueprintData bp;
		public BlueprintData GetBlueprintData()
		{
			return bp;
		}

		public void OnJobCanceled()
		{
			if (job == null) return;
			GameMain.UnregisterRenderer(job);
		}

		public void OnJobCompleted()
		{
			tile.IsFloor = true;
			tile.sprite = sprite;
			tile.movementCost -= movementCostReduction;
			if (job == null) return;
			GameMain.UnregisterRenderer(job);
		}

		public void OnJobSuspended()
		{

		}
	}
}
