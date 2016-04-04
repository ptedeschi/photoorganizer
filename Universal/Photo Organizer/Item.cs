using System;
using Windows.Storage;

namespace Photo_Organizer
{
    internal class Item
    {
        private StorageFile file;
        private DateTime dateTaken;

        public StorageFile File
        {
            get
            {
                return file;
            }

            set
            {
                file = value;
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