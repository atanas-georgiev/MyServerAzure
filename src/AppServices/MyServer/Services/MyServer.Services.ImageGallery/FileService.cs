namespace MyServer.Services.ImageGallery
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using MyServer.Common.ImageGallery;

    public class FileService : IFileService
    {
        private readonly IMemoryCache memoryCache;

        private readonly IConfiguration configuration;

        private CloudBlobContainer imagesBlobContainer;

        public FileService(IMemoryCache memoryCache, IConfiguration configuration)
        {
            this.memoryCache = memoryCache;
            this.configuration = configuration;
            var storageAccount = CloudStorageAccount.Parse(configuration["TableConnectionString"]);
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            this.imagesBlobContainer = cloudBlobClient.GetContainerReference("images");
        }

        public void EmptyTempFolder()
        {
            //var di = new DirectoryInfo(this.appEnvironment.WebRootPath + Constants.TempContentFolder);

            //foreach (var dir in di.GetDirectories())
            //{
            //    foreach (var file in dir.GetFiles())
            //    {
            //        file.Delete();
            //    }

            //    dir.Delete(true);
            //}
        }

        public string GetImageFolder(Guid albumId, ImageType type)
        {
            //var contentFolder = this.appEnvironment.WebRootPath + Constants.MainContentFolder;

            //switch (type)
            //{
            //    case ImageType.Low: return contentFolder + "\\" + albumId + "\\" + Constants.ImageFolderLow + "\\";
            //    case ImageType.Medium:
            //        return contentFolder + "\\" + albumId + "\\" + Constants.ImageFolderMiddle + "\\";
            //    case ImageType.Original:
            //        return contentFolder + "\\" + albumId + "\\" + Constants.ImageFolderOriginal + "\\";
            //}

            return string.Empty;
        }

        public string GetImageFolderSize()
        {
            string result = null;

            //if (!this.memoryCache.TryGetValue(CacheKeys.FileServiceCacheKey, out result))
            //{
            //    // fetch the value from the source
            //    var dir = this.appEnvironment.WebRootPath + Constants.MainContentFolder;
            //    var size = GetDirectorySize(dir);
            //    result = (size / 1024 / 1024 / 1024).ToString("0.00");

            //    // store in the cache
            //    this.memoryCache.Set(
            //        CacheKeys.FileServiceCacheKey,
            //        result,
            //        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(365)));
            //}

            return result;
        }

        public string MakeValidFileName(string name)
        {
            //var invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            //var invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            //return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
            return null;
        }

        public void RemoveAlbum(Guid albumId)
        {
            //this.RemoveFolder(this.appEnvironment.WebRootPath + Constants.MainContentFolder + "/" + albumId);
            //Directory.Delete(this.appEnvironment.WebRootPath + Constants.MainContentFolder + "/" + albumId);
        }

        public void RemoveImage(Guid albumId, string fileName)
        {
            //File.Delete(
            //    this.appEnvironment.WebRootPath + Constants.MainContentFolder + "/" + albumId + "/"
            //    + Constants.ImageFolderLow + "/" + fileName);
            //File.Delete(
            //    this.appEnvironment.WebRootPath + Constants.MainContentFolder + "/" + albumId + "/"
            //    + Constants.ImageFolderMiddle + "/" + fileName);
            //File.Delete(
            //    this.appEnvironment.WebRootPath + Constants.MainContentFolder + "/" + albumId + "/"
            //    + Constants.ImageFolderOriginal + "/" + fileName);
        }

        public void Save(Stream inputStream, ImageType type, string originalFilename, Guid albumId)
        {
            using (var fileStream = File.Create(this.GetImageFolder(albumId, type) + originalFilename))
            {
                inputStream.Seek(0, SeekOrigin.Begin);
                inputStream.CopyTo(fileStream);
            }
        }

        private static decimal GetDirectorySize(string location)
        {
            try
            {
                var files = new DirectoryInfo(location).GetFiles("*.jpg", SearchOption.AllDirectories);
                return files.Sum(file => file.Length);
            }
            catch (DirectoryNotFoundException)
            {
                return 0;
            }
        }

        private void RemoveFolder(string folder)
        {
            var dir = new DirectoryInfo(folder);

            foreach (var fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (var di in dir.GetDirectories())
            {
                this.RemoveFolder(di.FullName);
                di.Delete();
            }
        }

        public async Task CreateInitialFoldersAsync(Guid albumId)
        {
            //CloudBlockBlob cloudBlockBlob = imagesBlobContainer.GetBlockBlobReference() (localFileName);
            //this.imagesBlobContainer;

            //    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);
            //await cloudBlockBlob.UploadFromFileAsync(sourceFile);

            //var contentFolder = this.appEnvironment.WebRootPath + Constants.MainContentFolder;

            //if (!Directory.Exists(contentFolder))
            //{
            //    Directory.CreateDirectory(contentFolder);
            //}

            //if (!Directory.Exists(this.appEnvironment.WebRootPath + Constants.TempContentFolder))
            //{
            //    Directory.CreateDirectory(this.appEnvironment.WebRootPath + Constants.TempContentFolder);
            //}

            //if (!Directory.Exists(this.appEnvironment.WebRootPath + Constants.MainContentFolder + "\\" + albumId))
            //{
            //    Directory.CreateDirectory(
            //        this.appEnvironment.WebRootPath + Constants.MainContentFolder + "\\" + albumId);
            //}

            //if (!Directory.Exists(contentFolder + "\\" + albumId + "\\" + Constants.ImageFolderOriginal))
            //{
            //    Directory.CreateDirectory(contentFolder + "\\" + albumId + "\\" + Constants.ImageFolderOriginal);
            //}

            //if (!Directory.Exists(contentFolder + "\\" + albumId + "\\" + Constants.ImageFolderMiddle))
            //{
            //    Directory.CreateDirectory(contentFolder + "\\" + albumId + "\\" + Constants.ImageFolderMiddle);
            //}

            //if (!Directory.Exists(contentFolder + "\\" + albumId + "\\" + Constants.ImageFolderLow))
            //{
            //    Directory.CreateDirectory(contentFolder + "\\" + albumId + "\\" + Constants.ImageFolderLow);
            //}
        }

        public async Task EmptyTempFolderAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetImageFolderAsync(Guid albumId, ImageType type)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetImageFolderSizeAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> MakeValidFileNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAlbumAsync(Guid albumId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveImageAsync(Guid albumId, string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync(Stream inputStream, ImageType type, string originalFilename, Guid albumId)
        {
            throw new NotImplementedException();
        }
    }
}