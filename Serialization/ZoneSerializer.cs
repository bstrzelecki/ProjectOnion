using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectOnion
{
	class ZoneSerializer : ISerializable
	{
		public string GetHeader()
		{
			return "zones";
		}

		public void Load(XElement data)
		{
			foreach(XElement zone in data.Elements("Zone"))
			{
				ZoneManager.AddZone(MainScene.world.GetTile(int.Parse(zone.Element("X").Value), int.Parse(zone.Element("Y").Value)));
			}
		}

		public XElement Save()
		{
			XElement z = new XElement("zones");
			foreach(Zone zone in ZoneManager.zones)
			{
				z.Add(new XElement("Zone", new XElement("X", zone.GetTile().X), new XElement("Y", zone.GetTile().Y)));
			}
			return z;
		}
	}
}
