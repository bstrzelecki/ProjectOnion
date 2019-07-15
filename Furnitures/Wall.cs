using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	class Wall : NewFurniture
	{
		protected override void SetProperties()
		{
			obj.displayName = "Wall";
			obj.registryName = "wall";
			obj.sprite = new MultiSprite("wall");
			obj.moveCost = float.MaxValue;
			obj.objectEvents = new WallEvent(obj);
			obj.flags.Add("wall");
		}
	}

	class WallEvent : IObjectEvents
	{
		MountedObject obj;
		public WallEvent(MountedObject r)
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
				ms.SetTextureVariant((n?"N":string.Empty)+ (e ? "E" : string.Empty) + (s ? "S" : string.Empty) + (w ? "W" : string.Empty));
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
	}
}
