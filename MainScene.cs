using System;
using MBBSlib;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectOnion
{
	internal class MainScene : IStartingPoint, MBBSlib.MonoGame.IUpdateable
	{
		public static GameMain Game { get; set; }
		public static World world;
		public static Random rng = new Random();
		public void Start(GameMain game)
		{
			Game = game;
			Game.LoadFont("font");
			GameMain.graphics.PreferredBackBufferWidth = 1280;
			GameMain.graphics.PreferredBackBufferHeight = 720;
			GameMain.graphics.ApplyChanges();
			Game.BackgroundColor = Color.Black;
			GameMain.RegisterUpdate(this);
			world = new World(16, 16);
			new BuildController();
			new Character();
			new Character();
			new UIFabricator();
			GameMain.RegisterRenderer(new UIController());

			UIController.AddItem("Misc", new JobItem ("Cancel Job", () => JobManager.SetCancelJobMode()));
			UIController.AddItem("Misc", new JobItem ("Deconstruct", () => JobManager.SetDeconstructJob()));
			UIController.AddItem("Structure", new JobItem ("Floor", () => Architect.SetBuildObject(BuildMode.Area, BuildType.Floor)));


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
				JobManager.SetCancelJobMode();
			if (Input.IsKeyClicked(Keys.D9))
				JobManager.SetDeconstructJob();
			if (Input.IsKeyClicked(Keys.D1))
				Architect.SetBuildObject(BuildMode.Area, BuildType.Floor);
			if (Input.IsKeyClicked(Keys.D2))
				Architect.SetBuildObject(BuildMode.Line, BuildType.Furniture, new Wall());
			if (Input.IsKeyClicked(Keys.D3))
				Architect.SetBuildObject(BuildMode.Single, BuildType.Furniture, new Door());
		}
	}
}

