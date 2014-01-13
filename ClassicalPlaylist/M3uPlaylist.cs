using System;
using System.IO;

using iTunesLib;

namespace ClassicalPlaylist
{
    class M3uPlaylist
    {
        private readonly TextWriter textWriter;

        public M3uPlaylist(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public void WriteHeader()
        {
            // See http://schworak.com/blog/e39/m3u-play-list-specification/
            textWriter.WriteLine("#EXTM3U");
        }

        public void WriteTrack(IITFileOrCDTrack track, string trackName)
        {
            var trackSeconds = ParseTrackTimeToSeconds(track.Time);
            textWriter.WriteLine("#EXTINF:{0},{1}", trackSeconds, trackName);
            textWriter.WriteLine(track.Location);
        }

        private static int ParseTrackTimeToSeconds(string trackTime)
        {
            string[] trackTimeParts = trackTime.Split(':');
            if (trackTimeParts.Length != 2)
            {
                throw new Exception(string.Format("Invalid track time: {0}", trackTime));
            }

            return Convert.ToInt32(trackTimeParts[0])*60 + Convert.ToInt32(trackTimeParts[1]);
        }
    }
}
