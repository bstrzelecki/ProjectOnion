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
