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
			obj.sprite = new MultiSprite("wall");
			obj.moveCost = float.MaxValue;
			obj.OnNeighbourChanged += (Tile tile) =>
			{
				UpdateSprite();
			};
			obj.OnPlaced += () => UpdateSprite();
		}

		private void UpdateSprite()
		{
			if (obj.tile == null)
			{
				obj.tile = MainScene.world.GetTile(obj.position);
			}
			foreach (Tile tile in obj.tile.GetNeighbourTiles())
			{
				if (tile == null) return;
				if (obj.sprite is MultiSprite ms)
				{
					Vector2 delta = tile.Position - obj.tile.Position;
					ms.SetTextureVariant(string.Empty);

					if (delta == new Vector2(0, -1))
						ms.SetTextureVariant(ms.Variant + "N");
					if (delta == new Vector2(1, 0))
						ms.SetTextureVariant(ms.Variant + "E");
					if (delta == new Vector2(0, 1))
						ms.SetTextureVariant(ms.Variant + "S");
					if (delta == new Vector2(-1, 0))
						ms.SetTextureVariant(ms.Variant + "W");
				}
			}
		}
	}
}
