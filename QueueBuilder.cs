using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	class QueueBuilder
	{
		public static MountedObject toBuild;
		public static BuildMode buildMode;

		public static void PlaceObject(Vector2 pos)
		{
			if (MainScene.world.GetTile(pos) == null) return;
			if (MainScene.world.GetTile(pos).IsFloor) return;

			//List <Job> jobs = JobQueue.GetJobsAtPosition(pos);
			//foreach(Job j in jobs)
			//{
			//	if (j.jobEvents is FloorPlaceJob)
			//		j.Dispose();
			//}

			Job job = new Job(MainScene.world.GetTile(pos), new FloorPlaceJob(MainScene.world.GetTile(pos), new Sprite("floor")));
			JobQueue.AddJob(job);
		}
		private static Vector2 _start;
		public static void Update()
		{
			if (Input.IsMouseKeyDown(0))
			{
				if (_start == Vector2.Zero)
				{
					_start = (Input.GetMousePosition() - Input.cameraOffset) / World.TileSize;
				}
				ShapeUtils.DrawArea(_start, (Input.GetMousePosition() - Input.cameraOffset) / World.TileSize, PlaceObject);
			}
			else
			{
				_start = Vector2.Zero;
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
