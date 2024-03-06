using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();
        public IndexModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IList<Account> Account { get; set; } = default!;

        private int PageSize = 10;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public async Task<IActionResult> OnGetAsync(int pageIndex)
        {
            if (_httpContext.HttpContext!.Session.GetString("Account") != null)
            {
                var account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
                if (!account.Role.ToLower().Equals("admin"))
                {
                    return Redirect("/Index");
                }
                else
                {
                    TotalPages = (int)Math.Ceiling((double)_context.Accounts.Count() / PageSize);

                    CurrentPage = Math.Max(1, Math.Min(PageIndex, TotalPages));

                    Account = _context.Accounts.Where(a => a.AccountId != 1)
                                               .Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                    return Page();
                }
            }
            else
                return Redirect("/Index");

        }
    }
}
