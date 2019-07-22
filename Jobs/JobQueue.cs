using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal class JobQueue
	{
		public static Queue<Job> jobQueue = new Queue<Job>();

		public static void AddJob(Job job)
		{
			if (job == null || job.IsCompleted) return;
			foreach (Job j in jobQueue)
			{
				if (j.Equals(job))
				{
					return;
				}
			}
			job.Register();
			jobQueue.Enqueue(job);
		}
		public static List<Job> GetJobsAtPosition(Vector2 pos)
		{
			return (from n in jobQueue where new Vector2(n.tile.X, n.tile.Y) == pos select n).ToList();
		}
		public static Job GetJob(Character ch)
		{
			Job j;
			do
			{
				if (jobQueue.Count == 0) return null;
				j = jobQueue.Dequeue();
			} while (j.IsCompleted || j.Owner != null);
			j.Owner = ch;
			return j;
		}
	}
}
