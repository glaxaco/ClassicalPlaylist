using System.Collections.Generic;
using System.Linq;
using iTunesLib;

namespace iTunesUtils.Extensions
{
    static class ClassicalExtensions
    {
        public static IEnumerable<IITFileOrCDTrack> ShuffleByWork(this IEnumerable<IITFileOrCDTrack> filetracks)
        {
            var workGroups = from filetrack in filetracks
                             group filetrack by new ClassicalWork(filetrack.Composer, filetrack.Album, Classical.WorkName(filetrack.Grouping, filetrack.Name));

            var shuffledWorkGroups = workGroups.Shuffle();

            foreach (var workGroup in shuffledWorkGroups)
            {
                // Work name is workGroup.Key;
                var tracks = workGroup.OrderBy(t => t.TrackNumber).ToList();
                //var trackNames = tracks.Select(t => t.Name).ToList(); // sometimes useful for debugging
                foreach (IITFileOrCDTrack track in tracks)
                {
                    yield return track;
                }
            }

        }
    }
}
