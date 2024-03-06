using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.Accounts
{
    public class EditModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;
        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();

        public bool CheckAccount = true;
        public string Message;
        public EditModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        [BindProperty]
        public Account Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (_httpContext.HttpContext!.Session.GetString("Account") != null)
            {
                Account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
                if (!Account.Role.ToLower().Equals("admin"))
                {
                    return Redirect("/Index");
                }
                else
                {

                    if (id == null || _context.Accounts == null)
                    {
                        return NotFound();
                    }

                    var account = await _context.Accounts.Where(a => a.AccountId == id).FirstOrDefaultAsync();
                    if (account == null)
                    {
                        return NotFound();
                    }
                    Account = account;
                    return Page();
                }
            }
            return Redirect("/Index");
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (AccountEmailExists(Account.AccountId,Account.Email))
            {
                CheckAccount = false;
                Message = "Email dose exists!";
                return Page();
            }

            if (AccountUseNameExists(Account.AccountId, Account.UserName))
            {
                CheckAccount = false;
                Message = "User Name dose exists!";
                return Page();
            }



            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(Account.AccountId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }

        private bool AccountUseNameExists(int id, string userName)
        {
            return (_context.Accounts?.Any(e => e.UserName == userName && e.AccountId != id)).GetValueOrDefault();
        }

        private bool AccountEmailExists(int id, string email)
        {
            return (_context.Accounts?.Any(e => e.Email == email && e.AccountId != id)).GetValueOrDefault();
        }
    }
}
