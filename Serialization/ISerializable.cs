using System.Xml.Linq;

namespace ProjectOnion
{
	public interface ISerializable
	{
		XElement Save();
		void Load(XElement data);
		string GetHeader();
	}
}
