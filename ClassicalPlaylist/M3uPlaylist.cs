using System.IO;

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

        public void WriteTrack(string trackName, int trackSeconds, string trackPath)
        {
            textWriter.WriteLine("#EXTINF:{0},{1}", trackSeconds, trackName);
            textWriter.WriteLine(trackPath);
        }
    }
}
