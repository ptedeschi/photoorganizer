using System.IO;
using System.Linq;

namespace Photo_Organizer
{
    internal class Storage
    {
        public static bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static bool IsSubfolder(string source, string destination)
        {
            string cp = Path.GetFullPath(destination);
            string pp = Path.GetFullPath(source);

            if (pp.StartsWith(cp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}