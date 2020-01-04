using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectOnion
{
	class SettingsSerializer : ISerializable
	{
		public string GetHeader()
		{
			return "settings";
		}

		public void Load(XElement data)
		{
			Settings.mapSizeX = int.Parse(data.Element("mapSize").Element("X").Value);
			Settings.mapSizeY = int.Parse(data.Element("mapSize").Element("Y").Value);
		}

		public XElement Save()
		{
			XElement e = new XElement("settings");
			e.Add(new XElement("mapSize", new XElement("X", MainScene.world.Width), new XElement("Y", MainScene.world.Height)));
			return e;
		}
	}
}
