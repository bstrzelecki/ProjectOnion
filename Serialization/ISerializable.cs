using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	public interface ISerializable
	{
		XElement Save();
		void Load(XElement data);
		string GetHeader();
	}
}
