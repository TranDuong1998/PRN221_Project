using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN211_Project.Services;

namespace PRN211_Project.Pages
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private IHttpContextAccessor _httpContext;

        public LogoutModel(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            _httpContext.HttpContext!.Session.Clear();

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            _httpContext.HttpContext!.Session.RemoveAccountSession("Account");
            _httpContext.HttpContext!.Session.RemoveTeacherSession("Teacher");
            return RedirectToPage("/Index");
        }
    }
}
