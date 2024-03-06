using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN211_Project.Models;
using PRN211_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using PRN211_Project.Entities;
using Microsoft.AspNetCore.Http;
using PRN211_Project.Services;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace PRN211_Project.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private IHttpContextAccessor _httpContext;
        private readonly Prn211ProjectContext _context;
        public LoginModel(UserManager<User> userManager, SignInManager<User> signInManager,
            ILogger<LoginModel> logger, IHttpContextAccessor httpContext, Prn211ProjectContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _httpContext = httpContext;
            _context = context;
        }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        public string Message;

        public bool CheckAccount = true;

        [TempData]
        public string ErrorMessage { get; set; }

        [Required]
        [EmailAddress]
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [BindProperty(SupportsGet = true)]
        public string Password { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                return Page();
            }

            Account account = await _context.Accounts.Where(a => (a.Email.Equals(Email) && a.Password.Equals(Password)) ||
                                                       (a.UserName.Equals(Email) && a.Password.Equals(Password)))
                                                     .FirstOrDefaultAsync();
            if (account != null)
            {
                if (account.Active == true)
                {

                    _httpContext.HttpContext.Session.SetObject("Account", account);
                    if (account.Role == "teacher")
                    {
                        var teacher = _context.Teachers.Include(t => t.Account).Where(t => t.AccountId == account.AccountId).FirstOrDefault();
                        _httpContext.HttpContext.Session.SetObject("Teacher", teacher);
                    }
                    else if (account.Role == "student" && account.Active == true)
                    {
                        //var student = _context.Teachers.Include(t => t.Account).Where(t => t.AccountId == account.AccountId).FirstOrDefault();
                        //_httpContext.HttpContext.Session.SetObject("Account", account);
                    }
                    return RedirectToPage("/index");
                }
                else
                {
                    CheckAccount = false;
                    Message = "Account is inactive! Please choose other account.";
                    return Page();
                }
            }
            else
            {
                CheckAccount = false;
                Message = "Account does not exist! Please check your login information again.";
                return Page();
            }

        }

    }
}
