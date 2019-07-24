using System;
using System.Collections.Generic;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal class QueueBuilder
	{
		public static NewFurniture toBuild;

		public static BuildMode buildMode = BuildMode.None;
		public static BuildType buildType = BuildType.None;
		public static Func<Vector2, bool> TileValidator { get; set; } = (v) => true;
		public static Action<Vector2> PlaceAction { get; set; }
		private static List<Job> toAdd = new List<Job>();
		private static List<Vector2> vectors = new List<Vector2>();
		public static Func<Vector2, Job> jobOverride;
		public static void PlaceObject(Vector2 pos)
		{
			if (buildType == BuildType.None) return;
			if (MainScene.world.GetTile(pos) == null)
			{
				return;
			}
			Job job = null;
			if (jobOverride == null)
			{ 
				if (!TileValidator(pos)) return;

				if (buildType == BuildType.Floor)
				{
					if (MainScene.world.GetTile(pos).IsFloor) return;//TODO: fix for more floor variants
					job = new Job(MainScene.world.GetTile(pos), new FloorPlaceJobEvent(MainScene.world.GetTile(pos), new Sprite("floor")),jobLayer:JobLayer.Build);
				}
				else if (buildType == BuildType.Furniture)
				{
					if (toBuild != null && toBuild.Equals(MainScene.world.GetTile(pos).mountedObject)) return;
					job = new Job(MainScene.world.GetTile(pos), new FurniturePlaceJobEvent(MainScene.world.GetTile(pos), toBuild.GetFurniture()), false,jobLayer: JobLayer.Build);
				}
			}
			else
			{
				if(TileValidator(pos))
					job = jobOverride(pos);
			}
			vectors.Add(pos);
			toAdd.Add(job);
		}
		private static Vector2 _start;
		public static void Update()
		{
			if (Input.IsMouseKeyDown(0))
			{
				if (buildMode != BuildMode.Free)
				{
					toAdd.Clear();
					vectors.Clear();
				}
				if (_start == Vector2.Zero)
				{
					_start = GetMouseOnTilePosition();
				}
				switch (buildMode)
				{
					case BuildMode.Area:
						ShapeUtils.DrawArea(_start, GetMouseOnTilePosition(), PlaceObject);
						break;
					case BuildMode.Line:
						ShapeUtils.DrawLine(_start, GetMouseOnTilePosition(), PlaceObject);
						break;
					case BuildMode.Single:
						PlaceObject(GetMouseOnTilePosition());
						break;
					case BuildMode.Free:
						PlaceObject(GetMouseOnTilePosition());
						break;
					default:
						break;
				}
			}
			if (Input.IsMouseKeyUp(0))
			{
				_start = Vector2.Zero;
				foreach (Vector2 pos in vectors)
					PlaceAction?.Invoke(pos);
				vectors.Clear();
				if (buildType == BuildType.Other) return;
				foreach (Job job in toAdd)
				{
					JobQueue.AddJob(job);
				}
				toAdd.Clear();
			}
		}
		private static Vector2 GetMouseOnTilePosition()
		{
			return (Input.GetMousePosition() - Input.cameraOffset - new Vector2(World.Offset, World.Offset)) / World.TileSize;
		}
	}
	public enum BuildType
	{
		None,
		Floor,
		Furniture,
		MultiPart,
		Other
	}
}
