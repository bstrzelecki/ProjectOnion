using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	class Door : NewFurniture
	{
		protected override void SetProperties()
		{
			obj.displayName = "Door";
			obj.registryName = "door";
			obj.sprite = new MultiSprite("door");
			obj.characterCanEnter = false;
			obj.moveCost = 4;
			obj.objectEvents = new DoorEvent(obj);
			obj.objectUseEvent = new DoorUse(obj);
			obj.objectUpdate = new DoorEvent(obj);
			obj.flags.Add("wall");
		}
	}
	class DoorEvent : IObjectEvents, MBBSlib.MonoGame.IUpdateable
	{
		MountedObject obj;
		public DoorEvent(MountedObject r)
		{
			obj = r;
		}
		private void UpdateSprite()
		{
			if (obj.tile == null)
			{
				obj.tile = MainScene.world.GetTile(obj.position);
			}
			if (obj.sprite is MultiSprite ms)
			{
				ms.SetTextureVariant(string.Empty);
				bool n = false, e = false, s = false, w = false;
				foreach (Tile tile in obj.tile.GetNeighbourTiles())
				{
					if (tile == null) continue;
					if (tile.mountedObject == null || !tile.mountedObject.flags.Contains("wall")) continue;
					Vector2 delta = tile.Position - obj.tile.Position;
					if (delta == new Vector2(0, -1))
						n = true;
					if (delta == new Vector2(1, 0))
						e = true;
					if (delta == new Vector2(0, 1))
						s = true;
					if (delta == new Vector2(-1, 0))
						w = true;
				}
				ms.SetTextureVariant((n && s)?"NS":(e && w)?"EW":"");
			}
		}
		public void OnNeighbourChanged(Tile tile)
		{
			UpdateSprite();
		}
		public void OnPlaced()
		{
			UpdateSprite();
		}

		public void OnRemoved()
		{
			throw new NotImplementedException();
		}

		public void OnCharEnter(Character c)
		{

		}

		public void OnCharExit(Character c)
		{
			
		}
		public void Update()
		{
			bool isClosing = true;
			foreach(Tile t in obj.tile.GetNeighbourTiles())
			{
				if (t.isCharOnTile)
				{
					isClosing = false;
					break;
				}
			}
			if (isClosing)
			{
				if (obj.objectUseEvent is DoorUse du)
				{
					if (du.openStatus > 0)
					{
						du.openStatus -= 1f / 100f;
						if (obj.sprite is MultiSprite ms)
						{
							if (ms.Variant == "NS")
								obj.tileOffset.Y = (int)((float)World.TileSize * du.openStatus);
							if (ms.Variant == "EW")
								obj.tileOffset.X = (int)((float)World.TileSize * du.openStatus);
							if (ms.Variant == string.Empty)
								obj.tileOffset.X = (int)((float)World.TileSize * du.openStatus);
						}
					}
					else
					{
						obj.characterCanEnter = false;
					}
				}
			}
		}
	}
	class DoorUse : IUseable
	{
		MountedObject r;
		public DoorUse(MountedObject mountedObject)
		{
			r = mountedObject;
		}
		public bool CanUse(Character c)
		{
			if (c.tile == r.tile) return true;
			foreach(Tile t in r.tile.GetNeighbourTiles())
			{
				if (c.tile == t)
					return true;
			}
			return false;
		}
		public float openStatus = 0;

		public void Use(Character c)
		{
			openStatus += 1 / 100f;
			if (r.sprite is MultiSprite ms)
			{
				if(ms.Variant == "NS")
					r.tileOffset.Y = (int)((float)World.TileSize * openStatus);
				if(ms.Variant == "EW")
					r.tileOffset.X = (int)((float)World.TileSize * openStatus);
				if(ms.Variant == string.Empty)
					r.tileOffset.X = (int)((float)World.TileSize * openStatus);
			}
			if (openStatus >= 1f)
				r.characterCanEnter = true;
		}
	}
}
