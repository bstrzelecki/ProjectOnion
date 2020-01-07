using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	class CharacterSerializer : ISerializable
	{
		public string GetHeader()
		{
			return "chars";
		}

		public void Load(XElement data)
		{
			foreach (Character c in Registry.characters)
			{
				c.Dispose();
			}
			Registry.characters.Clear();
			foreach (XElement c in data.Elements("character"))
			{
				Character cc = new Character();
				string p = c.Element("position").Element("tile").Value;
				Vector2 pos;
				pos.X = int.Parse(p.Split(',')[0]);
				pos.Y = int.Parse(p.Split(',')[1]);
				cc.tile = MainScene.world.GetTile(pos);
				if (c.Element("resources") != null && c.Element("resources").Element("id") != null)
				{
					ItemStack stack = new ItemStack(c.Element("resources").Element("id").Value, int.Parse(c.Element("resources").Element("amount").Value));

					cc.carryItem = stack;
				}
				if (c.Element("position").Element("dest") != null)
				{
					string dp = c.Element("position").Element("dest").Value;
					pos.X = int.Parse(p.Split(',')[0]);
					pos.Y = int.Parse(p.Split(',')[1]);
					cc.dest = MainScene.world.GetTile(pos);
					cc.moveCompleted = float.Parse(c.Element("position").Element("progress").Value);
				}
				cc.id = int.Parse(c.Element("id").Value);
				cc.Name = c.Element("name").Value;
			}
		}

		public XElement Save()
		{
			XElement world = new XElement("chars");
			foreach (Character c in Registry.characters)
			{
				XElement cc = new XElement("character");
				XElement pos = new XElement("position");
				pos.Add(new XElement("tile", $"{c.tile.X},{c.tile.Y}"));
				if (c.dest != null)
					pos.Add(new XElement("dest", $"{c.dest.X},{c.dest.Y}"));
				pos.Add(new XElement("progress", c.moveCompleted));
				cc.Add(pos);
				cc.Add(new XElement("id", c.id));
				cc.Add(new XElement("name", c.Name));

				XElement r = new XElement("resources");
				if (c.carryItem != null)
				{
					r.Add(new XElement("id", c.carryItem.GetResource()));
					r.Add(new XElement("amount", c.carryItem.GetAmount()));
				}
				cc.Add(r);

				world.Add(cc);
			}
			return world;
		}
	}
}
