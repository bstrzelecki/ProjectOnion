using MBBSlib.MonoGame;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
    class World : IDrawable
    {
        public readonly int Height, Width;
        public Tile[,] map;
        public static int Offset = 0;
        public static int TileSize = 64;
        public World (int height, int width)
        {
            Height = height;
            Width = width;
            map = new Tile[Height, Width];
            for(int i = 0; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
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
            if (x < 0 || y < 0) return null;
            if (x > Width || y > Height) return null;
            return map[x, y];
        }

        public void Draw(SpriteBatch sprite)
        {
            foreach(Tile tile in map)
            {
                tile.Draw(sprite);
            }
        }
    }
}
