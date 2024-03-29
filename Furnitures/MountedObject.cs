﻿using System.Collections.Generic;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	internal class MountedObject : MBBSlib.MonoGame.IDrawable
	{
		public string registryName;
		public string displayName;
		public string category;
		public Point tileOffset = Vector2.Zero.ToPoint();
		public bool IsOpaque = false;
		public bool characterCanEnter = true;
		public Sprite sprite;
		public float moveCost = 0;
		public float pfMoveCost = 0;
		public Vector2 position;
		public Tile tile;
		public IObjectEvents objectEvents;
		public IUseable objectUseEvent;
		public MBBSlib.MonoGame.IUpdateable objectUpdate;
		public ISerializable serializer;
		public MBBSlib.MonoGame.IUpdateable update;
		public List<string> flags = new List<string>();
		public ItemStack[] resources;
		public void AssignPosition(Vector2 pos)
		{
			position = pos;
			tile = MainScene.world.GetTile(position);
		}
		public void Draw(RenderBatch sprite)
		{
			Rectangle rect = new Rectangle(((Rectangle)new TileRectangle(position)).Location + tileOffset, ((Rectangle)new TileRectangle(position)).Size);
			sprite.Draw(this.sprite, rect, Color.White);
		}
	}

	internal interface IObjectEvents
	{
		void OnNeighbourChanged(Tile tile);
		void OnPlaced();
		void OnRemoved();
		void OnCharEnter(Character c);
		void OnCharExit(Character c);
	}
	internal interface IUseable
	{
		bool CanUse(Character c);
		void Use(Character c);
	}

}
