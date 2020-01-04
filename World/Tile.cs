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
		public const int SIZE_STACK_LIMIT = 95;
		public Vector2 Position { get { return new Vector2(X, Y); } }
		public float movementCost = new TimeUnit(1f);
		public bool IsFloor = false;
		public bool IsInmovable = false;
		public bool isCharOnTile = false;
		public Character character;
		public MountedObject mountedObject { get; protected set; }
		public ItemStack stackItem { get; protected set; }
		public void RemoveItemStack()
		{
			stackItem = null;
		}
		public bool PutItemStack(ItemStack stack)
		{
			if (IsInmovable) return false;
			if (stackItem == null)
			{
				stackItem = stack;
				stackItem.SetTile(this);
				return true;
			}
			if (stackItem.ToString() == stack.ToString())
			{
				if ((stack.GetAmount() + stackItem.GetAmount()) <= SIZE_STACK_LIMIT)
				{
					stackItem.AddToStack(stack.GetAmount());
					stackItem.SetTile(this);
					return true;
				}//TODO fix for splitting too large staacks
			}

			return false;
		}
		public Tile(int x, int y)
		{
			X = x;
			Y = y;
			sprite = new Sprite("grid");
			GameMain.RegisterUpdate(this);
		}
		public float GetPathfindingMovementCost()
		{
			return movementCost + (mountedObject?.pfMoveCost ?? 0f);
		}
		public float GetMovementCost()
		{
			return movementCost + (float)(mountedObject?.moveCost ?? 0f);
		}
		private bool firstDraw = true;
		internal void Draw(SpriteBatch sprite)
		{
			if (firstDraw)
			{
				mountedObject?.objectEvents?.OnNeighbourChanged(this);
				firstDraw = false;
			}
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
			var tiles = new List<Tile>
			{
				MainScene.world.GetTile(X + 1, Y),
				MainScene.world.GetTile(X - 1, Y),
				MainScene.world.GetTile(X, Y + 1),
				MainScene.world.GetTile(X, Y - 1)
			};
			if (fourDirs) return tiles;
			tiles.Add(MainScene.world.GetTile(X + 1, Y + 1));
			tiles.Add(MainScene.world.GetTile(X + 1, Y - 1));
			tiles.Add(MainScene.world.GetTile(X - 1, Y + 1));
			tiles.Add(MainScene.world.GetTile(X - 1, Y - 1));
			return tiles;

		}
		public void DeconstructObject()
		{
			IsInmovable = false;
			PutItemStacks(mountedObject.resources);
			mountedObject = null;


			foreach (Tile tile in GetNeighbourTiles())
			{
				if (tile == null || tile.mountedObject == null) continue;
				tile.mountedObject.objectEvents.OnNeighbourChanged(this);
			}
		}
		public void PutItemStacks(params ItemStack[] stacks)
		{
			foreach (ItemStack s in stacks)
			{
				bool breaked = false;
				foreach (Tile tile in GetNeighbourTiles())
				{
					if (tile.stackItem != null && tile.stackItem.GetResource() == s.GetResource())
					{
						if (tile.PutItemStack(s))
						{
							breaked = true;
							break;
						}
					}
				}
				if (breaked) continue;
				if (!PutItemStack(s))
				{
					foreach (Tile tile in GetNeighbourTiles())
					{
						if (tile.PutItemStack(s))
						{
							break;
						}
					}
				}
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
