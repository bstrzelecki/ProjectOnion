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
        Sprite pixel;
        public void Start(GameMain game)
        {
            Game = game;
            Game.BackgroundColor = Color.Black;
            pixel = new Sprite("WhitePixel");
            GameMain.RegisterRenderer(this);
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(pixel, new Rectangle(30, 30, 500, 500), Color.Red);
        }
    }
}

