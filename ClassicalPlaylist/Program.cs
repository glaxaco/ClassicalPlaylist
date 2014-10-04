using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using iTunesLib;
using iTunesUtils.Extensions;

namespace iTunesUtils
{
    static class Program
    {
        /*
        ** License:
        ** You may use this software freely for personal use.
        ** This software is provided AS IS with no warrenty of any kind, implied or otherwise.         
        */
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            if (args.Length == 0)
            {
                string programName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                Console.WriteLine("Usage: {0} [playlist name] [playlist full path name]", programName);
                return;
            }

            string playlistName = "Classical Shuffle";
            string playlistFullPath = null;
            if (args.Length >= 1)
            {
                playlistName = args[0];
            }
            if (args.Length >= 2)
            {
                playlistFullPath = Environment.ExpandEnvironmentVariables(args[0]);
            }

            using (var iTunes = new iTunes())
            {
                List<IITFileOrCDTrack> tracks = 
                    iTunes.Library.FileTracks
                        .InGenre("Classical")
                        .WithComposer()
                        //.LastPlayed().Before(3.Months().Ago())  // Fun with fluent interfaces!
                        .WithFile()
                        .ShuffleByWork()
                        .ToList();

                IITUserPlaylist playlist = iTunes.FindPlaylistByName(playlistName);
                if (playlist != null)
                {
                    playlist.Delete();
                }
                playlist = iTunes.CreatePlaylist(playlistName);
                foreach (var track in tracks)
                {
                    playlist.AddTrack(track);
                }

                if (playlistFullPath != null)
                {
                    CreateM3uPlaylist(tracks, playlistFullPath, Classical.GetTrackName);
                }
            }
        }

        private static void CreateM3uPlaylist(IEnumerable<IITFileOrCDTrack> shuffledTracks, string playlistFullPath, Func<IITFileOrCDTrack, string> trackName)
        {
            using (var writer = new StreamWriter(playlistFullPath))
            {
                var playlist = new M3uPlaylist(writer);
                playlist.WriteHeader();
                foreach (var track in shuffledTracks)
                {
                    track.WriteTo(playlist, trackName(track));
                }
            }
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Console.Error.WriteLine("Exception caught: {0}", unhandledExceptionEventArgs.ExceptionObject);
        }
    }
}
