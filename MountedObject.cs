using System;
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
		private Vector2 _positon;
		public Action OnNeighbourChanged;
		public Action OnPlaced;
		public Action OnRemoved;
		public void AssignPosition(Vector2 pos)
		{
			_positon = pos;
		}
		public void Draw(SpriteBatch sprite)
		{
			sprite.Draw(this.sprite, new TileRectangle(_positon), Color.White);
		}
	}
}
