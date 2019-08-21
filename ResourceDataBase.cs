using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBBSlib.MonoGame;

namespace ProjectOnion
{
	class ResourceDataBase
	{
		private static Dictionary<string, Resource> resources = new Dictionary<string, Resource>();
		public static void Init()
		{
			resources.Add("METAL", new Resource(new Sprite("blip")));
		}
		public static Resource GetResource(string key)
		{
			return resources[key];
		}
	}
	class Resource
	{
		Sprite sprite;
		public Resource(Sprite sprite)
		{
			this.sprite = sprite;
		}
		public Sprite GetSprite()
		{
			return sprite;
		}
	}
}
