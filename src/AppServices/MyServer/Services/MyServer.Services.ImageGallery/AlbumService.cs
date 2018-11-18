namespace MyServer.Services.ImageGallery
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.WindowsAzure.Storage.Table;

    using MyServer.Common.ImageGallery;
    using MyServer.Data.Common;
    using MyServer.Data.Models;

    public class AlbumService : IAlbumService
    {
        private readonly IRepository<Album> albums;

        private readonly IFileService fileService;

        private readonly IRepository<Image> images;

        private readonly IMemoryCache memoryCache;

        public AlbumService(
            IRepository<Album> albums,
            IRepository<Image> images,
            IFileService fileService,
            IMemoryCache memoryCache)
        {
            this.albums = albums;
            this.images = images;
            this.fileService = fileService;;
            this.memoryCache = memoryCache;
        }

        public void Add(Album album)
        {
            var noCoverImageGuid = Guid.Parse(Constants.NoCoverId);
            //album.Cover = this.images.GetById(noCoverImageGuid);
            //this.albums.Add(album);
            //this.fileService.CreateInitialFolders(album.Id);
            this.memoryCache.Remove(CacheKeys.AlbumsServiceCacheKey);
            this.memoryCache.Remove(CacheKeys.ImageServiceCacheKey);
            this.memoryCache.Remove(CacheKeys.FileServiceCacheKey);
        }

        public string GenerateZipArchive(Guid id, ImageType type)
        {
            //var number = 0;
            //var album = this.GetById(id);
            ////var files = album.Images.OrderBy(x => x.OriginalFileName)
            //    .Select(x => new { id = number++, FileName = x.FileName, OriginalFileName = x.OriginalFileName })
            //    .ToList();
            //var duplicates = files.GroupBy(s => s.OriginalFileName).SelectMany(grp => grp.Skip(1)).ToList();
            //string albumPath = string.Empty;

            //switch (type)
            //{
            //    case ImageType.Low:
            //        albumPath = this.appEnvironment.WebRootPath + Constants.MainContentFolder + "\\" + id + "\\"
            //                    + Constants.ImageFolderLow + "\\";
            //        break;
            //    case ImageType.Medium:
            //        albumPath = this.appEnvironment.WebRootPath + Constants.MainContentFolder + "\\" + id + "\\"
            //                    + Constants.ImageFolderMiddle + "\\";
            //        break;
            //    case ImageType.Original:
            //        albumPath = this.appEnvironment.WebRootPath + Constants.MainContentFolder + "\\" + id + "\\"
            //                    + Constants.ImageFolderOriginal + "\\";
            //        break;
            //}

            //var albumPathTemp = this.appEnvironment.WebRootPath + Constants.TempContentFolder + "\\" + id + "\\";

            //this.fileService.EmptyTempFolder();
            //Directory.CreateDirectory(this.appEnvironment.WebRootPath + Constants.TempContentFolder + "\\" + id);

            //foreach (var file in files)
            //{
            //    if (!duplicates.Exists(x => x.OriginalFileName == file.OriginalFileName))
            //    {
            //        File.Copy(albumPath + file.FileName, albumPathTemp + file.OriginalFileName);
            //    }
            //    else
            //    {
            //        var newFileName = Path.GetFileNameWithoutExtension(file.OriginalFileName) + "_" + file.id
            //                          + Path.GetExtension(file.OriginalFileName);
            //        File.Copy(albumPath + file.FileName, albumPathTemp + newFileName);
            //    }
            //}

            //// TODO: change en/bg
            //var archiveName = this.appEnvironment.WebRootPath + Constants.TempContentFolder + "\\"
            //                  + this.fileService.MakeValidFileName(album.TitleEn) + ".zip";
            //File.Delete(archiveName);
            //ZipFile.CreateFromDirectory(albumPathTemp, archiveName);

            //return archiveName.Replace(
            //    this.appEnvironment.WebRootPath + Constants.TempContentFolder,
            //    Constants.TempContentFolder);

            return null;
        }

        public Album GetById(Guid id, bool cache = true)
        {
            return null;//this.GetAllReqursive(cache).FirstOrDefault(x => x.Id == id);
        }

        public void Remove(Guid id)
        {
            //var album = this.GetById(id, false);

            //if (album != null)
            //{
            //    if (album.Images != null)
            //    {
            //        foreach (var image in album.Images.ToList())
            //        {
            //            this.images.Delete(image.Id);
            //        }
            //    }

            //    this.albums.Delete(id);
            //    this.fileService.RemoveAlbum(id);

            //    this.memoryCache.Remove(CacheKeys.AlbumsServiceCacheKey);
            //    this.memoryCache.Remove(CacheKeys.ImageServiceCacheKey);
            //    this.memoryCache.Remove(CacheKeys.FileServiceCacheKey);
            //}
        }

        public void Update(Album album)
        {
            //this.albums.Update(album);
            //this.memoryCache.Remove(CacheKeys.AlbumsServiceCacheKey);
            //this.memoryCache.Remove(CacheKeys.ImageServiceCacheKey);
            //this.memoryCache.Remove(CacheKeys.FileServiceCacheKey);
        }

        public void UpdateCoverImage(string album, string image)
        {
            //var albumDb = this.GetById(album, false);
            //albumDb.CoverId = image;
            //this.Update(albumDb);
            //this.memoryCache.Remove(CacheKeys.AlbumsServiceCacheKey);
            //this.memoryCache.Remove(CacheKeys.ImageServiceCacheKey);
            //this.memoryCache.Remove(CacheKeys.FileServiceCacheKey);
        }

        public async Task AddAsync(Album album)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateZipArchiveAsync(string id, ImageType type)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Album>> GetAllReqursiveAsync(bool cache = true)
        {
            var firstAlbumToExcludeGuid = Guid.Parse(Constants.NoCoverId).ToString();

            if (cache)
            {
                IEnumerable<Album> result = null;

                if (!this.memoryCache.TryGetValue(CacheKeys.AlbumsServiceCacheKey, out result))
                {
                    // fetch the value from the source
                    result = (await this.albums.GetAllAsync()).Where(x => x.RowKey != firstAlbumToExcludeGuid).ToList().AsQueryable();

                    //result = this.albums.All().Include(x => x.Cover).Include(x => x.Images)
                    //    .ThenInclude(x => x.ImageGpsData)
                    //    .Where(x => x.IsDeleted == false && x.Id != firstAlbumToExcludeGuid).ToList().AsQueryable();

                    // store in the cache
                    this.memoryCache.Set(
                        CacheKeys.AlbumsServiceCacheKey,
                        result,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(365)));
                }

                return result;
            }

            return (await this.albums.GetAllAsync()).Where(x => x.RowKey != firstAlbumToExcludeGuid).ToList().AsQueryable();
        }

        public async Task<Album> GetByIdAsync(string id, bool cache = true)
        {
            var filter = TableQuery
                .GenerateFilterCondition("RowKey", QueryComparisons.Equal, id);
            var result = await this.albums.QueryAsync(new TableQuery<Album>().Where(filter));
            return result.FirstOrDefault();
        }

        public async Task RemoveAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Album album)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCoverImageAsync(string albumId, string imageId)
        {
            throw new NotImplementedException();
        }
    }
}