using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyServer.Web.Pages
{
    using MyServer.Data.Models;
    using MyServer.Services.Users;

    public class IndexModel : PageModel
    {
        private IUserService us;

        public IndexModel(IUserService us)
        {
            this.us = us;
        }

        public async Task OnGet()
        {
           // await this.us.UpdateAsync("e94948de-909f-4f68-9638-d590b5017dc8", true);
        }
    }
}
