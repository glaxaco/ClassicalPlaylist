using System;
using System.Linq;
using System.Runtime.InteropServices;
using iTunesLib;

namespace iTunesUtils
{
    class iTunes : IDisposable
    {
        private readonly iTunesAppClass iTunesApp = new iTunesAppClass();

        public iTunesLibrary Library
        {
            get
            {
                return new iTunesLibrary(iTunesApp.LibraryPlaylist);
            }
        }

        public IITUserPlaylist FindPlaylistByName(string playlistName)
        {
            dynamic library = iTunesApp.Sources.Cast<dynamic>().First(source => (int)source.Kind == (int)ITSourceKind.ITSourceKindLibrary);
            foreach (dynamic playlist in library.Playlists)
            {
                if (playlist.Name == playlistName)
                {
                    return playlist;
                }
            }
            return null;
        }

        public IITUserPlaylist CreatePlaylist(string playlistName)
        {
            var playlist = (IITUserPlaylist)iTunesApp.CreatePlaylist(playlistName);
            return playlist;
        }

        public void Dispose()
        {
            Marshal.ReleaseComObject(iTunesApp);
        }
    }
}
