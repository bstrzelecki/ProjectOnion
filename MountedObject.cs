using System;
using System.Collections.Generic;
using System.Diagnostics;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	internal class MountedObject : MBBSlib.MonoGame.IDrawable
	{
		public string registryName;
		public string displayName;
		public bool IsOpaque = false;
		public Sprite sprite;
		public float moveCost = 0;
		public Vector2 position;
		public Tile tile;
		public IObjectEvents objectEvents;
		public List<string> flags = new List<string>();
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

	internal interface IObjectEvents
	{
		void OnNeighbourChanged(Tile tile);
		void OnPlaced();
		void OnRemoved();
	}
	internal interface IUseable
	{
		bool CanUse(Character c);
		void Use();
	}

}
