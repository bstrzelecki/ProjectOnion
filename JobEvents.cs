using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal class JobEvents
	{
	}
	class FurniturePlaceJob : IJobEvents
	{
		private Tile tile;
		private MountedObject mountedObject;
		private float movementCostReduction;
		private BlueprintData bp;
		public FurniturePlaceJob(Tile tile, MountedObject mountedObject, float mcr = 0f)
		{
			this.tile = tile;
			this.mountedObject = mountedObject;
			this.movementCostReduction = mcr;
			bp = new BlueprintData(new Vector2(tile.X, tile.Y), mountedObject.sprite, new Sprite("bp"));
		}
		public BlueprintData GetBlueprintData()
		{
			return bp;
		}

		public void OnJobCanceled()
		{
			
		}

		public void OnJobCompleted()
		{
			tile.PlaceObject(mountedObject);
			tile.movementCost += movementCostReduction;
		}

		public void OnJobSuspended()
		{
			
		}
	}
	internal class FloorPlaceJob : IJobEvents
	{
		private Tile tile;
		private Sprite sprite;
		private float movementCostReduction;

		public FloorPlaceJob(Tile tile, Sprite sprite, float movementCostReduction = 0f)
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

		}

		public void OnJobCompleted()
		{
			tile.IsFloor = true;
			tile.sprite = sprite;
			tile.movementCost -= movementCostReduction;
		}

		public void OnJobSuspended()
		{

		}
	}
}
