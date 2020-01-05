using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MBBSlib.MonoGame;

namespace ProjectOnion
{
	class JobSerializer : ISerializable
	{
		public string GetHeader()
		{
			return "jobs";
		}
		public void Load(XElement data)
		{
			foreach(XElement e in data.Elements("job"))
			{
				Type type = Type.GetType("ProjectOnion." + e.Element("JobData").Value);

				List<object> objs = new List<object>();
				objs.Add(MainScene.world.GetTile(int.Parse(e.Element("Position").Element("X").Value), int.Parse(e.Element("Position").Element("Y").Value)));
				foreach(XAttribute a in e.Element("Ctor").Attributes())
				{
					switch (a.Name.ToString())
					{
						case "MountedObject":
							objs.Add(Registry.furnitures[a.Value].GetFurniture());
							break;
						case "Sprite":
							objs.Add(new Sprite(a.Value));
							break;
						case "Float":
							objs.Add(float.Parse(a.Value));
							break;
					}
				}	
				
				Job job = (Job)Activator.CreateInstance(type, objs.ToArray());

				job.jobLayer = (JobLayer)Enum.Parse(typeof(JobLayer), e.Element("JobLayer").Value);
				job.onTile = bool.Parse(e.Element("IsOnTile").Value);
				job.startWorkTime = int.Parse(e.Element("StartWorkTime").Value);
				job.workTime = int.Parse(e.Element("WorkTime").Value);
				job.tile = MainScene.world.GetTile(int.Parse(e.Element("Position").Element("X").Value), int.Parse(e.Element("Position").Element("Y").Value));
				if (e.Element("Resources")?.HasElements??false)
				{
					foreach (XElement r in e.Element("Resources").Elements("Resources"))
					{
						job.resources.Add(new ItemStack(r.Element("Name").Value, int.Parse(r.Element("Amount").Value)));
					}
				}
				if (e.Element("SuppliedResources")?.HasElements??false)
				{
					foreach (XElement r in e.Element("SuppliedResources").Elements("Resources"))
					{
						job.suppliedResources.Add(new ItemStack(r.Element("Name").Value, int.Parse(r.Element("Amount").Value)));
					}
				}
				if (e.HasAttributes)
				{
					JobQueue.AddActiveJob(job, Registry.characters[int.Parse(e.Attribute("OwnerId").Value)], job.jobLayer);
				}
				else
				{
					JobQueue.AddJob(job, job.jobLayer);
				}
			}
		}

		public XElement Save()
		{
			XElement e = new XElement("jobs");

			foreach(JobLayer l in JobQueue.GetPendingJobs().Keys)
			{
				foreach(Job j in JobQueue.GetPendingJobs()[l])
				{
					XElement entry = new XElement("job");
					entry.Add(new XElement("JobLayer", j.jobLayer));
					entry.Add(new XElement("IsOnTile", j.onTile));
					entry.Add(new XElement("StartWorkTime", j.startWorkTime));
					entry.Add(new XElement("WorkTime", j.workTime));
					entry.Add(new XElement("Position", new XElement("X", j.tile.X), new XElement("Y", j.tile.Y)));
					entry.Add(new XElement("JobData", j.GetType().Name));
					entry.Add(j.GetCtorData());
					XElement r = new XElement("Resources");
					foreach(ItemStack stack in j.resources)
					{
						r.Add(new XElement("Resource", new XElement("Name", stack.GetResource()), new XElement("Amount", stack.GetAmount())));
					}
					entry.Add(r);
					XElement sr = new XElement("SuppliedResuorces");
					foreach(ItemStack stack in j.suppliedResources)
					{
						r.Add(new XElement("Resource", new XElement("Name", stack.GetResource()), new XElement("Amount", stack.GetAmount())));
					}
					entry.Add(sr);
					e.Add(entry);
				}
			}

			foreach (JobLayer l in JobQueue.GetActiveJobs().Keys)
			{
				foreach (Job j in JobQueue.GetActiveJobs()[l])
				{
					XElement entry = new XElement("job");
					entry.SetAttributeValue("OwnerId", j.Owner.id);
					entry.Add(new XElement("JobLayer", j.jobLayer));
					entry.Add(new XElement("IsOnTile", j.onTile));
					entry.Add(new XElement("StartWorkTime", j.startWorkTime));
					entry.Add(new XElement("WorkTime", j.workTime));
					entry.Add(new XElement("Position", new XElement("X", j.tile.X), new XElement("Y", j.tile.Y)));
					entry.Add(new XElement("JobData", j.GetType().Name));
					entry.Add(j.GetCtorData());
					XElement r = new XElement("Resources");
					foreach (ItemStack stack in j.resources)
					{
						r.Add(new XElement("Resource", new XElement("Name", stack.GetResource()), new XElement("Amount", stack.GetAmount())));
					}
					entry.Add(r);
					XElement sr = new XElement("SuppliedResources");
					foreach (ItemStack stack in j.suppliedResources)
					{
						r.Add(new XElement("Resource", new XElement("Name", stack.GetResource()), new XElement("Amount", stack.GetAmount())));
					}
					entry.Add(sr);
					e.Add(entry);
				}
			}

			return e;
		}
	}
}
