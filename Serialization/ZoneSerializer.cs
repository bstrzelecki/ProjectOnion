﻿using System.Xml.Linq;

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
			if (data == null || !data.HasElements) return;
			foreach (XElement zone in data.Elements("Zone"))
			{
				ZoneManager.AddZone(MainScene.world.GetTile(int.Parse(zone.Element("X").Value), int.Parse(zone.Element("Y").Value)));
			}
		}

		public XElement Save()
		{
			XElement z = new XElement("zones");
			foreach (Zone zone in ZoneManager.zones)
			{
				z.Add(new XElement("Zone", new XElement("X", zone.GetTile().X), new XElement("Y", zone.GetTile().Y)));
			}
			return z;
		}
	}
}
