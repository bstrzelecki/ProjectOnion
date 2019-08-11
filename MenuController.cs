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
	class MenuController : MBBSlib.MonoGame.IDrawable
	{
		public bool IsMenuOpened { get; protected set; }
		public static bool IsMouseOverUI = false;
		public MenuController()
		{

		}

		public void Draw(SpriteBatch sprite)
		{
			
		}
	}
	class Button : MBBSlib.MonoGame.IDrawable, MBBSlib.MonoGame.IUpdateable
	{
		string displayText = "missing_text";
		public event Action OnClicked;
		public event Action OnHover;
		public Microsoft.Xna.Framework.Rectangle size;
		public Color color = Color.White;
		MultiSprite image;
		Microsoft.Xna.Framework.Vector2 position;
		public Button()
		{
			image = new MultiSprite("button");
			size = image.Texture.Bounds;
		}
		public Button(string text, Vector2 pos, MultiSprite sprite = null)
		{
			displayText = text;
			if (sprite == null)
				image = new MultiSprite("button");
			else
				image = sprite;
			position = pos;
		}
		public Button(string text, Vector2 pos,Color color, MultiSprite sprite = null)
		{
			displayText = text;
			if (sprite == null)
				image = new MultiSprite("button");
			else
				image = sprite;
			position = pos;
			this.color = color;
		}
		public bool IsMouseOverUI
		{
			get { return MenuController.IsMouseOverUI; }
			set { MenuController.IsMouseOverUI = value; }
		}
		public void Draw(SpriteBatch sprite)
		{
			sprite.Draw(image, position, Microsoft.Xna.Framework.Color.White);
			sprite.DrawString(GameMain.fonts["font"], displayText, position + new Microsoft.Xna.Framework.Vector2(0.25f * size.Height, 0.1f * size.Width), color);
		}

		public void Update()
		{
			if (size.Contains(Input.GetMousePosition()))
			{
				IsMouseOverUI = true;
				OnHover?.Invoke();
				image.SetTextureVariant("hover");
				if (Input.IsMouseButtonClicked(0))
				{
					OnClicked?.Invoke();
					image.SetTextureVariant("click");
				}
			}
			else
			{
				IsMouseOverUI = false;
				image.SetTextureVariant("");
			}
		}
	}
}
