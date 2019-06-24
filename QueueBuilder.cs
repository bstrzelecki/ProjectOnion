using System.Collections.Generic;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal class QueueBuilder
	{
		public static MountedObject toBuild;
		public static BuildMode buildMode;

		private static List<Job> toAdd = new List<Job>();
		public static void PlaceObject(Vector2 pos)
		{
			if (MainScene.world.GetTile(pos) == null)
			{
				return;
			}

			if (MainScene.world.GetTile(pos).IsFloor)
			{
				return;
			}

			//List <Job> jobs = JobQueue.GetJobsAtPosition(pos);
			//foreach(Job j in jobs)
			//{
			//	if (j.jobEvents is FloorPlaceJob)
			//		j.Dispose();
			//}

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
