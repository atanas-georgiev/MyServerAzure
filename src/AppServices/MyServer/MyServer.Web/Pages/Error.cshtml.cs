namespace MyServer.Web.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class ErrorModel : PageModel
    {
        public new int? StatusCode = 0;

        public void OnGet(int? statusCode)
        {
            this.StatusCode = statusCode;
        }
    }
}