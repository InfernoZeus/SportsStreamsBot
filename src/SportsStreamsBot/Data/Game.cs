using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace SportsStreamsBot.Data
{
	[XmlRoot("game")]
	[XmlType("game")]
	public class Game
	{
		[XmlAttribute("id")]
		public int GameID { get; set; }

		[XmlAttribute("monthId")]
		public int MonthID { get; set; }
		
		[XmlElement("utcStart")]
		public string UtcStartSTring
		{
			get { return UtcStart.ToString("yyyy-MM-dd HH:mm:ss") + "+0000"; }
			set { throw new NotImplementedException(); }
		}

		[XmlIgnore]
		public DateTime UtcStart { get; set; }

		[XmlIgnore]
		public string Summary { get; set; }

		[XmlText]
		[XmlElement("summary")]
		public XmlNode[] CDataContent
		{
			get
			{
				var dummy = new XmlDocument();
				return new XmlNode[] { dummy.CreateCDataSection(Summary) };
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		[XmlElement("homeTeam")]
		public Team HomeTeam { get; set; }

		[XmlElement("awayTeam")]
		public Team AwayTeam { get; set; }

		public Game()
		{
			HomeTeam = new Team();
			AwayTeam = new Team();
		}
	}
}
