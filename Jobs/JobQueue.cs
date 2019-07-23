using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal class JobQueue
	{
		public static Queue<Job> jobQueue = new Queue<Job>();
		private static Dictionary<JobType, List<Job>> jobs = new Dictionary<JobType, List<Job>>();
		private static void ValidateJobs(JobType j)
		{
			List<Job> t = new List<Job>();
			if (j == JobType.Any)
			{
				foreach(JobType jt in jobs.Keys)
				{
					foreach (Job job in jobs[jt])
					{
						if (job.IsCompleted || job.Owner != null)
							t.Add(job);
					}
				}
			}
			else
			{
				foreach (Job job in jobs[j])
				{
					if (job.IsCompleted || job.Owner != null)
						t.Add(job);
				}
			}
			foreach(Job job in t)
			{
				jobs[j].Remove(job);
			}
		}
		public static void AddJob(Job job, JobType jobType = JobType.Any)
		{
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
		public static Job GetJob(Character ch, JobType jobType = JobType.Any)
		{
			ValidateJobs(jobType);
			Job j;

			j = jobQueue.Dequeue();
			j.Owner = ch;
			return j;
		}
	}
	enum JobType
	{
		Any,
		Build,
		Deconstruct
	}
}
