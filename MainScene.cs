using MBBSlib;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectOnion
{
	internal class MainScene : IStartingPoint, MBBSlib.MonoGame.IDrawable, MBBSlib.MonoGame.IUpdateable
	{
		public static GameMain Game { get; set; }
		public static World world;
		public void Start(GameMain game)
		{
			Game = game;
			GameMain.graphics.PreferredBackBufferWidth = 1280;
			GameMain.graphics.PreferredBackBufferHeight = 720;
			GameMain.graphics.ApplyChanges();
			Game.BackgroundColor = Color.Black;
			GameMain.RegisterRenderer(this);
			GameMain.RegisterUpdate(this);
			world = new World(16, 16);
			new BuildController();
			new Character();

		}
		public void Update()
		{
			MouseController.HandleDeltaMouse();
			MouseController.HandleDeltaScroll();
			if (Input.IsKeyClicked(Keys.C))
			{
				Debugger.OpenConsole();
			}
			Debugger.ExecuteCommands();
			QueueBuilder.Update();

			if (Input.IsKeyClicked(Keys.D0))
				Architect.SetBuildObject(BuildMode.Line, BuildType.Furniture, new Wall());
			if (Input.IsKeyClicked(Keys.D1))
				Architect.SetBuildObject(BuildMode.Area, BuildType.Floor);
		}
		public void Draw(SpriteBatch sprite)
		{

		}
	}
}

