using System;
using System.Diagnostics;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	internal class MountedObject : MBBSlib.MonoGame.IDrawable
	{
		public bool IsOpaque = false;
		public Sprite sprite;
		public float moveCost = 0;
		public Vector2 position;
		public Tile tile;
		public Action<Tile> OnNeighbourChanged;
		public Action OnPlaced;
		public Action OnRemoved;
		public void AssignPosition(Vector2 pos)
		{
			if (pos == Vector2.Zero) Debug.WriteLine("!!!");
			position = pos;
			tile = MainScene.world.GetTile(position);
		}
		public void Draw(SpriteBatch sprite)
		{
			sprite.Draw(this.sprite, new TileRectangle(position), Color.White);
		}
	}
}
