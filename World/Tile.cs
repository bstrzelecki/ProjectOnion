using System;
using System.Collections.Generic;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	internal class Tile : MBBSlib.MonoGame.IUpdateable
	{
		public Sprite sprite;
		public readonly int X, Y;
		public Vector2 Position { get { return new Vector2(X, Y); } }
		public float movementCost = new TimeUnit(1f);
		public bool IsFloor = false;
		public bool IsInmovable = false;
		public bool isCharOnTile = false;
		public Character character;
		public Job[] job = new Job[Enum.GetNames(typeof(JobLayer)).Length];
		public MountedObject mountedObject { get; protected set; }
		public StackItem stackItem { get; protected set; }
		public void RemoveItemStack()
		{
			stackItem = null;
		}
		public bool PutItemStack(StackItem stack)
		{
			if (stackItem == null)
			{
				stackItem = stack;
				return true;
			}
			if (stackItem.ToString() == stack.ToString())
			{
				stackItem.AddToStack(stack.GetAmount());
				return true;
			}
			return false;
		}
		public Tile(int x, int y)
		{
			X = x;
			Y = y;
			sprite = new Sprite("grid");
			GameMain.RegisterUpdate(this);
			stackItem = new StackItem(this, "METAL", 50);
		}
		public float GetPathfindingMovementCost()
		{
			return movementCost + (mountedObject?.pfMoveCost??0f);
		}
		public float GetMovementCost()
		{
			return movementCost + (float)(mountedObject?.moveCost??0f);
		}
		private bool firstDraw = true;
		internal void Draw(SpriteBatch sprite)
		{
			if (firstDraw)
				mountedObject?.objectEvents?.OnNeighbourChanged(this);
			if (mountedObject != null)
			{
				if (!mountedObject.IsOpaque)
					sprite.Draw(this.sprite, new TileRectangle(X, Y), Color.White);
				mountedObject.Draw(sprite);
			}
			else
			{
				sprite.Draw(this.sprite, new TileRectangle(X, Y), Color.White);
			}
			stackItem?.Draw(sprite);
		}
		public void OnTagLoad(TagCompound compound)
		{
			mountedObject.serializer?.OnLoad(compound);
		}
		public void OnTagSave(TagCompound compound)
		{
			mountedObject.serializer?.OnSave(compound);
		}
		public List<Tile> GetNeighbourTiles(bool fourDirs = true)
		{
			List<Tile> tiles = new List<Tile>();
			tiles.Add(MainScene.world.GetTile(X + 1, Y));
			tiles.Add(MainScene.world.GetTile(X - 1, Y));
			tiles.Add(MainScene.world.GetTile(X, Y + 1));
			tiles.Add(MainScene.world.GetTile(X, Y - 1));
			if (fourDirs) return tiles;
			tiles.Add(MainScene.world.GetTile(X + 1, Y + 1));
			tiles.Add(MainScene.world.GetTile(X + 1, Y - 1));
			tiles.Add(MainScene.world.GetTile(X - 1, Y + 1));
			tiles.Add(MainScene.world.GetTile(X - 1, Y - 1));
			return tiles;

		}
		public void DeconstructObject()
		{
			mountedObject = null;
			IsInmovable = false;


			foreach (Tile tile in GetNeighbourTiles())
			{
				if (tile == null || tile.mountedObject == null) continue;
				tile.mountedObject.objectEvents.OnNeighbourChanged(this);
			}
		}
		public void PlaceObject(MountedObject mounted)
		{
			mountedObject = mounted;
			if (mountedObject.moveCost == float.MaxValue) IsInmovable = true;
			mountedObject.AssignPosition(Position);
			mountedObject.objectEvents.OnPlaced();
			foreach (Tile tile in GetNeighbourTiles())
			{
				if (tile == null || tile.mountedObject == null) continue;
				tile.mountedObject.objectEvents.OnNeighbourChanged(this);
			}
			mountedObject.objectEvents.OnNeighbourChanged(this);
		}

		public void Update()
		{
			if (mountedObject != null) mountedObject.objectUpdate?.Update();
		}
	}
}
