using System;
using MBBSlib.MonoGame;

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
		private static void Main(string[] args)
		{
			string f = string.Empty;
			if(args.Length > 0) 
			{
				f = args[0];
			}

			using (var game = new GameMain(new MainScene(f)))
			{
				game.Run();
			}
		}
	}
}
