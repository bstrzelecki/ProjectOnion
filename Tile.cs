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

        public bool IsFloor = false;
        public MountedObject mountedObject { get; protected set; }
        public Tile(int x, int y)
        {
            X = x;
            Y = y;
            sprite = new Sprite("grid");
        }

        internal void Draw(SpriteBatch sprite)
        {
            sprite.Draw(this.sprite, new TileRectangle(X,Y), Color.White);
        }
        //TODO:
        public void PlaceObject(MountedObject mounted)
        {
            mountedObject = mounted;
        }
    }
}
