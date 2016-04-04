using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Photo_Organizer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string SpecialFolderName = "misc";
        private int textBoxMinPhotosDays = 2;
        private int textBoxMinPhotosPerFolder = 7;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async Task<StorageFolder> PickSingleFolderAsync()
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add(".jpg");
            folderPicker.FileTypeFilter.Add(".jpeg");
            folderPicker.FileTypeFilter.Add(".png");

            return await folderPicker.PickSingleFolderAsync();
        }

        private async void startProcess_Click(object sender, RoutedEventArgs e)
        {
            var watchTotal = new System.Diagnostics.Stopwatch();
            watchTotal.Start();

            StorageFolder source = await PickSingleFolderAsync();
            StorageFolder destination = await PickSingleFolderAsync();

            if (await ValidateInput(source, destination))
            {
                //// Initialize queryOptions using a common query
                QueryOptions queryOptions = new QueryOptions(CommonFolderQuery.DefaultQuery);

                // Clear all existing sorts
                queryOptions.SortOrder.Clear();

                // add descending sort by date modified
                //SortEntry se = new SortEntry();
                //se.PropertyName = "System.DateModified";
                //se.AscendingOrder = true;
                //queryOptions.SortOrder.Add(se);
                //queryOptions.ApplicationSearchFilter = "System.Photo.DateTaken:> System.StructuredQueryType.DateTime";

                // Get the files in the current folder
                StorageFileQueryResult queryResult = source.CreateFileQueryWithOptions(queryOptions);
                IReadOnlyList<StorageFile> files = await queryResult.GetFilesAsync();

                var watchGetFilesAsync = new System.Diagnostics.Stopwatch();
                watchGetFilesAsync.Start();

                watchGetFilesAsync.Stop();

                List<Item> items = new List<Item>();

                var watchGetDateTakenFromImage = new System.Diagnostics.Stopwatch();
                watchGetDateTakenFromImage.Start();
                foreach (StorageFile file in files)
                {
                    Item item = new Item();
                    item.File = file;
                    item.DateTaken = await GetDateTakenFromImage(file);

                    items.Add(item);
                }
                watchGetDateTakenFromImage.Stop();

                var watchOrderBy = new System.Diagnostics.Stopwatch();
                watchOrderBy.Start();
                Item[] itemArray = items.OrderBy(p => p.DateTaken).ToArray();
                watchOrderBy.Stop();

                string pattern = "yyyy.MM.dd";
                DateTime currentDateTaken = new DateTime(1, 1, 1);

                StorageFolder currentFolder = null;

                var watchCore = new System.Diagnostics.Stopwatch();
                watchCore.Start();
                foreach (Item item in itemArray)
                {
                    DateTime dateTaken = item.DateTaken;

                    // If currentFolder is null, it means that is the first file so we need to create a folder for it anyway
                    if (currentFolder == null)
                    {
                        currentFolder = await CreateFolderAsync(destination, dateTaken.ToString(pattern));
                        await CopyFileAsync(item.File, currentFolder);
                    }
                    else
                    {
                        if (await Exists(destination, dateTaken.ToString(pattern)))
                        {
                            // If the directory already exists, it means that is the current folder
                            await CopyFileAsync(item.File, currentFolder);
                        }
                        else
                        {
                            // if the directory doesn`t exists, we need to check if we need to combine the folder days or create a new one
                            int totalDays = (int)Math.Round((dateTaken - currentDateTaken).TotalDays);
                            int minDays = this.textBoxMinPhotosDays;

                            if (totalDays <= minDays)
                            {
                                await CopyFileAsync(item.File, currentFolder);
                            }
                            else
                            {
                                // At this point, we need to create a new directory to keep the incoming files.
                                // But, before changing the current folder, let`s check if it meets the MinPhotosPerFolder criteria
                                //await HandleMinPhotosPerFolder(currentFolder, destination);

                                currentFolder = await CreateFolderAsync(destination, dateTaken.ToString(pattern));
                                await CopyFileAsync(item.File, currentFolder);
                            }
                        }
                    }

                    // Update last item date
                    currentDateTaken = dateTaken;
                }

                watchCore.Stop();

                // And now that we finish, let`s check if the latest folder meets the MinPhotosPerFolder criteria
                //await HandleMinPhotosPerFolder(currentFolder, destination);

                watchTotal.Stop();

                var dialog2 = new Windows.UI.Popups.MessageDialog(string.Format("watchTotal: {0}\nwatchGetFilesAsync: {1}\nwatchGetDateTakenFromImage: {2}\nwatchOrderBy: {3}\nwatchCore: {4}", watchTotal.Elapsed, watchGetFilesAsync.Elapsed, watchGetDateTakenFromImage.Elapsed, watchOrderBy.Elapsed, watchCore.Elapsed));
                await dialog2.ShowAsync();

                watchTotal.Reset();
                watchGetFilesAsync.Reset();
                watchGetDateTakenFromImage.Reset();
                watchOrderBy.Reset();
                watchCore.Reset();
            }
            else
            {
                var dialog1 = new Windows.UI.Popups.MessageDialog("Error", "Error");
                await dialog1.ShowAsync();
            }

            //var sourceCount = (from file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories) select file).Count();
            //var destinationCount = (from file in Directory.EnumerateFiles(destination, "*", SearchOption.AllDirectories) select file).Count();

            //var dialog2 = new Windows.UI.Popups.MessageDialog(string.Format("Source: {0}\nDestination: {1}\nTotal time: {2}", 0, 0, watchTotal.Elapsed));
        }

        private async Task<StorageFile> CopyFileAsync(StorageFile file, StorageFolder folder)
        {
            return null;
            //return await file.CopyAsync(folder);
        }

        private async Task<StorageFolder> CreateFolderAsync(StorageFolder folder, string name)
        {
            return await folder.CreateFolderAsync(name);
        } 

        private async Task<bool> HandleMinPhotosPerFolder(StorageFolder folder, StorageFolder destination)
        {
            IReadOnlyList<StorageFile> items = await folder.GetFilesAsync();

            if (items.Count <= textBoxMinPhotosPerFolder)
            {
                foreach (StorageFile item in items)
                {
                    StorageFolder specialFolder = null;

                    if (!await Exists(destination, SpecialFolderName))
                    {
                        specialFolder = await destination.CreateFolderAsync(SpecialFolderName);
                    }
                    else
                    {
                        specialFolder = await destination.GetFolderAsync(SpecialFolderName);
                    }

                    await item.MoveAsync(specialFolder);
                }

                await folder.DeleteAsync();
            }

            return true;
        }

        private async Task<bool> Exists(StorageFolder folder, string name)
        {
            try
            {
                await folder.GetFolderAsync(name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> IsEmpty(StorageFolder directory)
        {
            var items = await directory.GetItemsAsync(0, 1);
            return items.Count == 0;
        }

        private async Task<bool> ValidateInput(StorageFolder source, StorageFolder destination)
        {
            // Source
            if (await IsEmpty(source))
            {
                return false;
            }

            // Destination
            if (!await IsEmpty(destination))
            {
                return false;
            }

            if (IsSubfolder(source.Path, destination.Path))
            {
                return false;
            }

            return true;
        }

        private bool IsSubfolder(string source, string destination)
        {
            string cp = Path.GetFullPath(destination);
            string pp = Path.GetFullPath(source);

            return pp.StartsWith(cp);
        }

        public async Task<DateTime> GetDateTakenFromImage(StorageFile file)
        {
            try
            {
                ImageProperties props = await file.Properties.GetImagePropertiesAsync();
                DateTime dateTime = props.DateTaken.Date;

                if (dateTime.Year > 1900 && dateTime.Year <= DateTime.Now.Year)
                {
                    return dateTime;
                }
                else
                {
                    var properties = await file.GetBasicPropertiesAsync();
                    return DateTime.Parse(properties.DateModified.ToString());
                }
            }
            catch
            {
                var properties = await file.GetBasicPropertiesAsync();
                return DateTime.Parse(properties.DateModified.ToString());
            }
        }
    }
}