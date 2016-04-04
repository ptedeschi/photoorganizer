using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExifLib;

namespace Photo_Organizer
{
    public partial class MainForm : Form
    {
        private const string SpecialFolderName = "misc";
        private static Regex r = new Regex(":");

        public MainForm()
        {
            InitializeComponent();

            this.textBoxSource.Text = @"C:\Users\patrick.tedeschi\Google Drive\Google Photos";
            this.textBoxDestination.Text = @"C:\Users\patrick.tedeschi\Desktop\Result";
        }

        private void buttonSource_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            this.textBoxSource.Text = fbd.SelectedPath;
        }

        private void buttonDestination_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            this.textBoxDestination.Text = fbd.SelectedPath;
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            string source = this.textBoxSource.Text;
            string destination = this.textBoxDestination.Text;

            if (ValidateInput(source, destination))
            {
                DirectoryInfo info = new DirectoryInfo(source);
                FileInfo[] files = info.GetFiles();

                List<Photo> photos = new List<Photo>();

                foreach (FileInfo file in files)
                {
                    Photo photo = new Photo();
                    photo.Name = file.Name;
                    photo.Path = file.FullName;
                    photo.DateTaken = GetDateTakenFromImage(file.FullName);

                    photos.Add(photo);
                }

                Photo[] photoArray = photos.OrderBy(p => p.DateTaken).ToArray();

                string currentFolder = null;
                DateTime currentDateTaken = new DateTime(1, 1, 1);

                foreach (Photo item in photoArray)
                {
                    DateTime dateTaken = item.DateTaken;

                    // If currentFolder is null, it means that is the first file so we need to create a folder for it anyway
                    if (currentFolder == null)
                    {
                        currentFolder = Path.Combine(destination, item.DateTakenString);
                        Directory.CreateDirectory(currentFolder);
                        File.Copy(item.Path, Path.Combine(currentFolder, item.Name), true);
                    }
                    else
                    {
                        string tempFolder = Path.Combine(destination, item.DateTakenString);

                        if (Storage.Exists(tempFolder))
                        {
                            // If the directory already exists, it means that is the current folder
                            File.Copy(item.Path, Path.Combine(currentFolder, item.Name), true);
                        }
                        else
                        {
                            // if the directory doesn`t exists, we need to check if we need to combine the folder days or create a new one
                            int totalDays = (int)Math.Round((dateTaken - currentDateTaken).TotalDays);
                            int minDays = int.Parse(this.textBoxMinPhotosDays.Text);

                            if (totalDays <= minDays)
                            {
                                File.Copy(item.Path, Path.Combine(currentFolder, item.Name), true);
                            }
                            else
                            {
                                // At this point, we need to create a new directory to keep the incoming files.
                                // But, before changing the current folder, let`s check if it meets the MinPhotosPerFolder criteria
                                HandleMinPhotosPerFolder(currentFolder, destination);

                                currentFolder = tempFolder;

                                Directory.CreateDirectory(currentFolder);
                                File.Copy(item.Path, Path.Combine(currentFolder, item.Name), true);
                            }
                        }
                    }

                    // Update last item date
                    currentDateTaken = dateTaken;
                }

                // And now that we finish, let`s check if the latest folder meets the MinPhotosPerFolder criteria
                HandleMinPhotosPerFolder(currentFolder, destination);
            }
            else
            {
                MessageBox.Show("Error");
            }

            watch.Stop();

            var sourceCount = (from file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories) select file).Count();
            var destinationCount = (from file in Directory.EnumerateFiles(destination, "*", SearchOption.AllDirectories) select file).Count();

            Cursor.Current = Cursors.Default;

            MessageBox.Show(string.Format("Source: {0}\nDestination: {1}\nTotal time: {2}", sourceCount, destinationCount, watch.Elapsed));

            watch.Reset();
        }

        private bool ValidateInput(string source, string destination)
        {
            // Source
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }

            if (!Directory.Exists(source))
            {
                return false;
            }

            if (Storage.IsDirectoryEmpty(source))
            {
                return false;
            }

            // Destination
            if (string.IsNullOrWhiteSpace(destination))
            {
                return false;
            }

            if (!Directory.Exists(destination))
            {
                return false;
            }

            if (!Storage.IsDirectoryEmpty(destination))
            {
                return false;
            }

            if (Storage.IsSubfolder(source, destination))
            {
                return false;
            }

            return true;
        }

        public static DateTime GetDateTakenFromImage(string path)
        {
            try
            {
                // Instantiate the reader
                using (ExifReader reader = new ExifReader(path))
                {
                    // Extract the tag data using the ExifTags enumeration
                    DateTime datePictureTaken;

                    if (reader.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out datePictureTaken))
                    {
                        return datePictureTaken;
                    }
                    else
                    {
                        FileInfo file = new FileInfo(path);
                        return file.LastWriteTime;
                    }
                }
            }
            catch
            {
                FileInfo file = new FileInfo(path);
                return file.LastWriteTime;
            }

            //try
            //{
            //    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            //    using (Image image = Image.FromStream(fs, false, false))
            //    {
            //        PropertyItem propItem = image.GetPropertyItem(36867);
            //        string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
            //        DateTime dateTime = DateTime.Parse(dateTaken);

            //        if (dateTime.Year > 1900 && dateTime.Year <= DateTime.Now.Year)
            //        {
            //            return dateTime;
            //        }
            //        else
            //        {
            //            FileInfo file = new FileInfo(path);
            //            return file.LastWriteTime;
            //        }
            //    }
            //}
            //catch
            //{
            //    FileInfo file = new FileInfo(path);
            //    return file.LastWriteTime;
            //}
        }

        private void HandleMinPhotosPerFolder(string folder, string destination)
        {
            DirectoryInfo info = new DirectoryInfo(folder);
            FileInfo[] files = info.GetFiles();

            if (files.Count() <= int.Parse(textBoxMinPhotosPerFolder.Text))
            {
                foreach (FileInfo item in files)
                {
                    string specialFolder = Path.Combine(destination, SpecialFolderName);

                    if (!Storage.Exists(specialFolder))
                    {
                        Directory.CreateDirectory(specialFolder);
                    }

                    File.Move(item.FullName, Path.Combine(specialFolder, item.Name));
                }

                Directory.Delete(folder);
            }
        }
    }
}