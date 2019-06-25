using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	class Character : MBBSlib.MonoGame.IDrawable, MBBSlib.MonoGame.IUpdateable
	{
		public Tile tile;
		public Tile dest;
		public Vector2 Position { get {return (tile != null)?tile.Position: Vector2.Zero; } }
		public float X { get { return Position.X; } }
		public float Y { get { return Position.Y; } }

		public float moveCompleted = 0f;

		public float moveSpeed = 1f;
		Sprite img;
		public Character()
		{
			img = new Sprite("pChar");
			tile = MainScene.world.GetTile(6, 6);
			GameMain.RegisterRenderer(this);
			GameMain.RegisterUpdate(this);
			SetDestination(MainScene.world.GetTile(10, 15));
		}
		public void SetDestination(Tile d)
		{
			dest = d;
		}
		public void Draw(SpriteBatch sprite)
		{
			sprite.Draw(img, new Rectangle((dest != null)?(Vector2.Lerp(TileRectangle.GetCorner(Position), TileRectangle.GetCorner(dest.Position), moveCompleted)).ToPoint() : TileRectangle.GetCorner(Position).ToPoint(), TileRectangle.GetSize().ToPoint()), Color.White);
		}

		public void Update()
		{
			if (dest == null) return;
			if(moveCompleted >= 1f)
			{
				tile = dest;
				dest = null;
				moveCompleted = 0;
				return;
			}
			moveCompleted += moveSpeed * 0.01f;
		}
	}
}
