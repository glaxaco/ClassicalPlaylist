using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using iTunesLib;

namespace ClassicalPlaylist
{
    class Program
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
                Console.WriteLine("Usage: {0} <playlist full path name>", programName);
                return;
            }

            string playlistFullPath = args[0].Replace("%USERPROFILE%", Environment.GetEnvironmentVariable("USERPROFILE"));
            var iTunesApp = new iTunesAppClass();
            try
            {
                IITLibraryPlaylist mainLibrary = iTunesApp.LibraryPlaylist;
                var random = new Random();

                var workGroups = from object track in mainLibrary.Tracks
                    let filetrack = track as IITFileOrCDTrack
                    where filetrack != null
                          && filetrack.Genre == "Classical"
                          && !string.IsNullOrEmpty(filetrack.Composer)
                          && !string.IsNullOrEmpty(filetrack.Grouping)
                          && File.Exists(filetrack.Location)
                    group filetrack by new ClassicalWork(filetrack.Composer, filetrack.Grouping);

                var shuffledWorkGroups = from item in workGroups
                                     orderby random.Next()
                                     select item;

                using (var writer = new StreamWriter(playlistFullPath))
                {
                    var playlist = new M3uPlaylist(writer);
                    playlist.WriteHeader();
                    foreach (var workGroup in shuffledWorkGroups)
                    {
                        foreach (IITFileOrCDTrack track in workGroup.OrderBy(t => t.TrackNumber))
                        {
                            var classicalWork = workGroup.Key;
                            string trackName = string.Format("{0} - {1}", classicalWork.WorkName,
                                classicalWork.ComposerName);
                            playlist.WriteTrack(track, trackName);
                        }
                    }
                }
            }
            finally
            {
                Marshal.ReleaseComObject(iTunesApp);
            }
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Console.Error.WriteLine("Exception caught: {0}", unhandledExceptionEventArgs.ExceptionObject);
        }
    }
}
