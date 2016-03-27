using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Photo_Organizer
{
    public partial class Form1 : Form
    {
        private static Regex r = new Regex(":");

        public Form1()
        {
            InitializeComponent();
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

            if (Check(source, destination))
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
                string pattern = "yyyy.MM.dd";

                string currentPath = null;
                DateTime dateTaken = new DateTime(1,1,1);

                // Move to folders
                foreach (Photo x in photoArray)
                {
                    // If targetPath is null, it means that is the first photo so we need to create a folder for it
                    if (string.IsNullOrWhiteSpace(currentPath))
                    {
                        currentPath = Path.Combine(destination, x.DateTaken.ToString(pattern));

                        Directory.CreateDirectory(currentPath);
                        File.Copy(x.Path, Path.Combine(currentPath, x.Name), true);
                    }
                    else
                    {
                        string path = Path.Combine(destination, x.DateTaken.ToString(pattern));

                        if (Directory.Exists(path))
                        {
                            // If the directory already exists, it means that is a photo from an already created day
                            File.Copy(x.Path, Path.Combine(path, x.Name), true);
                        }
                        else
                        {
                            // if the directory doesn`t exists, we need to check if we need to combine the folder days or create a new one
                            int totalDays = (int)Math.Round((x.DateTaken - dateTaken).TotalDays);
                            int minDays = int.Parse(this.textBoxMinPhotosDays.Text);

                            if (totalDays <= minDays)
                            {
                                File.Copy(x.Path, Path.Combine(currentPath, x.Name), true);
                            }
                            else
                            {
                                Directory.CreateDirectory(path);
                                File.Copy(x.Path, Path.Combine(path, x.Name), true);

                                // Update path
                                currentPath = path;
                            }
                        }
                    }

                    // Update last item date
                    dateTaken = x.DateTaken;
                }

                // Move files from folders that have small items
                string miscFolder = Path.Combine(destination, "misc");

                foreach (var directory in Directory.GetDirectories(destination))
                {
                    if (Directory.GetFileSystemEntries(directory).Length <= int.Parse(this.textBoxMinPhotosPerFolder.Text))
                    {
                        Move(directory, miscFolder);
                    }
                }

                // Delete empty folders
                cleanEmptyFolders(destination);
            }
            else
            {
                MessageBox.Show("Error");
            }

            var sourceCount = (from file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories)  select file).Count();
            var destinationCount = (from file in Directory.EnumerateFiles(destination, "*", SearchOption.AllDirectories) select file).Count();

            watch.Stop();

            Cursor.Current = Cursors.Default;

            MessageBox.Show(string.Format("Source: {0}\nDestination: {1}\nTotal time: {2}", sourceCount, destinationCount, watch.Elapsed));

            watch.Reset();
        }

        private static void Move(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            MoveAll(diSource, diTarget);
        }

        private static void MoveAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.MoveTo(Path.Combine(target.FullName, fi.Name));
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                MoveAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private bool Check(string source, string destination)
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

            if (IsDirectoryEmpty(source))
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

            if (!IsDirectoryEmpty(destination))
            {
                return false;
            }

            if (IsSubfolder(source, destination))
            {
                return false;
            }

            return true;
        }
        private void cleanEmptyFolders(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                cleanEmptyFolders(directory);

                if (Directory.GetFileSystemEntries(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        private bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private bool IsSubfolder(string source, string destination)
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

        public static DateTime GetDateTakenFromImage(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (Image image = Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = image.GetPropertyItem(36867);
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                }
            }
            catch
            {
                return File.GetCreationTime(path);
            }
        }
    }
}
