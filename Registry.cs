using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	class Registry
	{
		public static List<NewFurniture> furnitures = new List<NewFurniture>();
		public static Dictionary<string, Action> jobActions = new Dictionary<string, Action>();
		public static void Register(NewFurniture f)
		{
			furnitures.Add(f);
		}
		public static void Register(string s, Action j)
		{
			jobActions.Add(s, j);
		}
		public static void RegisterAll()
		{
			Assembly assembly = Assembly.GetCallingAssembly();
			foreach (Type type in from tp in assembly.GetTypes() where tp.IsSubclassOf(typeof(NewFurniture)) select tp)
			{
				NewFurniture nf = (NewFurniture)Activator.CreateInstance(type);
				furnitures.Add(nf);
				jobActions.Add(nf.GetFurniture().registryName + "_job", nf.GetOnClickAction());
				UIController.AddItem(nf.GetFurniture().category, nf.GetUIItem());
			}
		}
	}
}
