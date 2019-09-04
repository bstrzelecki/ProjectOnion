using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	internal class World : IDrawable, IEnumerable<Tile>
	{
		public readonly int Height, Width;
		public Tile[,] map;
		public static int Offset = 0;
		public static int TileSize = 64;
		public float[,] GetPathfindingGraph()
		{
			float[,] p = new float[Height, Width];
			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					p[i, j] = map[i, j].GetPathfindingMovementCost();
				}
			}
			return p;
		}
		public void SetupMap(Tile[,] tiles)
		{
			if (tiles != null)
				map = tiles;
		}
		public World(int height, int width)
		{
			Height = height;
			Width = width;
			map = new Tile[Height, Width];
			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					map[i, j] = new Tile(i, j);
				}
			}
			GameMain.RegisterRenderer(this);
		}
		public Tile GetTile(Microsoft.Xna.Framework.Vector2 pos)
		{
			return GetTile((int)pos.X, (int)pos.Y);
		}
		public Tile GetTile(int x, int y)
		{
			if (x < 0 || y < 0 || x >= Width || y >= Height)
			{
				Debug.WriteLine($"Returning null at {x}, {y}");
				return null;
			}

			return map[x, y];
		}

		public void Draw(SpriteBatch sprite)
		{
			foreach (Tile tile in map)
			{
				tile.Draw(sprite);
			}
		}

		public IEnumerator<Tile> GetEnumerator()
		{
			return new TIleEnumerator(map);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
	class TIleEnumerator : IEnumerator<Tile>
	{
		public Tile Current { get; private set; } = null;
		Tile[,] map;
		public TIleEnumerator(Tile[,] map)
		{
			this.map = map;
		}
		public Tile GetTile(int x, int y)
		{
			if (x < 0 || y < 0 || x >= MainScene.world.Width || y >= MainScene.world.Height)
			{
				Debug.WriteLine($"Returning null at {x}, {y}");
				return null;
			}

			return map[x, y];
		}
		object IEnumerator.Current => Current;

		public void Dispose()
		{
			map = null;
		}
		int x = 0;
		int y = 0;
		public bool MoveNext()
		{
			Current = GetTile(x, y);
			x++;
			if (x >= MainScene.world.Width)
			{
				x = 0;
				y++;
				if (y >= MainScene.world.Height)
				{
					return false;
				}
			}
			return true;
		}

		public void Reset()
		{
			x = 0;
			y = 0;
			Current = map[0, 0];
		}
	}
}
