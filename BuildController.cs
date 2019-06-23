﻿using System.Collections.Generic;
using MBBSlib;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	internal class BuildController : MBBSlib.MonoGame.IUpdateable, MBBSlib.MonoGame.IDrawable
	{
		private List<Vector2> blips = new List<Vector2>();
		public BuildController()
		{
			GameMain.RegisterUpdate(this);
			GameMain.RegisterRenderer(this);
			Debugger.OnCmd += Debugger_OnCmd;
		}

		private void Debugger_OnCmd(CommandCompund cmd)
		{
			if (cmd.Check("BuildController"))
			{
				if (cmd.Source == "BuildMode")
				{
					buildMode = (BuildMode)cmd.GetInt(0);
				}
			}
		}

		public void Draw(SpriteBatch sprite)
		{
			foreach (Vector2 blip in blips)
			{
				sprite.Draw(new Sprite("blip"), new TileRectangle(blip), Color.White);
			}
		}
		public static BuildMode buildMode = BuildMode.None;
		private Vector2 _start;
		public void Update()
		{
			if (Input.IsMouseKeyDown(0))
			{
				if (_start == Vector2.Zero)
				{
					_start = (Input.GetMousePosition() - Input.cameraOffset) / World.TileSize;
				}
				Vector2 bp = (Input.GetMousePosition() - Input.cameraOffset) / World.TileSize;
				switch (buildMode)
				{
					case BuildMode.Free:
						DrawObject(bp);
						break;
					case BuildMode.Single:
						blips.Clear();
						DrawObject(bp);
						break;
					case BuildMode.Line:
						blips.Clear();
						ShapeUtils.DrawLine(_start, bp, DrawObject);
						break;
					case BuildMode.Area:
						blips.Clear();
						ShapeUtils.DrawArea(_start, bp, DrawObject);
						break;
				}
			}
			if (Input.IsMouseKeyUp(0))
			{
				_start = Vector2.Zero;
				blips.Clear();
			}

		}

		#region DrawThings
		private void DrawObject(Vector2 bp)
		{
			if (bp.Y > MainScene.world.Height || bp.X > MainScene.world.Width)
			{
				return;
			}

			if (bp.Y < 0 || bp.X < 0)
			{
				return;
			}

			blips.Add(bp);
		}
		#endregion
	}
	//FIXME: extract this to another class* *file
	public enum BuildMode
	{
		None,
		Free,
		Single,
		Line,
		Area
	}
}
