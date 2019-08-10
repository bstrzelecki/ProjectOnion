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
		public static bool IsMouseOverUI { get; private set; } = false;
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
			if(displayedCategory != string.Empty)
			{
				i = 0;
				foreach(IUIItem c in Items[displayedCategory])
				{
					string text = c.GetDisplayName();
					sprite.Draw(new Sprite("button"), new Vector2(128, 32 * (i + displayedOffset)), Color.White);
					sprite.DrawString(GameMain.fonts["font"], text, new Vector2(144, 8 + 32 * (i + displayedOffset)), Color.White);
					i++;
				}
			}
		}
		string displayedCategory = string.Empty;
		int displayedOffset = 0;
		List<Rectangle> itemButtons = new List<Rectangle>();
		public void Update()
		{
			IsMouseOverUI = false;
			foreach(Rectangle rect in buttons)
			{
				if (rect.Contains(Input.GetMousePosition()))
					IsMouseOverUI = true;
			}
			foreach(Rectangle rect in itemButtons)
			{
				if (rect.Contains(Input.GetMousePosition()))
					IsMouseOverUI = true;
			}
			if (!IsMouseOverUI) return;
			if (Input.IsMouseButtonClicked(0))
			{
				int i = 0;
				foreach (Rectangle rect in buttons)
				{
					if (rect.Contains(Input.GetMousePosition()))
					{
						Debug.WriteLine("Clicked " + categories[i]);
						if(displayedCategory == categories[i])
						{
							displayedCategory = string.Empty;
							return;
						}
						displayedCategory = categories[i];
						displayedOffset = i;
						itemButtons.Clear();
						int j = 0;
						foreach (IUIItem c in Items[displayedCategory])
						{
							itemButtons.Add(new Rectangle(128, 32 * (i + j), 128, 32));
							j++;
						}
					}
					
					i++;
				}
				if (displayedCategory != string.Empty)
				{
					i = 0;
					foreach(Rectangle rect in itemButtons)
					{
						if (rect.Contains(Input.GetMousePosition()))
						{
							Debug.WriteLine("Clicked " + Items[displayedCategory][i].GetDisplayName());
							Items[displayedCategory][i].OnClick();
						}
						i++;
					}
				}
			}
		}
	}
}
