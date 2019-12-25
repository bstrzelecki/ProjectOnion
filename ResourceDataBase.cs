using System.Collections.Generic;
using MBBSlib.MonoGame;

namespace ProjectOnion
{
	class ResourceDataBase
	{
		private static Dictionary<string, Resource> resources = new Dictionary<string, Resource>();
		public static void Init()
		{
			resources.Add("METAL", new Resource(new Sprite("metal")));
			resources.Add("CONCRETE", new Resource(new Sprite("concrete")));
		}
		public static Resource GetResource(string key)
		{
			if (resources.ContainsKey(key))
			{
				return resources[key];
			}
			else
			{
				return Resource.Empty;
			}
		}
	}
	class Resource
	{
		public static Resource Empty
		{
			get
			{
				return new Resource(new Sprite("WhitePixel"));
			}
		}
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
