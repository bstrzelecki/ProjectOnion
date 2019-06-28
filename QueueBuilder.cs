using System.Collections.Generic;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal class QueueBuilder
	{
#pragma warning disable CS0649 // Field 'QueueBuilder.toBuild' is never assigned to, and will always have its default value null
		public static MountedObject toBuild;
#pragma warning restore CS0649 // Field 'QueueBuilder.toBuild' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'QueueBuilder.buildMode' is never assigned to, and will always have its default value
		public static BuildMode buildMode;
#pragma warning restore CS0649 // Field 'QueueBuilder.buildMode' is never assigned to, and will always have its default value

		private static List<Job> toAdd = new List<Job>();
		public static void PlaceObject(Vector2 pos)
		{
			if (MainScene.world.GetTile(pos) == null || MainScene.world.GetTile(pos).IsFloor)
			{
				return;
			}

			Job job = new Job(MainScene.world.GetTile(pos), new FloorPlaceJob(MainScene.world.GetTile(pos), new Sprite("floor")));
			toAdd.Add(job);
		}
		private static Vector2 _start;
		public static void Update()
		{
			if (Input.IsMouseKeyDown(0))
			{
				toAdd.Clear();
				if (_start == Vector2.Zero)
				{
					_start = (Input.GetMousePosition() - Input.cameraOffset) / World.TileSize;
				}
				ShapeUtils.DrawArea(_start, (Input.GetMousePosition() - Input.cameraOffset) / World.TileSize, PlaceObject);
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
	public enum BuiltType
	{
		Floor,
		Furniture,
		MultiPart
	}
}
