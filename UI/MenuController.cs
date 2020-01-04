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
		public static bool IsMouseOverUI
		{
			get
			{
				foreach (Button btn in buttons)
				{
					if (btn.IsMouseOverUI) return true;
				}
				foreach (Button btn in sideMenu)
				{
					if (btn.IsMouseOverUI) return true;
				}
				return false;
			}
		}
		readonly static List<Button> buttons = new List<Button>();
		readonly static List<Button> sideMenu = new List<Button>();
		public MenuController()
		{
			GameMain.RegisterRenderer(this, 10);
			GameMain.RegisterUpdate(this);
			Vector2 screenCenter = new Vector2(GameMain.graphics.PreferredBackBufferWidth / 2 - 64, GameMain.graphics.PreferredBackBufferHeight / 2 - 120);
			int i = 0;
			Button resume = new Button("Resume", screenCenter + new Vector2(0, 32 * i));
			resume.OnClicked += () =>
			{
				sideMenu.Clear();
				IsMenuOpened = false;
			};
			buttons.Add(resume); i++;
			Button save = new Button("Save", screenCenter + new Vector2(0, 32 * i));
			save.OnClicked += () =>
			{
				sideMenu.Clear();
				sideMenu.Add(new TextBox("", screenCenter + new Vector2(128, 0)));
				if (GameSerializer.lastName != string.Empty)
				{
					string s = GameSerializer.lastName;
					Button btn = new Button(s, screenCenter + new Vector2(128, 32));
					btn.OnClicked += () =>
					{
						GameSerializer ss = new GameSerializer(s);
						ss.Save();
					};
					sideMenu.Add(btn);
				}
			};
			buttons.Add(save); i++;
			Button load = new Button("Load", screenCenter + new Vector2(0, 32 * i));
			load.OnClicked += () =>
			{
				sideMenu.Clear();
				int j = 0;
				foreach (string s in GameSerializer.GetSaveStrings())
				{
					Button btn = new Button(s, screenCenter + new Vector2(128, 32 * j)); j++;
					btn.OnClicked += () =>
					{
						GameSerializer ss = new GameSerializer(s);
						//Legacy
						ss.Load();
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
				sideMenu.Clear();
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
	class TextBox : Button
	{
		public TextBox(string text, Vector2 pos, MultiSprite sprite = null) : base(text, pos, sprite)
		{
			GameMain.lastCopy.Window.TextInput += Window_TextInput;
			OnClicked += TextBox_OnClicked;
			displayText = string.Empty;
		}

		private void TextBox_OnClicked()
		{
			if (displayText == string.Empty) return;
			GameSerializer s = new GameSerializer(displayText);
			s.Save();
		}

		private void Window_TextInput(object sender, TextInputEventArgs e)
		{
			if (e.Character == 9)
				return;
			if (e.Character == 8)
			{
				if (displayText.Length > 0)
				{
					displayText = displayText.Remove(displayText.Length - 1);
				}
				return;
			}
			displayText += e.Character;
		}
		public override void Draw(SpriteBatch sprite)
		{
			sprite.Draw(image, position, Color.White);
			if (size.Location == Point.Zero || size.Size == Point.Zero)
			{
				size.Location = position.ToPoint();
				size.Size = image.Texture.Bounds.Size;
			}
			try
			{
				sprite.DrawString(GameMain.fonts["font"], displayText, position + new Vector2(0.1f * size.Width, 0.25f * size.Height), color);
			}
			catch (Exception e)
			{
				Debug.Write(e.ToString());
			}
		}

	}
	class Button : MBBSlib.MonoGame.IDrawable, MBBSlib.MonoGame.IUpdateable
	{
		protected string displayText = "missing_text";
		public event Action OnClicked;
		public event Action OnHover;
		public Rectangle size;
		public Color color = Color.White;
		protected MultiSprite image;
		protected Vector2 position;
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
			if (image.Texture != null)
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
		public virtual void Draw(SpriteBatch sprite)
		{
			sprite.Draw(image, position, Color.White);
			if (size.Location == Point.Zero || size.Size == Point.Zero)
			{
				size.Location = position.ToPoint();
				size.Size = image.Texture.Bounds.Size;
			}
			if (displayText == null) return;
			sprite.DrawString(GameMain.fonts["font"], displayText, position + new Vector2(0.1f * size.Width, 0.25f * size.Height), color);
		}

		public void Update()
		{
			if (size.Location == Point.Zero || size.Size == Point.Zero)
			{
				size.Location = position.ToPoint();
				if (image.Texture != null)
					size.Size = image.Texture.Bounds.Size;
			}
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
