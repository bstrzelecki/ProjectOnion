using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	class Serializer
	{
		string fileName;
		public static string lastName;
		public Serializer(string fn)
		{
			fileName = fn;
			lastName = fn;
		}
		public void Save(Tile[,] map)
		{
			XDocument doc = new XDocument();
			doc.Add(new XElement("Map"));
			XElement root = doc.Root;

			XElement settings = new XElement("settings");
			settings.Add(new XElement("mapSize", new XElement("X", MainScene.world.Width), new XElement("Y", MainScene.world.Height)));
			root.Add(settings);
			foreach (Tile tile in map)
			{
				XElement t = new XElement("tile");
				t.SetAttributeValue("X", tile.X);
				t.SetAttributeValue("Y", tile.Y);
				t.Add(new XElement("isFloor", tile.IsFloor));
				t.Add(new XElement("objName", tile.mountedObject?.registryName ?? ""));
				t.Add(new XElement("floorName", tile.sprite.ToString()));
				t.Add(new XElement("character", tile.character?.id.ToString() ?? ""));
				XElement j = new XElement("jobs");
				for(int i = 0; i < tile.job.Length;i++)
				{
					Job job = tile.job[i];
					if (job == null || job.IsCompleted) { continue; }
					XElement jb = new XElement("job");
					jb.SetAttributeValue("layer", (int)job.jobLayer);
					jb.Add(new XElement("events", job.jobEvents.GetType().Name));

					XElement args = new XElement("eventArgs");
					foreach (string s in job.jobEvents.GetSerializationData())
					{
						args.Add(new XElement("a", s));
					}
					jb.Add(args);
					jb.Add(new XElement("onTile", job.onTile.ToString()));
					jb.Add(new XElement("workTime", job.workTime));
					j.Add(jb);
				}
				t.Add(j);
				root.Add(t);
			}
			try
			{
				doc.Save($"{Environment.CurrentDirectory}/Saves/{fileName}_map.xml");
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
				Directory.CreateDirectory($"{Environment.CurrentDirectory}/Saves/");
				doc.Save($"{Environment.CurrentDirectory}/Saves/{fileName}_map.xml");
			}
			SaveChars();
		}
		public Tile[,] Load()
		{
			if (!File.Exists($"{Environment.CurrentDirectory}/Saves/{fileName}_map.xml"))
			{
				return null;
			}
			Assembly a = Assembly.GetCallingAssembly();
			XDocument doc = XDocument.Load($"{Environment.CurrentDirectory}/Saves/{fileName}_map.xml");
			XElement root = doc.Root;

			int x = int.Parse(root.Element("settings").Element("mapSize").Element("X").Value);
			int y = int.Parse(root.Element("settings").Element("mapSize").Element("Y").Value);

			Tile[,] map = new Tile[x, y];
			var tiles = root.Elements("tile");
			foreach (XElement tile in tiles)
			{
				Tile t = new Tile(int.Parse(tile.Attribute("X").Value), int.Parse(tile.Attribute("Y").Value));
				t.IsFloor = bool.Parse(tile.Element("isFloor").Value);
				t.sprite = new MBBSlib.MonoGame.Sprite(tile.Element("floorName").Value);
				if (tile.Element("objName").Value != string.Empty)
					t.PlaceObject(Registry.furnitures[tile.Element("objName").Value].GetFurniture());

				XElement jobs = tile.Element("jobs");
				foreach (XElement job in jobs.Elements("job"))
				{
					string[] s = (from n in job.Element("eventArgs").Elements("a") select n.Value).ToArray();

					object[] eventArgs = new object[1 + s.Length];
					eventArgs[0] = t;
					for (int i = 1; i < 1 + s.Length; i++)
					{
						eventArgs[i] = s[i - 1];
					}
					string typeName = job.Element("events").Value;
					Type type = a.GetType("ProjectOnion." + typeName);
					IJobEvents events = (IJobEvents)Activator.CreateInstance(type, eventArgs);
					//TODO fix layering problem
					bool onTile = bool.Parse(job.Element("onTile").Value);
					int workTime = int.Parse(job.Element("workTime").Value);
					JobLayer jb = (JobLayer)int.Parse(job.Attribute("layer").Value);
					Job j = new Job(t, events, onTile, workTime, jb);
					j.Register();
					JobQueue.AddJob(j);
				}
				map[t.X, t.Y] = t;
			}
			try
			{
				LoadChars();
			}
			catch { }
			return map;
		}

		private void SaveChars()
		{
			XDocument doc = new XDocument();
			doc.Add(new XElement("root"));
			XElement root = doc.Root;

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
				root.Add(cc);
			}
			doc.Save(Environment.CurrentDirectory + @"\Saves\" + fileName + "_char.xml");
		}
		private void LoadChars()
		{
			foreach (Character c in Registry.characters)
			{
				c.Dispose();
			}
			Registry.characters.Clear();
			XDocument doc = XDocument.Load(Environment.CurrentDirectory + @"\Saves\" + fileName + "_char.xml");
			foreach (XElement c in doc.Root.Elements("character"))
			{
				Character cc = new Character();
				string p = c.Element("position").Element("tile").Value;
				Vector2 pos;
				pos.X = int.Parse(p.Split(',')[0]);
				pos.Y = int.Parse(p.Split(',')[1]);
				cc.tile = MainScene.world.GetTile(pos);
				if (c.Element("position").Element("dest") != null)
				{
					string dp = c.Element("position").Element("dest").Value;
					pos.X = int.Parse(p.Split(',')[0]);
					pos.Y = int.Parse(p.Split(',')[1]);
					cc.dest = MainScene.world.GetTile(pos);
					cc.moveCompleted = int.Parse(c.Element("position").Element("progress").Value);
				}
				cc.id = int.Parse(c.Element("id").Value);
				cc.Name = c.Element("name").Value;
			}
		}
		public void SaveTags(Tile[,] map)
		{
			XDocument doc = new XDocument();
			doc.Add(new XElement("root"));

			XElement root = doc.Root;
			foreach (Tile tile in map)
			{
				TagCompound tag = new TagCompound(new Vector2(tile.X, tile.Y));
				tile.OnTagSave(tag);
				XElement t = new XElement("tag");
				t.SetAttributeValue("X", tile.X);
				t.SetAttributeValue("Y", tile.Y);
				foreach (string s in tag.GetSerializableData().Keys)
				{
					t.Add(new XElement(s, tag.GetSerializableData()[s]));
				}
				root.Add(t);
			}
		}

		public static string[] GetSaveStrings()
		{
			string path = Environment.CurrentDirectory + @"\Saves\";
			string[] files = Directory.GetFiles(path);
			string[] saves = (from n in files where n.Contains("_map.xml") select n.Substring(path.Length, n.IndexOf('_') - path.Length)).ToArray();
			return saves;
		}
	}
}
