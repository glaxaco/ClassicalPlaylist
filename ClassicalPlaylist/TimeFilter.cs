using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTunesLib;

namespace iTunesUtils
{
    class TimeFilter
    {
        private readonly IEnumerable<IITFileOrCDTrack> tracks;
        private readonly Func<IITFileOrCDTrack, DateTime> dateTimeFunc;

        public TimeFilter(IEnumerable<IITFileOrCDTrack> tracks, Func<IITFileOrCDTrack, DateTime> dateTimeFunc)
        {
            this.tracks = tracks;
            this.dateTimeFunc = dateTimeFunc;
        }

        public IEnumerable<IITFileOrCDTrack> After(DateTime dateTimeToMatch)
        {
            return tracks.Where(t => dateTimeFunc(t) > dateTimeToMatch);
        }

        public IEnumerable<IITFileOrCDTrack> Before(DateTime dateTimeToMatch)
        {
            return tracks.Where(t => dateTimeFunc(t) < dateTimeToMatch);
        }
    }
}
