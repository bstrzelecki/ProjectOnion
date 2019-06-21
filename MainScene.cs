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
    class MainScene : IStartingPoint, MBBSlib.MonoGame.IDrawable
    {
        public static GameMain Game { get; set; }
        World world;
        public void Start(GameMain game)
        {
            Game = game;
            Game.graphics.PreferredBackBufferWidth = 1280;
            Game.graphics.PreferredBackBufferHeight = 720;
            Game.graphics.ApplyChanges();
            Game.BackgroundColor = Color.Black;
            GameMain.RegisterRenderer(this);
            world = new World(16, 16);
        }

        public void Draw(SpriteBatch sprite)
        {

        }
    }
}

