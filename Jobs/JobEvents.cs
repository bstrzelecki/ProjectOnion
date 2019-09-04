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
		public FurniturePlaceJobEvent(Tile tile, MountedObject mountedObject)
		{
			this.tile = tile;
			this.mountedObject = mountedObject;
			bp = new BlueprintData(new Vector2(tile.X, tile.Y), mountedObject.sprite, new Sprite("bp"));
		}
		public FurniturePlaceJobEvent(Tile tile, string mo)
		{
			this.tile = tile;
			this.mountedObject = Registry.furnitures[mo].GetFurniture();
			bp = new BlueprintData(new Vector2(tile.X, tile.Y), mountedObject.sprite, new Sprite("bp"));
		}
		public BlueprintData GetBlueprintData()
		{
			return bp;
		}

		public string[] GetSerializationData()
		{
			return new string[] { mountedObject.registryName };
		}

		public void OnJobCanceled()
		{

		}

		public void OnJobCompleted()
		{
			tile.PlaceObject(mountedObject);

		}

		public void OnJobSuspended()
		{

		}
	}
	internal class DeconstructJobEvent : IJobEvents
	{
		Tile tile;
		BlueprintData bp;

		public DeconstructJobEvent(Tile tile)
		{
			this.tile = tile;
			bp = new BlueprintData(new Vector2(tile.X, tile.Y), tile.mountedObject?.sprite ?? tile.sprite, new Sprite("ds"));
		}
		public BlueprintData GetBlueprintData()
		{
			return bp;
		}

		public string[] GetSerializationData()
		{
			return new string[] { string.Empty };
		}

		public void OnJobCanceled()
		{

		}

		public void OnJobCompleted()
		{
			if (tile.mountedObject != null)
				tile.DeconstructObject();
			else
			{
				tile.sprite = new Sprite("grid");
				tile.IsFloor = false;
			}
		}

		public void OnJobSuspended()
		{
			throw new System.NotImplementedException();
		}
	}

	internal class FloorPlaceJobEvent : IJobEvents
	{
		private readonly Tile tile;
		private readonly Sprite sprite;
		private readonly float movementCostReduction;

		public FloorPlaceJobEvent(Tile tile, Sprite sprite, float movementCostReduction = 0f)
		{
			this.tile = tile;
			this.sprite = sprite;
			this.movementCostReduction = movementCostReduction;
			bp = new BlueprintData(new Vector2(tile.X, tile.Y), sprite, new Sprite("bp"));
		}
		public FloorPlaceJobEvent(Tile tile, string sprite, string cost)
		{
			this.tile = tile;
			this.sprite = new Sprite(sprite);
			this.movementCostReduction = int.Parse(cost);
			bp = new BlueprintData(new Vector2(tile.X, tile.Y), this.sprite, new Sprite("bp"));

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

		public string[] GetSerializationData()
		{
			return new string[] { sprite.ToString(), movementCostReduction.ToString() };
		}
	}
}
