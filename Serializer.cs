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

				XElement j = new XElement("jobs");
				int i = 0;
				foreach(Job job in tile.job)
				{
					if (job == null) { i++; continue; }
					XElement jb = new XElement("job");
					jb.SetAttributeValue("layer", i);

				}

				root.Add(t);
			}
			doc.Save("map.xml");
		}
		//public Tile[,] Load()
		//{

		//}
	}
}
