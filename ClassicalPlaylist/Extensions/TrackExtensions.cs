using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using iTunesLib;

namespace iTunesUtils.Extensions
{
    static class TrackExtensions
    {
        public static IEnumerable<IITFileOrCDTrack> InGenre(this IEnumerable<IITFileOrCDTrack> trackCollection, string genreName)
        {
            return from IITFileOrCDTrack track in trackCollection
                where track.Genre == genreName
                select track;
        }

        public static IEnumerable<IITFileOrCDTrack> WithComposer(this IEnumerable<IITFileOrCDTrack> trackCollection)
        {
            return from IITFileOrCDTrack track in trackCollection
                   where !string.IsNullOrWhiteSpace(track.Composer)
                   select track;
        }

        public static IEnumerable<IITFileOrCDTrack> WithFile(this IEnumerable<IITFileOrCDTrack> trackCollection)
        {
            return from IITFileOrCDTrack track in trackCollection
                   where File.Exists(track.Location)
                   select track;
        }

        public static TimeFilter LastPlayed(this IEnumerable<IITFileOrCDTrack> trackCollection)
        {
            return new TimeFilter(trackCollection, track => track.PlayedDate);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var random = new Random();
            var shuffled = from item in enumerable
                           orderby random.Next()
                           select item;

            return shuffled;
        }


        public static void WriteTo(this IITFileOrCDTrack track, M3uPlaylist playlist, string trackName)
        {
            int trackSeconds = ParseTrackTimeToSeconds(track.Time);
            playlist.WriteTrack(trackName, trackSeconds, track.Location);
        }

        private static int ParseTrackTimeToSeconds(string trackTime)
        {
            string[] trackTimeParts = trackTime.Split(':');
            if (trackTimeParts.Length != 2)
            {
                throw new Exception(string.Format("Invalid track time: {0}", trackTime));
            }

            return Convert.ToInt32(trackTimeParts[0]) * 60 + Convert.ToInt32(trackTimeParts[1]);
        }

    }
}
