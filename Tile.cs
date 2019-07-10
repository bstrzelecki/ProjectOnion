using System.Collections.Generic;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	internal class Tile
	{
		public Sprite sprite;
		public readonly int X, Y;
		public Vector2 Position { get { return new Vector2(X, Y); } }
		public float movementCost = 1f;
		public bool IsFloor = false;
		public bool IsInmovable = false;
		public MountedObject mountedObject { get; protected set; }
		public Tile(int x, int y)
		{
			X = x;
			Y = y;
			sprite = new Sprite("grid");
		}

		internal void Draw(SpriteBatch sprite)
		{
			if (mountedObject != null)
			{
				if (!mountedObject.IsOpaque)
					sprite.Draw(this.sprite, new TileRectangle(X, Y), Color.White);
				mountedObject.Draw(sprite);
			}
			else
			{
				sprite.Draw(this.sprite, new TileRectangle(X, Y), Color.White);
			}
		}
		public List<Tile> GetNeighbourTiles(bool fourDirs = true)
		{
			List<Tile> tiles = new List<Tile>();
			tiles.Add(MainScene.world.GetTile(X + 1, Y));
			tiles.Add(MainScene.world.GetTile(X - 1, Y));
			tiles.Add(MainScene.world.GetTile(X, Y + 1));
			tiles.Add(MainScene.world.GetTile(X, Y - 1));
			if (fourDirs) return tiles;
			tiles.Add(MainScene.world.GetTile(X + 1, Y + 1));
			tiles.Add(MainScene.world.GetTile(X + 1, Y - 1));
			tiles.Add(MainScene.world.GetTile(X -1, Y + 1));
			tiles.Add(MainScene.world.GetTile(X - 1, Y - 1));
			return tiles;

		}
		//TODO:
		public void PlaceObject(MountedObject mounted)
		{
			mountedObject = mounted;
			movementCost += mountedObject.moveCost;
			if (mountedObject.moveCost == float.MaxValue) IsInmovable = true;
			mountedObject.AssignPosition(Position);
		}
	}
}
