using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	class UIController : MBBSlib.MonoGame.IDrawable
	{
		static Dictionary<string, List<IUIItem>> Items = new Dictionary<string, List<IUIItem>>();
		public static int OffsetX, OffsetY; 
		public static void AddItem(string category, IUIItem item)
		{
			if (!Items.ContainsKey(category))
				Items.Add(category, new List<IUIItem>());
			Items[category].Add(item);
		}

		public void Draw(SpriteBatch sprite)
		{
			int i = 0;
			foreach(string c in Items.Keys)
			{
				sprite.Draw(new Sprite("button"), new Vector2(0, 32 * i), Color.White);
				i++;
			}
		}
	}
}
