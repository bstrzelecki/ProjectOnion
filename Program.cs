using MBBSlib;
using MBBSlib.MonoGame;
using System;

namespace ProjectOnion
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new GameMain(new MainScene()))
                game.Run();
        }
    }
}
