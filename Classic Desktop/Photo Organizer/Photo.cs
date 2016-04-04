using System;

namespace Photo_Organizer
{
    internal class Photo
    {
        private const string pattern = "yyyy.MM.dd";
        private string name;
        private string path;
        private DateTime dateTaken;
        private string dateTakenString;

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

        public string DateTakenString
        {
            get
            {
                return dateTaken.ToString(pattern);
            }
        }
    }
}