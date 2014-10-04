using System;
using iTunesLib;

namespace iTunesUtils
{
    static class Classical
    {
        public static string GetTrackName(IITFileOrCDTrack track)
        {
            string workName = WorkName(track.Grouping, track.Name);
            // If the WorkName is empty, we've just collected the tracks by composer and album
            string trackName = workName == String.Empty
                ? track.Name
                : String.Format("{0} - {1}", workName, track.Composer);
            return trackName;
        }

        public static string WorkName(string groupingName, string trackName)
        {
            // If the Grouping is filled in, use it - this is the best case
            if (!String.IsNullOrEmpty(groupingName))
            {
                return groupingName;
            }

            // Overtures are normally only one track, and are good to intersperse between
            // longer works, so if the track name contains "overture" return it as the work name
            if (trackName.IndexOf("overture", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return trackName;
            }

            // Otherwise, return a blank work name, aka "unknown". We'll just group all of the blank
            // works for a given album and composer, which will work well enough in most cases
            return String.Empty;
        }
    }
}
