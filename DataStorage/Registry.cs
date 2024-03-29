﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProjectOnion
{
	class Registry
	{
		public static Dictionary<string, NewFurniture> furnitures = new Dictionary<string, NewFurniture>();
		public static Dictionary<string, Action> jobActions = new Dictionary<string, Action>();
		public static List<Character> characters = new List<Character>();
		public static void Register(NewFurniture f)
		{
			string name = f.GetFurniture().registryName;
			furnitures.Add(name, f);
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
				Register(nf);
				jobActions.Add(nf.GetFurniture().registryName + "_job", nf.GetOnClickAction());
				UIController.AddItem(nf.GetFurniture().category, nf.GetUIItem());
			}
		}
	}
}
