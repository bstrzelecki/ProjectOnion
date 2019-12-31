using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
namespace ProjectOnion
{
	internal class JobQueue
	{
		public static Queue<Job> jobQueue = new Queue<Job>();
		private static Dictionary<JobLayer, List<Job>> jobs = InitializeJobDictionary();
		private static Dictionary<JobLayer, List<Job>> InitializeJobDictionary()
		{
			var j = new Dictionary<JobLayer, List<Job>>();
			foreach (string name in Enum.GetNames(typeof(JobLayer)))
			{
				j.Add((JobLayer)Enum.Parse(typeof(JobLayer), name), new List<Job>());
			}
			return j;
		}
		private static void ValidateJobs(JobLayer j)
		{
			List<Job> t = new List<Job>();
			if (j == JobLayer.Any)
			{
				foreach (JobLayer jt in jobs.Keys)
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
			foreach (Job job in t)
			{
				foreach (JobLayer l in jobs.Keys)
				{
					jobs[l].Remove(job);
				}
			}
		}
		public static void AddJob(Job job, JobLayer jobType = JobLayer.Any)
		{
			ValidateJobs(jobType);
			foreach (Job j in jobs[jobType])
			{
				if (j.Equals(job))
				{
					return;
				}
			}
			if (job == null) return;

			job.tile.job[(int)jobType] = job;
			job.Register();
			jobs[jobType].Add(job);
		}
		public static List<Job> GetJobsAtPosition(Vector2 pos)
		{
			return (from n in jobQueue where new Vector2(n.tile.X, n.tile.Y) == pos select n).ToList();
		}
		private static Job GetClosestJob(Character c, JobLayer jt)
		{

			var pathfinding = new MBBSlib.AI.Pathfinding(MainScene.world.GetPathfindingGraph());


			var points = new List<MBBSlib.AI.Point>();
			foreach (Job job in jobs[jt])
			{
				points.Add(new MBBSlib.AI.Point(job.tile.X, job.tile.Y));
			}
			var path = pathfinding.GetPath(points, new MBBSlib.AI.Point(c.tile.X, c.tile.Y));
			if (path == null) return null;
			Tile t = MainScene.world.GetTile(path[path.Count - 1].X, path[path.Count - 1].Y);
			Job j = t.job[(int)jt];
			if (j != null) jobs[jt].Remove(j);
			return j;
		}
		public static Job GetJob(Character ch, JobLayer jobType = JobLayer.Any)
		{
			ValidateJobs(jobType);
			Job j = null;
			if (jobType == JobLayer.Any)
			{
				foreach (JobLayer jt in jobs.Keys)
				{
					j = GetClosestJob(ch, jt);
					if (j != null) break;
				}
			}
			else
			{
				j = GetClosestJob(ch, jobType);
			}
			if (j == null) return null;
			j.Owner = ch;
			return j;
		}
	}
}
