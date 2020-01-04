using System;
using System.Collections.Generic;
using System.Linq;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	internal class JobQueue : MBBSlib.MonoGame.IDrawable
	{
		private static readonly Dictionary<JobLayer, List<Job>> activeJobs = InitializeJobDictionary();
		private static readonly Dictionary<JobLayer, List<Job>> pendingJobs = InitializeJobDictionary();

		private static Dictionary<JobLayer, List<Job>> InitializeJobDictionary()
		{
			var j = new Dictionary<JobLayer, List<Job>>();
			foreach (string name in Enum.GetNames(typeof(JobLayer)))
			{
				j.Add((JobLayer)Enum.Parse(typeof(JobLayer), name), new List<Job>());
			}
			return j;
		}
		public JobQueue()
		{
			GameMain.RegisterRenderer(this);
		}
		public static void AddJob(Job job, JobLayer jobType = JobLayer.Any)
		{
			if (job == null) return;

			pendingJobs[jobType].Add(job);
		}
		private static Job GetClosestJob(Vector2 pos, JobLayer jt)
		{
			var pathfinding = new MBBSlib.AI.Pathfinding(MainScene.world.GetPathfindingGraph());

			var points = new List<MBBSlib.AI.Point>();
			foreach (Job job in pendingJobs[jt])
			{
				points.Add(new MBBSlib.AI.Point(job.tile.X, job.tile.Y));
			}
			var path = pathfinding.GetPath(points, new MBBSlib.AI.Point((int)pos.X, (int)pos.Y));
			if (path == null) return null;
			Tile t = MainScene.world.GetTile(path[path.Count - 1].X, path[path.Count - 1].Y);
			Job j = (from n in pendingJobs[jt] where n.tile == t select n).First();
			return j;
		}
		public static Job GetJob(Character ch, JobLayer jobType = JobLayer.Any)
		{
			Job j = null;
			if (jobType == JobLayer.Any)
			{
				foreach (JobLayer jt in pendingJobs.Keys)
				{
					j = GetClosestJob(ch.Position, jt);
					if (j != null) break;
				}
			}
			else
			{
				j = GetClosestJob(ch.Position, jobType);
			}
			if (j == null) return null;
			if (jobType == JobLayer.Any)
			{
				foreach (JobLayer jt in pendingJobs.Keys)
				{
					if (pendingJobs.ContainsKey(jt))
					{
						if (pendingJobs[jt].Contains(j))
						{
							pendingJobs[jt].Remove(j);
							activeJobs[jt].Add(j);
						}
					}
				}
			}
			else
			{
				pendingJobs[jobType].Remove(j);
				activeJobs[jobType].Add(j);
			}

			j.Owner = ch;
			return j;
		}

		public void Draw(SpriteBatch sprite)
		{
			foreach (JobLayer jl in pendingJobs.Keys)
			{
				foreach (Job j in pendingJobs[jl])
				{
					j.Draw(sprite);
				}
			}
			foreach (JobLayer jl in activeJobs.Keys)
			{
				foreach (Job j in activeJobs[jl])
				{
					j.Draw(sprite);
				}
			}
		}
	}
}
