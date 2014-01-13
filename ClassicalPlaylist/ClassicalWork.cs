using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicalPlaylist
{
    public class ClassicalWork
    {
        public string ComposerName { get; private set; }
        public string WorkName { get; private set; }

        public ClassicalWork(string composerName, string workName)
        {
            if (composerName == null) throw new ArgumentNullException("composerName");
            if (workName == null) throw new ArgumentNullException("workName");

            ComposerName = composerName;
            WorkName = workName;
        }

        public override string ToString()
        {
            return string.Format("{0} by {1}", WorkName, ComposerName);
        }

        public override bool Equals(object obj)
        {
            var other = obj as ClassicalWork;
            if (other == null) return false;

            return other.ComposerName == ComposerName && other.WorkName == WorkName;
        }

        public override int GetHashCode()
        {
            return ComposerName.GetHashCode() ^ WorkName.GetHashCode();
        }
    }
}
