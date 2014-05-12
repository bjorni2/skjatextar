using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SkjaTextar.Helpers
{
	/// <summary>
	/// The function returns the id for a YouTube video.  The code was borrowed from 
	/// the user Septih on StackOverflow, and was posted at this address: 
	/// http://stackoverflow.com/questions/3652046/c-sharp-regex-to-get-video-id-from-youtube-and-vimeo-by-url.
	/// </summary>
	public class YoutubeParser
	{
		public static string parseLink(string youtubeLink)
		{
			Regex regx = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");

			Match youtubeMatch = regx.Match(youtubeLink);

			string id = string.Empty;

			if (youtubeMatch.Success)
			{
				id = youtubeMatch.Groups[1].Value;
			}

			return id;
		}
	}
}