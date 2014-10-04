using System;

namespace iTunesUtils
{
    public class ClassicalWork
    {
        public string Composer { get; private set; }
        public string Album { get; private set; }
        public string Name { get; private set; }

        public ClassicalWork(string composerName, string albumName, string workName)
        {
            if (composerName == null) throw new ArgumentNullException("composerName");
            if (workName == null) throw new ArgumentNullException("workName");

            Composer = composerName;
            Album = albumName;
            Name = workName;
        }

        public override string ToString()
        {
            return string.Format("{0} by {1}", Name, Composer);
        }

        public override bool Equals(object obj)
        {
            var other = obj as ClassicalWork;
            if (other == null) return false;

            return 
                other.Composer == Composer && 
                other.Album == Album && 
                other.Name == Name;
        }

        public override int GetHashCode()
        {
            return Composer.GetHashCode() ^ Album.GetHashCode() ^ Name.GetHashCode();
        }
    }
}
