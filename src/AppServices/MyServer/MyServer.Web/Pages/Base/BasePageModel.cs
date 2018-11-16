using System.Linq;
using System.Net.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyServer.Data;
using MyServer.Data.Models;
using MyServer.Services.Users;

namespace MyServer.Web.Pages.Base
{
    public class BasePageModel : PageModel
    {
        public BasePageModel(
            IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            //this.UserService = userService;
            //this.userManager = userManager;
            //this.signInManager = signInManager;
            //this.dbContext = dbContext;

            //this.UserProfile =
            //    this.UserService.GetAll().FirstOrDefault(u => u.UserName == httpContextAccessor.HttpContext.User.Identity.Name);
        }

        // protected User UserProfile { get; private set; }

        protected IUserService UserService { get; set; }
    }
}
