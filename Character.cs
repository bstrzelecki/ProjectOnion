using System.Collections.Generic;
using MBBSlib.AI;
using MBBSlib.MonoGame;

namespace ProjectOnion
{
	class Character : IDrawable, IUpdateable
	{
		public Tile tile;
		public Tile dest;
		public Microsoft.Xna.Framework.Vector2 Position { get { return (tile != null) ? tile.Position : Microsoft.Xna.Framework.Vector2.Zero; } }
		public float X { get { return Position.X; } }
		public float Y { get { return Position.Y; } }

		public float moveCompleted = 0f;

		public float moveSpeed = .5f;
		Sprite img;
		public Character()
		{
			img = new Sprite("pChar");
			tile = MainScene.world.GetTile(6, 6);
			GameMain.RegisterRenderer(this);
			GameMain.RegisterUpdate(this);
		}
		public void SetDestination(Tile d)
		{
			dest = d;
		}
		public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
		{
			sprite.Draw(img, new Microsoft.Xna.Framework.Rectangle((dest != null) ? (Microsoft.Xna.Framework.Vector2.Lerp(TileRectangle.GetCorner(Position), TileRectangle.GetCorner(dest.Position), moveCompleted)).ToPoint() : TileRectangle.GetCorner(Position).ToPoint(), TileRectangle.GetSize().ToPoint()), Microsoft.Xna.Framework.Color.White);
		}
		List<Point> path;
		Job currentJob;
		public void Update()
		{
			if ((path == null || path.Count == 0) && currentJob == null)
			{
				var p = new Pathfinding(MainScene.world.GetPathfindingGraph());
				currentJob = JobQueue.GetJob();
				if (currentJob == null) return;
				path = p.GetPath(new Point((int)tile.Position.X, (int)tile.Position.Y), new Point((int)currentJob.tile.Position.X, (int)currentJob.tile.Position.Y));
				path.Reverse();
			}
			if (path != null && path.Count != 0 && dest == null)
			{
				Point tilePos = path[0];
				path.RemoveAt(0);
				SetDestination(MainScene.world.GetTile(tilePos.X,tilePos.Y));
			}
			if(tile == currentJob.tile)
			{
				currentJob.Work(1);
			}
			if (currentJob == null || currentJob.IsDisposed)
			{
				currentJob = null;
			}
			if (dest == null) return;
			if (moveCompleted >= 1f)
			{
				tile = dest;
				dest = null;
				moveCompleted = 0;
				return;
			}
			moveCompleted += moveSpeed * 0.1f;
		}
	}
}
