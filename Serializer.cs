using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	class Serializer
	{
		Tile[,] map;
		public  Serializer(Tile[,] map)
		{
			this.map = map;
		}
		public void Save(string fileName)
		{
			XDocument doc = new XDocument();
			doc.Add(new XElement("Map"));
			XElement root = doc.Root;

			XElement settings = new XElement("settings");
			settings.Add(new XElement("mapSize", new XElement("X", MainScene.world.Width), new XElement("Y", MainScene.world.Height)));

			root.Add(settings);
			foreach(Tile tile in map)
			{
				XElement t = new XElement("tile");
				t.SetAttributeValue("X", tile.X);
				t.SetAttributeValue("Y", tile.Y);
				t.Add(new XElement("isFloor", tile.IsFloor));
				t.Add(new XElement("objName", tile.mountedObject?.registryName??""));
				t.Add(new XElement("floorName", tile.sprite.ToString()));
				t.Add(new XElement("character", tile.character?.id.ToString()??""));
				XElement j = new XElement("jobs");
				int i = 0;
				foreach(Job job in tile.job)
				{
					if (job == null) { i++; continue; }
					XElement jb = new XElement("job");
					jb.SetAttributeValue("layer", i);
					jb.Add(new XElement("events", job.jobEvents.GetType().Name));

					XElement args = new XElement("eventArgs");
					foreach(string s in job.jobEvents.GetSerializationData())
					{
						args.Add("a", s);
					}

					jb.Add(new XElement("onTile", job.IsOnTile.ToString()));
					jb.Add(new XElement("workTime", job.workTime));
					j.Add(jb);
				}
				t.Add(j);
				root.Add(t);
			}
			doc.Save(fileName+"_map.xml");
		}
		public Tile[,] Load(string fileName)
		{
			Assembly a = Assembly.GetCallingAssembly();
			XDocument doc = XDocument.Load(fileName + "_map.xml");
			XElement root = doc.Root;

			int x = int.Parse(root.Element("settings").Element("mapSize").Element("X").Value);
			int y = int.Parse(root.Element("settings").Element("mapSize").Element("Y").Value);

			Tile[,] map = new Tile[x, y];
			var tiles = root.Elements("tile");
			foreach(XElement tile in tiles)
			{
				Tile t = new Tile(int.Parse(tile.Attribute("X").Value), int.Parse(tile.Attribute("Y").Value));
				t.IsFloor = bool.Parse(tile.Element("isFloor").Value);
				t.sprite = new MBBSlib.MonoGame.Sprite(tile.Element("floorName").Value);
				t.PlaceObject(Registry.furnitures[tile.Element("objName").Value].GetFurniture());

				XElement jobs = tile.Element("jobs");
				foreach(XElement job in jobs.Elements("job"))
				{
					string[] s = (from n in job.Element("eventArgs").Elements("a") select n.Value).ToArray();

					object[] eventArgs = new object[1 + s.Length];
					eventArgs[0] = t;
					for(int i = 1; i < 1 + s.Length; i++)
					{
						eventArgs[i] = s[i - 1];
					}
					Job j = new Job(t, (IJobEvents)a.CreateInstance(job.Element("events").Value, false, BindingFlags.Default, null, eventArgs,null,null), bool.Parse(tile.Element("onTile").Value), int.Parse(tile.Element("workTime").Value), int.Parse(job.Attribute("layer").Value));

					t.job[int.Parse(job.Attribute("layer").Value)] = j;
				}
				map[t.X, t.Y] = t;
				//TODO: Add character load
				//t.isCharOnTile = true;
			}
			return map;
		}
	}
}
