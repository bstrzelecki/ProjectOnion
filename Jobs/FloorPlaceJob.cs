using MBBSlib.MonoGame;

namespace ProjectOnion
{
	class FloorPlaceJob : Job
	{
		readonly Sprite img;
		readonly float moveSpeed;
		public FloorPlaceJob(Tile tile, Sprite sprite, float moveSpeed) : base(tile)
		{
			img = sprite;
			this.moveSpeed = moveSpeed;
			data = new BlueprintData(tile.Position, img, new Sprite("bp"));
			jobLayer = JobLayer.Build;
		}

		public override void OnCancel()
		{
		}

		public override void OnComplete()
		{
			tile.IsFloor = true;
			tile.sprite = img;
			tile.movementCost += moveSpeed;
		}

		public override void OnWork()
		{
		}
	}
}
