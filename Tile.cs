using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
    class Tile
    {
        public Sprite sprite;
        public readonly int X, Y;

        public Tile(int x, int y)
        {
            X = x;
            Y = y;
            sprite = new Sprite("grid");
        }

        internal void Draw(SpriteBatch sprite)
        {
            sprite.Draw(this.sprite, new Rectangle(X * World.TileSize + World.Offset + (int)Input.cameraOffset.X, Y * World.TileSize + World.Offset + (int)Input.cameraOffset.Y, World.TileSize, World.TileSize), Color.White);
        }
    }
}
