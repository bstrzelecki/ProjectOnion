using System.Collections.Generic;
using System.Linq;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	class ZoneManager
	{
		public static List<Zone> zones = new List<Zone>();

		static ZoneManager()
		{
			GameMain.RegisterRenderer(new ZoneRenderer());
		}
		public static List<Zone> GetZones(string tag = "STOCKPILE")
		{
			return zones;
		}
		public static bool IsZoneOnTile(Tile tile)
		{
			return (from n in zones where n.GetTile() == tile select n).Count() > 0;
		}
		public static void AddZone(Tile tile)
		{
			zones.Add(new Zone(tile));
		}
		public static void RemoveZone(Tile tile)
		{
			zones.RemoveAll(x => x.GetTile() == tile);
		}

		private class ZoneRenderer : MBBSlib.MonoGame.IDrawable
		{
			public void Draw(SpriteBatch sprite)
			{
				foreach (Zone zone in zones)
				{
					sprite.Draw(new Sprite("WhitePixel"), new TileRectangle(zone.GetTile().Position), Color.Pink * .5f);
				}
			}
		}
	}

}
