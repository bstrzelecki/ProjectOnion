using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	class TagCompound
	{
		Dictionary<string, string> tags = new Dictionary<string, string>();
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
