using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo_Organizer
{
    class Photo
    {
        string name;
        string path;
        DateTime dateTaken;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Path
        {
            get
            {
                return path;
            }

            set
            {
                path = value;
            }
        }

        public DateTime DateTaken
        {
            get
            {
                return dateTaken;
            }

            set
            {
                dateTaken = value;
            }
        }
    }
}
