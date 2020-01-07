using System.Xml.Linq;

namespace ProjectOnion
{
	class WorldMapSerializer : ISerializable
	{
		public string GetHeader()
		{
			return "world";
		}

		public void Load(XElement data)
		{
			Tile[,] map = new Tile[Settings.mapSizeX, Settings.mapSizeY];

			var tiles = data.Elements("tile");

			foreach (var tile in tiles)
			{
				Tile t = new Tile(int.Parse(tile.Attribute("X").Value), int.Parse(tile.Attribute("Y").Value))
				{
					IsFloor = bool.Parse(tile.Element("isFloor").Value),
					sprite = new MBBSlib.MonoGame.Sprite(tile.Element("floorName").Value)
				};
				if (tile.Element("objName").Value != string.Empty)
					t.PlaceObject(Registry.furnitures[tile.Element("objName").Value].GetFurniture());

				if (tile.Element("resources") != null && tile.Element("resources").Element("id") != null)
				{
					ItemStack stack = new ItemStack(tile.Element("resources").Element("id").Value, int.Parse(tile.Element("resources").Element("amount").Value));

					t.PutItemStack(stack);
				}

				map[t.X, t.Y] = t;
			}
			MainScene.world.SetupMap(map);
		}

		public XElement Save()
		{
			Tile[,] map = MainScene.world.map;
			XElement world = new XElement("world");
			foreach (Tile tile in map)
			{
				XElement t = new XElement("tile");
				t.SetAttributeValue("X", tile.X);
				t.SetAttributeValue("Y", tile.Y);
				t.Add(new XElement("isFloor", tile.IsFloor));
				t.Add(new XElement("objName", tile.mountedObject?.registryName ?? ""));
				t.Add(new XElement("floorName", tile.sprite.ToString()));
				t.Add(new XElement("character", tile.character?.id.ToString() ?? ""));

				XElement r = new XElement("resources");
				if (tile.stackItem != null)
				{
					r.Add(new XElement("id", tile.stackItem.GetResource()));
					r.Add(new XElement("amount", tile.stackItem.GetAmount()));
				}
				t.Add(r);

				world.Add(t);
			}
			return world;
		}
	}
}
