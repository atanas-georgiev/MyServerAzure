namespace MyServer.Services.ImageGallery
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using MyServer.Common;
    using MyServer.Data.Models;

    public interface IImageService
    {
        void Add(Guid albumId, string userId, Stream fileStream, string fileName);

        // void AddGpsDataToImage(Guid imageId, ImageGpsData gpsData);

        Task<IEnumerable<Image>> GetAllAsync(bool cache = true);

        Task<IEnumerable<Image>> GetAllFromAlbumAsync(string albumId, bool cache = true);

        Task<Image> GetByIdAsync(string id, bool cache = true);

        string GetRandomImagePath();

        void PrepareFileForDownload(Guid id);

        void Remove(Guid id);

        void Rotate(Guid imageId, MyServerRotateType rotateType);

        void Update(Image image);

        void UpdateDateTaken(Guid imageId, DateTime date);
    }
}