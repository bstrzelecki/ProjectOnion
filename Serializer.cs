using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectOnion
{
	class Serializer
	{
		Tile[,] map;
		public  Serializer(Tile[,] map)
		{
			this.map = map;
		}
		public void Save()
		{
			XDocument doc = new XDocument();
			doc.Add(new XElement("Map"));
			XElement root = doc.Root;
			foreach(Tile tile in map)
			{
				XElement t = new XElement("tile");
				t.Add(new XElement("isFloor", tile.IsFloor));
				t.Add(new XElement("objName", tile.mountedObject?.registryName??""));
				t.Add(new XElement("floorName", tile.sprite.ToString()));
				root.Add(t);
			}
			doc.Save("map.xml");
		}
		//public Tile[,] Load()
		//{

		//}
	}
}
