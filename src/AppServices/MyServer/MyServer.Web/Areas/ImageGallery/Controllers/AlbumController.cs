namespace MyServer.Web.Areas.ImageGallery.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using MyServer.Common.ImageGallery;
    using MyServer.Data;
    using MyServer.Data.Models;
    using MyServer.Services.ImageGallery;
    using MyServer.Services.Mappings;
    using MyServer.Services.Users;
    using MyServer.Web.Areas.ImageGallery.Models.Album;
    using MyServer.Web.Areas.Shared.Controllers;
    using Shared.Models;

    [Area("ImageGallery")]
    public class AlbumController : BaseController
    {
        private readonly IAlbumService albumService;

        public AlbumController(
            IUserService userService,
            IAlbumService albumService)
            : base(userService)
        {
            this.albumService = albumService;
        }

        public async Task<IActionResult> Details(string id)
        {
            //if (id == "FinishRender")
            //{
            //    var album2 =
            //        this.albumService.GetAllReqursive()
            //            .To<AlbumViewModel>()
            //            .FirstOrDefault();

            //    return this.ViewComponent("ImageList", new { albumId = album2.Id });
            //}

            var albumDb = new List<Album>() { await this.albumService.GetByIdAsync(id) };
            var album = albumDb.To<AlbumViewModel>().FirstOrDefault();
            return this.View(album);
        }

        public async Task<IActionResult> Download(string id, ImageType type)
        {
            //var zip = this.albumService.GenerateZipArchive(Guid.Parse(id), type);
            //return this.Content(zip.Replace("~", string.Empty));
            return null;
        }

        public async Task<IActionResult> Index()
        {
            return this.View(new SortFilterAlbumViewModel() { SearchString = "", SortType = Common.MyServerSortType.SortImagesDateDesc });
        }

        [HttpPost]
        public async Task<IActionResult> SortFilter(SortFilterAlbumViewModel model)
        {
            return this.ViewComponent("AllAlbums", new { ViewDetailsUrl = "Album/Details/", Filter = model.SearchString, Sort = model.SortType });
        }

        public async Task<IActionResult> FinishRender()
        {
            var album = (await this.albumService.GetAllReqursiveAsync())
                    .To<AlbumViewModel>()
                    .FirstOrDefault();

            return this.ViewComponent("ImageList", new { albumId = album.RowKey });
        }
    }
}