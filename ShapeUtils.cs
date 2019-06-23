using System;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal class ShapeUtils
	{
		public static void DrawArea(Vector2 start, Vector2 end, Action<Vector2> DrawObject)
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
			for (int x = (int)start.X; x < end.X; x++)
			{
				for (int y = (int)start.Y; y < end.Y; y++)
				{
					DrawObject(new Vector2(x, y));
				}
			}
		}
		public static void DrawLine(Vector2 start, Vector2 end, Action<Vector2> DrawObject)
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
	}
}
