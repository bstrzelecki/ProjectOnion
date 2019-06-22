using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MBBSlib;

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
                if(cmd.Source == "BuildMode")
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
                        DrawLine(_start, bp);
                        break;
                    case BuildMode.Area:
                        blips.Clear();
                        DrawArea(_start, bp);
                        break;
                }
            }
            if (Input.IsMouseKeyUp(0))
            {
                _start = Vector2.Zero;
                blips.Clear();
            }

        }

        //FIXME: extract this to another class
        #region DrawThings
        private void DrawArea(Vector2 start, Vector2 end)
        {
            if (end.X < start.X)
            {
                int b = (int)end.X;
                end.X = start.X;
                start.X = b;
            }
            if (end.Y < start.Y)
            {
                int b = (int)end.Y;
                end.Y = start.Y;
                start.Y = b;
            }
            for (int x = (int)start.X; x <= end.X; x++)
            {
                for (int y = (int)start.Y; y <= end.Y; y++)
                {
                    DrawObject(new Vector2(x, y));
                }
            }
        }
        private void DrawLine(Vector2 start, Vector2 end)
        {
            Vector2 origin = start;
            if (end.X < start.X)
            {
                int b = (int)end.X;
                end.X = start.X;
                start.X = b;
            }
            if (end.Y < start.Y)
            {
                int b = (int)end.Y;
                end.Y = start.Y;
                start.Y = b;
            }
            if (end.Y - start.Y > end.X - start.X)
            {
                for (int y = (int)start.Y; y <= end.Y; y++)
                {
                    DrawObject(new Vector2(origin.X, y));
                }
            }
            else
            {
                for (int x = (int)start.X; x <= end.X; x++)
                {
                    DrawObject(new Vector2(x, origin.Y));
                }
            }
        }

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
