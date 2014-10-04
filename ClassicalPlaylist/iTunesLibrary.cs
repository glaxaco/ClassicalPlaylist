using System;
using System.Collections.Generic;
using System.Linq;

using iTunesLib;

namespace iTunesUtils
{
    class iTunesLibrary
    {
        private readonly IITLibraryPlaylist mainLibrary;

        public iTunesLibrary(IITLibraryPlaylist mainLibrary)
        {
            this.mainLibrary = mainLibrary;
        }

        public IEnumerable<IITFileOrCDTrack> FileTracks
        {
            get
            {
                var filetracks = from object track in mainLibrary.Tracks
                    let filetrack = track as IITFileOrCDTrack
                    where filetrack != null
                    select filetrack;

                return filetracks;
            }
        }

        #region Work Name Extraction
        // This is experimental stuff to extract the name of the work from a group of track titles.
        // This turns out to be a hard problem

        public void FindWorkNames()
        {
            var albumGroups = from object track in mainLibrary.Tracks
                let filetrack = track as IITFileOrCDTrack
                where filetrack != null
                      && filetrack.Genre == "Classical"
                      && !string.IsNullOrEmpty(filetrack.Composer)
                      && string.IsNullOrEmpty(filetrack.Grouping)
                group filetrack by new {filetrack.Album, filetrack.Composer}
            ;

            foreach (var albumGroup in albumGroups)
            {
                Console.WriteLine("Album: {0}, Composer: {1}", albumGroup.Key.Album, albumGroup.Key.Composer);
                var albumtracks = albumGroup.OrderBy(t => t.TrackNumber).ToList();
                for (int i=0; i<albumtracks.Count; i++)
                {
                    var track1 = albumtracks[i];
                    for (int j = i + 1; j < albumtracks.Count; j++)
                    {
                        var track2 = albumtracks[j];
                        string overlap = FindOverlap(track1.Name, track2.Name);
                        Console.WriteLine("  track[{0}] = '{1}'", i, track1.Name);
                        Console.WriteLine("  track[{0}] = '{1}'", j, track2.Name);
                        Console.WriteLine("  overlap  = '{0}'", overlap);
                    }
                }
                Console.WriteLine("====================================================================");
            }
        }

        private string FindOverlap(string s1, string s2)
        {
            int overlapLength = FindOverlapLength(s1, s2);
            return s1.Substring(0, overlapLength);
        }

        private int FindOverlapLength(string s1, string s2)
        {
            int minLength = Math.Min(s1.Length, s2.Length);
            for (int i = 0; i < minLength; i++)
            {
                if (s1[i] != s2[i])
                {
                    return i;                    
                }
            }
            return minLength;
        }
        #endregion

    }
}
