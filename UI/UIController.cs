using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	class UIController : MBBSlib.MonoGame.IDrawable, MBBSlib.MonoGame.IUpdateable
	{
		static Dictionary<string, List<IUIItem>> Items = new Dictionary<string, List<IUIItem>>();
		static List<Rectangle> buttons = new List<Rectangle>();
		public static int OffsetX, OffsetY; 
		public static void AddItem(string category, IUIItem item)
		{
			if (!Items.ContainsKey(category))
				Items.Add(category, new List<IUIItem>());
			Items[category].Add(item);
			buttons.Clear();
			categories.Clear();
			int i = 0;
			foreach (string c in Items.Keys)
			{
				categories.Add(i, c);
				buttons.Add(new Rectangle(0, 32 * i, 128, 32));
				i++;
			}
		}
		static Dictionary<int, string> categories = new Dictionary<int, string>();
		public void Draw(SpriteBatch sprite)
		{
			int i = 0;
			foreach(string c in Items.Keys)
			{
				sprite.Draw(new Sprite("button"), new Vector2(0, 32 * i), Color.White);
				sprite.DrawString(GameMain.fonts["font"], c, new Vector2(16, 8 + 32 * i), Color.White);
				i++;
			}
		}

		public void Update()
		{
			int i = 0;
			foreach (Rectangle rect in buttons) {
				if (Input.IsMouseButtonClicked(0) && rect.Contains(Input.GetMousePosition()))
				{
					Debug.WriteLine("Clicked" + Items[categories[i]]);
				}
				i++;
			}
		}
	}
}
