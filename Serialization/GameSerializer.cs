using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	class GameSerializer
	{
		readonly string fileName;
		public static string lastName;
		private readonly List<ISerializable> serializers = new List<ISerializable>();
		public GameSerializer(string fn)
		{
			fileName = fn;
			lastName = fn;

			serializers.AddRange(new ISerializable[]{ new SettingsSerializer(), new WorldMapSerializer(), new CharacterSerializer()});
		}
		public void Save()
		{
			XDocument doc = new XDocument();
			doc.Add(new XElement("root"));
			XElement root = doc.Root;
			foreach (var s in serializers)
			{
				root.Add(s.Save());
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
		}
		public void Load()
		{
			XDocument doc = XDocument.Load($"{Environment.CurrentDirectory}/Saves/{fileName}_map.xml");

			foreach(var s in serializers)
			{
				s.Load(doc.Root.Element(s.GetHeader()));
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
