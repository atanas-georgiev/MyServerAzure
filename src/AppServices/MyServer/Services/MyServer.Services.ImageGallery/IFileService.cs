namespace MyServer.Services.ImageGallery
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using MyServer.Common.ImageGallery;

    public interface IFileService
    {
        Task CreateInitialFoldersAsync(Guid albumId);

        Task EmptyTempFolderAsync();

        Task<string> GetImageFolderAsync(Guid albumId, ImageType type);

        Task<string> GetImageFolderSizeAsync();

        Task<string> MakeValidFileNameAsync(string name);

        Task RemoveAlbumAsync(Guid albumId);

        Task RemoveImageAsync(Guid albumId, string fileName);

        Task SaveAsync(Stream inputStream, ImageType type, string originalFilename, Guid albumId);
    }
}