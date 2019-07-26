using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	class UIFabricator
	{
		public UIFabricator()
		{
			Assembly assembly = Assembly.GetCallingAssembly();
			foreach(Type type in from tp in assembly.GetTypes() where tp.IsSubclassOf(typeof(NewFurniture)) select tp)
			{
				Attribute[] attributes = Attribute.GetCustomAttributes(type);
				foreach(Attribute a in attributes)
				{
					if(a is Category c)
					{
						NewFurniture nf = (NewFurniture)Activator.CreateInstance(type);
						UIController.AddItem(c.category, nf.GetUIItem());
					}
				}
			}
		}
	}
}
