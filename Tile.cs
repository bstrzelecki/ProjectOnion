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
		public MountedObject mountedObject { get; protected set; }
		public Tile(int x, int y)
		{
			X = x;
			Y = y;
			sprite = new Sprite("grid");
		}

		internal void Draw(SpriteBatch sprite)
		{
			if(!mountedObject.IsOpaque)
				sprite.Draw(this.sprite, new TileRectangle(X, Y), Color.White);
			mountedObject.Draw(sprite);
		}
		//TODO:
		public void PlaceObject(MountedObject mounted)
		{
			mountedObject = mounted;
			mountedObject.AssignPosition(Position);
		}
	}
}
