using System.Linq;
using MBBSlib.MonoGame;

namespace ProjectOnion
{
	class FurniturePlaceJob : Job
	{
		readonly MountedObject mountedObject;
		public FurniturePlaceJob(Tile tile, MountedObject mo) : base(tile)
		{
			mountedObject = mo;
			data = new BlueprintData(tile.Position, mountedObject.sprite, new Sprite("bp"));
			jobLayer = JobLayer.Build;
			resources = mountedObject.resources.ToList();
			onTile = false;
		}
		public override bool IsAvailable()
		{
			return (resources == null || resources.Count == 0) || (from n in resources where n.GetAmount() > 0 select n).Count() == 0;
		}
		public override void OnCancel()
		{

		}

		public override void OnComplete()
		{
			tile.PlaceObject(mountedObject);
		}

		public override void OnWork()
		{

		}
	}
}
