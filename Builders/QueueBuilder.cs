using System.Collections.Generic;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal class QueueBuilder
	{
		public static NewFurniture toBuild;

		public static BuildMode buildMode = BuildMode.None;
		public static BuildType buildType = BuildType.Floor;

		private static List<Job> toAdd = new List<Job>();
		public static void PlaceObject(Vector2 pos)
		{
			if (MainScene.world.GetTile(pos) == null)
			{
				return;
			}
			Job job = null;
			if (buildType == BuildType.Floor)
			{
				job = new Job(MainScene.world.GetTile(pos), new FloorPlaceJobEvent(MainScene.world.GetTile(pos), new Sprite("floor")));
			} else if (buildType == BuildType.Furniture)
			{
				job = new Job(MainScene.world.GetTile(pos), new FurniturePlaceJobEvent(MainScene.world.GetTile(pos), toBuild.GetFurniture()), false);
			}
			toAdd.Add(job);
		}
		private static Vector2 _start;
		public static void Update()
		{
			if (Input.IsMouseKeyDown(0))
			{
				if(buildMode != BuildMode.Free)
					toAdd.Clear();
				if (_start == Vector2.Zero)
				{
					_start = (Input.GetMousePosition() - Input.cameraOffset) / World.TileSize;
				}
				switch (buildMode) {
					case BuildMode.Area:
						ShapeUtils.DrawArea(_start, (Input.GetMousePosition() - Input.cameraOffset) / World.TileSize, PlaceObject); 
						break;
					case BuildMode.Line:
						ShapeUtils.DrawLine(_start, (Input.GetMousePosition() - Input.cameraOffset) / World.TileSize, PlaceObject);
						break;
					case BuildMode.Single:
						PlaceObject((Input.GetMousePosition() - Input.cameraOffset) / World.TileSize);
						break;
					case BuildMode.Free:
						PlaceObject((Input.GetMousePosition() - Input.cameraOffset) / World.TileSize);
						break;
					default:
						break;
				}
			} 
			if (Input.IsMouseKeyUp(0))
			{
				_start = Vector2.Zero;
				foreach (Job job in toAdd)
				{
					JobQueue.AddJob(job);
				}
			}
		}
	}
	public enum BuildType
	{
		Floor,
		Furniture,
		MultiPart
	}
}
