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
    public class DetailsModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;
        private readonly AccountServices _accountServices = new AccountServices();
        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();

        public DetailsModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public Account Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (_httpContext.HttpContext!.Session.GetString("Account") == null)
            {
                return Redirect("/Index");
            }
            else
            {
                if (id == null || _context.Accounts == null)
                {
                    return NotFound();
                }

                var account = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountId == id);
                if (account == null)
                {
                    return NotFound();
                }
                else
                {
                    Account = account;
                }
                return Page();
            }
        }
    }
}
