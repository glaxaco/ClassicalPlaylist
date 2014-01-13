using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iTunesLib;

namespace ClassicalPlaylist
{
    class ClassicalLibrary
    {
        private readonly IITLibraryPlaylist mainLibrary;
        public ClassicalLibrary(IITLibraryPlaylist mainLibrary)
        {
            this.mainLibrary = mainLibrary;
        }

        public void CreatePlaylist(string playlistFullPath)
        {
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
                        int trackSeconds = ParseTrackTimeToSeconds(track.Time);
                        playlist.WriteTrack(trackName, trackSeconds, track.Location);
                    }
                }
            }            
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
