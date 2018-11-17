namespace MyServer.Services.ImageGallery
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MyServer.Common.ImageGallery;
    using MyServer.Data.Models;

    public interface IAlbumService
    {
        Task AddAsync(Album album);

        Task<string> GenerateZipArchiveAsync(string id, ImageType type);

        Task<IEnumerable<Album>> GetAllReqursiveAsync(bool cache = true);

        Task<Album> GetByIdAsync(string id, bool cache = true);

        Task RemoveAsync(string id);

        Task UpdateAsync(Album album);

        Task UpdateCoverImageAsync(string albumId, string imageId);
    }
}