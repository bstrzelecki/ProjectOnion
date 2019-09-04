using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	class TagCompound
	{
		Dictionary<string, string> tags = new Dictionary<string, string>();
		Vector2 pos;
		public TagCompound(Vector2 pos)
		{
			this.pos = pos;
		}
		public void SaveValue<T>(string name, T i)
		{
			tags.Add(name, i.ToString());
		}
		public T LoadValue<T>(string name)
		{
			return (T)Convert.ChangeType(tags[name], typeof(T));
		}
		public Dictionary<string, string> GetSerializableData()
		{
			return tags;
		}
	}
}
