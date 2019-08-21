﻿using System;
using System.Collections.Generic;
using System.Linq;
using MBBSlib.AI;
using MBBSlib.MonoGame;

namespace ProjectOnion
{
	class Character : IDrawable, IUpdateable
	{
		#region Positions
		public Tile tile;
		public Tile dest;
		float distance = 1;
		public float moveCompleted = 0f;
		public Microsoft.Xna.Framework.Vector2 Position { get { return (tile != null) ? tile.Position : Microsoft.Xna.Framework.Vector2.Zero; } }
		public float X { get { return Position.X; } }
		public float Y { get { return Position.Y; } }
		#endregion

		#region Id
		private static int _chars = 0;
		public int id;
		#endregion

		public string Name = "Bob";

		public float WorkValue = 1f;

		public float moveSpeed = .5f;

		Sprite img;

		#region JobData
		List<Point> path;
		Queue<Job> enqueuedJobs = new Queue<Job>();
		Job currentJob;
		#endregion

		public Character()
		{
			img = new Sprite("pChar");
			tile = MainScene.world.GetTile(MainScene.rng.Next(0,15), MainScene.rng.Next(0,15));
			id = _chars;
			_chars++;
			GameMain.RegisterRenderer(this, 7);
			GameMain.RegisterUpdate(this);
			Registry.characters.Add(this);
		}
		public void SetDestination(Tile d)
		{
			moveCompleted = 0;
			distance = d.GetMovementCost();
			dest = d;
		}

		public void EnqueueJob(Job j)
		{
			enqueuedJobs.Enqueue(j);
		}
		
		#region Interface implementation
		public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
		{
			sprite.Draw(img, new Microsoft.Xna.Framework.Rectangle((dest != null) ? (Microsoft.Xna.Framework.Vector2.Lerp(TileRectangle.GetCorner(Position), TileRectangle.GetCorner(dest.Position), moveCompleted)).ToPoint() : TileRectangle.GetCorner(Position).ToPoint(), TileRectangle.GetSize().ToPoint()), Microsoft.Xna.Framework.Color.White);
		}
		public void Update()
		{
			if (!tile.IsInmovable)
			{
				if ((path == null || path.Count == 0) && currentJob == null)
					GetPath();
				if (currentJob == null) path = null;

				DoWork();
				if (path != null && path.Count != 0 && dest == null)
				{
					Point tilePos = path[0];
					path.RemoveAt(0);
					SetDestination(MainScene.world.GetTile(tilePos.X, tilePos.Y));
					if (dest.IsInmovable)
					{
						dest = null;
						path = null;
						if (currentJob.onTile) currentJob = null;
						return;
					}
				}
			}
			else
			{
				foreach (Tile t in tile.GetNeighbourTiles())
				{
					//Temp fix
					if (!t.IsInmovable) { SetDestination(t); moveCompleted = 1f; break; }

				}
			}
			if (dest == null) return;
			if (dest.IsInmovable)
			{
				dest = null;
				path = null;
				if (currentJob.onTile) currentJob = null;
				return;
			}
			if(dest.character != null && dest.character != this)
			{
				if(dest.character.currentJob != null)
					return;
			}
			if (currentJob != null && !currentJob.onTile && dest == currentJob.tile) return;

			if (moveCompleted >= 1f)
			{
				MoveCharacter();
				return;
			}
			if (dest.mountedObject == null || dest.mountedObject.characterCanEnter)
				moveCompleted += (moveSpeed * Time.DeltaTime)/distance;
			else if (dest.mountedObject != null && dest.mountedObject.objectUseEvent.CanUse(this))
				dest.mountedObject.objectUseEvent.Use(this);

		}
		#endregion

		#region private members
		private void MoveCharacter()
		{
			tile.mountedObject?.objectEvents.OnCharExit(this);
			tile.isCharOnTile = false;
			tile.character = null;
			tile = dest;
			tile.character = this;
			dest = null;
			moveCompleted = 0;
		}

		private void DoWork()
		{
			if (currentJob == null)
			{
				return;
			}
			if (!currentJob.onTile && dest == currentJob.tile)
			{
				path = null;
				currentJob.Work(1);
			}
			if (!currentJob.onTile && tile.GetNeighbourTiles().Contains(currentJob.tile))
			{
				path = null;
				dest = null;
				currentJob.Work(1);
			}
			if (currentJob.onTile && tile == currentJob.tile)
			{
				currentJob.Work(1);
			}


			if (currentJob.IsCompleted)
			{
				if (!currentJob.onTile && dest == currentJob.tile) dest = null;
				currentJob = null;
				return;
			}
		}
		private void GetPath()
		{
			var p = new Pathfinding(MainScene.world.GetPathfindingGraph());

			if (enqueuedJobs.Count > 0)
			{
				currentJob = enqueuedJobs.Dequeue();
			}
			else
			{
				currentJob = JobQueue.GetJob(this);
			}
			if (currentJob == null) return;


			path = p.GetPath(new Point((int)tile.Position.X, (int)tile.Position.Y), new Point((int)currentJob.tile.Position.X, (int)currentJob.tile.Position.Y));
			path.Reverse();
			if ((from n in path where MainScene.world.GetTile(n.X, n.Y).IsInmovable select n).Count() > 0)
			{
				if (!MainScene.world.GetTile(path[path.Count - 1].X, path[path.Count - 1].Y).IsInmovable)
				{
					JobQueue.AddJob(currentJob);
					currentJob = null;
					path.Clear();
					return;
				}
			}
		}
		#endregion
		
		#region objectOverrides
		public override bool Equals(object obj)
		{
			if (obj is Character c)
			{
				return id == c.id;
			}
			return false;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public override string ToString()
		{
			return base.ToString();
		}
		#endregion

		internal void Dispose()
		{
			GameMain.UnregisterRenderer(this, 7);
			GameMain.UnregisterUpdate(this);
		}
	}
}