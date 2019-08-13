using System;
using System.Collections.Generic;
using System.Diagnostics;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	class MenuController : MBBSlib.MonoGame.IDrawable, MBBSlib.MonoGame.IUpdateable
	{
		public static bool IsMenuOpened { get; protected set; }
		public static bool IsMouseOverUI { get
			{
				foreach(Button btn in buttons)
				{
					if (btn.IsMouseOverUI) return true;
				}
				return false;
			}
		}
		static List<Button> buttons = new List<Button>();
		static List<Button> sideMenu = new List<Button>();
		public MenuController()
		{
			GameMain.RegisterRenderer(this, 10);
			GameMain.RegisterUpdate(this);
			Vector2 screenCenter = new Vector2(GameMain.graphics.PreferredBackBufferWidth / 2 - 64, GameMain.graphics.PreferredBackBufferHeight / 2 - 120);
			int i = 0;
			Button resume = new Button("Resume", screenCenter + new Vector2(0, 32 * i));
			resume.OnClicked += () =>
			{
				IsMenuOpened = false;
			};
			buttons.Add(resume); i++;
			buttons.Add(new Button("Save", screenCenter + new Vector2(0,32 * i))); i++;
			Button load = new Button("Load", screenCenter + new Vector2(0, 32 * i));
			load.OnClicked += () =>
			{
				sideMenu.Clear();
				int j = 0;
				foreach (string s in Serializer.GetSaveStrings())
				{
					Button btn = new Button(s, screenCenter + new Vector2(128, 32 * j)); j++;
					btn.OnClicked += () =>
					{
						Debug.WriteLine("Loading " + s);
						Serializer ss = new Serializer(s);
						MainScene.world.SetupMap(ss.Load());
					};
					sideMenu.Add(btn);
				}
			};
			buttons.Add(load); i++;
			Button exit = new Button("Exit", screenCenter + new Vector2(0, 32 * i));
			exit.OnClicked += () =>
			{
				GameMain.lastCopy.Exit();
			};
			buttons.Add(exit);
		}

		public void Draw(SpriteBatch sprite)
		{
			if (!IsMenuOpened) return;
			foreach (Button btn in buttons)
			{
				btn.Draw(sprite);
			}
			foreach (Button btn in sideMenu)
			{
				btn.Draw(sprite);
			}
		}

		public void Update()
		{
			if (Input.IsKeyClicked(Microsoft.Xna.Framework.Input.Keys.Escape))
			{
				IsMenuOpened = !IsMenuOpened;
			}
			if (IsMenuOpened) Time.IsPaused = true;
			if (!IsMenuOpened)
			{
				Time.IsPaused = false;
				return;
			}
			foreach (Button btn in buttons)
			{
				btn.Update();
			}
			foreach (Button btn in sideMenu)
			{
				btn.Update();
				
			}
		}
	}
	class Button : MBBSlib.MonoGame.IDrawable, MBBSlib.MonoGame.IUpdateable
	{
		string displayText = "missing_text";
		public event Action OnClicked;
		public event Action OnHover;
		public Rectangle size;
		public Color color = Color.White;
		MultiSprite image;
		Vector2 position;
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
			if(image.Texture != null)
				size = image.Texture.Bounds;
			if (image.Texture != null)
			{
				size = image.Texture.Bounds;
				size.Location = position.ToPoint();
			}
			position = pos;
		}
		public Button(string text, Vector2 pos, Color color, MultiSprite sprite = null)
		{
			displayText = text;
			if (sprite == null)
				image = new MultiSprite("button");
			else
				image = sprite;
			if (image.Texture != null)
			{
				size = image.Texture.Bounds;
				size.Location = position.ToPoint();
			}
			position = pos;
			this.color = color;
		}
		public bool IsMouseOverUI
		{
			get; set;
		}
		public void Draw(SpriteBatch sprite)
		{
			sprite.Draw(image, position, Color.White);
			if (size.Location == Point.Zero || size.Size == Point.Zero)
			{
				size.Location = position.ToPoint();
				size.Size = image.Texture.Bounds.Size;
			}
			sprite.DrawString(GameMain.fonts["font"], displayText, position + new Vector2(0.1f * size.Width,0.25f * size.Height), color);
		}

		public void Update()
		{
			if (size.Location == Point.Zero || size.Size == Point.Zero)
			{
				size.Location = position.ToPoint();
				if(image.Texture != null)
					size.Size = image.Texture.Bounds.Size;
			}
			if (size.Contains(Input.GetMousePosition()))
			{
				IsMouseOverUI = true;
				OnHover?.Invoke();
				image.SetTextureVariant("hover");
				if (Input.IsMouseButtonClicked(0))
				{
					Debug.WriteLine("sdaasd");
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
