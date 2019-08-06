using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	class Registry
	{
		public static List<NewFurniture> furnitures = new List<NewFurniture>();
		public static List<Job> jobs = new List<Job>();
		public static void Register(NewFurniture f)
		{
			furnitures.Add(f);
		}
		public static void Register(Job j)
		{
			jobs.Add(j);
		}
		public static void RegisterAll()
		{

		}
	}
}
