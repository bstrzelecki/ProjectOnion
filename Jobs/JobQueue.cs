﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System;
namespace ProjectOnion
{
	internal class JobQueue
	{
		public static Queue<Job> jobQueue = new Queue<Job>();
		private static Dictionary<JobType, List<Job>> jobs = InitializeJobDictionary();
		private static Dictionary<JobType, List<Job>> InitializeJobDictionary()
		{
			Dictionary<JobType, List<Job>> j = new Dictionary<JobType, List<Job>>();
			foreach(string name in Enum.GetNames(typeof(JobType)))
			{
				j.Add((JobType)Enum.Parse(typeof(JobType), name), new List<Job>());
			}
			return j;
		}
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
		private static float CalculatePathLenght(List<MBBSlib.AI.Point> path)
		{
			float val = 0f;
			foreach(var p in path)
			{
				Tile t = MainScene.world.GetTile(p.X, p.Y);
				val += t.GetPathfindingMovementCost();
			}
			return val;
		}
		private static Job GetClosestJob(Character c,JobType jt)
		{

			var pathfinding = new MBBSlib.AI.Pathfinding(MainScene.world.GetPathfindingGraph());


			List<MBBSlib.AI.Point> points = new List<MBBSlib.AI.Point>();
			foreach(Job job in jobs[jt])
			{
				points.Add(new MBBSlib.AI.Point(job.tile.X, job.tile.Y));
			}
			var path = pathfinding.GetPath(points, new MBBSlib.AI.Point(c.tile.X, c.tile.Y));
			if (path == null) return null;
			Job j = MainScene.world.GetTile(path[path.Count - 1].X, path[path.Count - 1].Y).job[(int)jt];
			if (j != null)jobs[jt].Remove(j);
			return j;
		}
		public static Job GetJob(Character ch, JobType jobType = JobType.Any)
		{
			ValidateJobs(jobType);
			Job j = null;
			if (jobType == JobType.Any)
			{
				foreach (JobType jt in jobs.Keys)
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
	enum JobType
	{
		Any,
		Build,
		Deconstruct
	}
}
