using MyServer.Web.Pages.UsersAdmin;

namespace MyServer.Web.Areas.UsersAdmin.Controllers
{
    using System.Threading.Tasks;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using MyServer.Common;
    using MyServer.Data;
    using MyServer.Data.Models;
    using MyServer.Services.Mappings;
    using MyServer.Services.Users;
    using MyServer.Web.Areas.Shared.Controllers;

    [Authorize(Roles = "Admin")]
    [Area("UsersAdmin")]
    public class JsonController : BaseController
    {
        public JsonController(IUserService userService) : base(userService)
        {
        }
        
        public async Task<ActionResult> UsersRead([DataSourceRequest] DataSourceRequest request)
        {
            var users = await this.UserService.GetAllAsync();
            return this.Json(users.To<UsersViewModel>().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<ActionResult> UsersUpdate([DataSourceRequest] DataSourceRequest request, UsersViewModel user)
        {
            if (user != null && this.ModelState.IsValid)
            {
                await this.UserService.UpdateAsync(user.RowKey, user.Role == MyServerRoles.Admin);
            }

            return this.Json(new[] { user }.ToDataSourceResult(request, this.ModelState));
        }
    }
}