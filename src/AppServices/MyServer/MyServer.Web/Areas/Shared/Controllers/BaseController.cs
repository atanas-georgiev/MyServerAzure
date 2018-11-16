namespace MyServer.Web.Areas.Shared.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using MyServer.Services.Users;

    public class BaseController : Controller
    {
        public BaseController(IUserService userService)
        {
            this.UserService = userService;
        }

        protected IUserService UserService { get; set; }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}