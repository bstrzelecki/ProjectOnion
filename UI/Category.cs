using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	class Category : Attribute
	{
		public string category;
		public Category(string category)
		{
			this.category = category;
		}
	}
}
