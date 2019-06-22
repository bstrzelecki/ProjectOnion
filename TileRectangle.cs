using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
    class TileRectangle
    {
        private int X, Y;
        public TileRectangle(int x, int y)
        {
            X = x;
            Y = y;
        }
        public TileRectangle(Vector2 pos)
        {
            X = (int)pos.X;
            Y = (int)pos.Y;
        }
        public static implicit operator Rectangle(TileRectangle rect)
        {
            return new Rectangle(rect.X * World.TileSize + World.Offset + (int)Input.cameraOffset.X, rect.Y * World.TileSize + World.Offset + (int)Input.cameraOffset.Y, World.TileSize, World.TileSize);
        }
    }
}
