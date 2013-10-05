using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;

namespace SportsStreamsBot
{
	class HockeySummaryDownloader : ISummaryDownloader
	{
		// we might need to factor the key generation outside of this interface, in case NBA and NHL end up having different formats.

		public string GetGameSummary(DateTime startTimeEastern, int monthID, int gameID)
		{
			// sometimes games show up as repeats with a non-broadcast feed
			// they have a long ID, and we can't resolve them
			if (gameID > 9999)
				return string.Empty;
			
			// generate the full game ID
			// format is "yyyymmgggg"
			var fullID = string.Format("{0}{1:00}{2:0000}", startTimeEastern.Year, monthID, gameID);

			var url = string.Format("http://www.nhl.com/gamecenter/en/preview?id={0}", fullID);

			var document = new HtmlWeb().Load(url);

			var paragraphs = document.DocumentNode.QuerySelectorAll("p b").ToList();

			
			foreach (var paragraph in paragraphs)
			{
				// look for the "big story" section
				if (paragraph.InnerText.ToLower().Trim() == "big story:")
				{
					// null safety in case they change the format
					if (paragraph.ParentNode.ChildNodes.Count < 2)
					{
						// bail out, something is wrong.
						break;
					}

					// format avoids possible NPE if innertext is null.
					var bigStory = string.Format("{0}", paragraph.ParentNode.InnerText);
					// remove the caption
					var colonIndex = bigStory.IndexOf(":");
					bigStory = bigStory.Substring(colonIndex + 1).Trim();

					// we're done here
					return bigStory;
				}
			}

			// couldn't find what we were looking for
			return string.Empty;
		}
	}
}
