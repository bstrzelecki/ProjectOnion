using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal class JobEvents
	{
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
#pragma warning disable CS0414 // The field 'FloorPlaceJob.initialized' is assigned but its value is never used
		private bool initialized = false;
#pragma warning restore CS0414 // The field 'FloorPlaceJob.initialized' is assigned but its value is never used
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
