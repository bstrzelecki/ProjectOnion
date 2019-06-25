using System.Collections.Generic;
using System.Linq;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	internal class JobQueue
	{
		public static Queue<Job> jobQueue = new Queue<Job>();

		public static void AddJob(Job job)
		{
			foreach (Job j in jobQueue)
			{
				if (j.Equals(job))
				{
					return;
				}
			}
			jobQueue.Enqueue(job);
		}
		public static List<Job> GetJobsAtPosition(Vector2 pos)
		{
			return (from n in jobQueue where new Vector2(n.tile.X, n.tile.Y) == pos select n).ToList();
		}
		public static Job GetJob()
		{
			Job j;
			do
			{
				j = jobQueue.Dequeue();
			} while (j.isDispoded || j == null);
			return jobQueue.Dequeue();
		}
		public static void DrawBlueprints(SpriteBatch sprite)
		{
			foreach (Job job in jobQueue)
			{
				if (job.isDispoded)
				{
					continue;
				}

				BlueprintData data = job.jobEvents.GetBlueprintData();
				sprite.Draw(data.objectSprite, new TileRectangle(data.position), Color.White);
				DrawBlips(sprite, data);
			}
		}

		private static void DrawBlips(SpriteBatch sprite, BlueprintData data)
		{
			Vector2 cornerPos = TileRectangle.GetCorner(data.position);
			int i = 0;
			foreach (Sprite blip in data.blips)
			{
				sprite.Draw(blip, new Rectangle((int)cornerPos.X + blip.Texture.Width * i, (int)cornerPos.Y, 8, 8), Color.White);
				i++;
			}
		}
	}
}
