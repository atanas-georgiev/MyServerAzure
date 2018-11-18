using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyServer.Web.Pages
{
    using MyServer.Data.Models;
    using MyServer.Services.ImageGallery;
    using MyServer.Services.Users;

    public class IndexModel : PageModel
    {
        private IUserService us;

        private IAlbumService as1;

        public IndexModel(IUserService us, IAlbumService as1)
        {
            this.us = us;
            this.as1 = as1;
        }

        public async Task OnGet()
        {
            var res = await this.as1.GetAllReqursiveAsync(false);
            // await this.us.UpdateAsync("e94948de-909f-4f68-9638-d590b5017dc8", true);
        }
    }
}
