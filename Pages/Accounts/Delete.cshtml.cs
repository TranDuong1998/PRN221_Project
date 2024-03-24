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
    public class DeleteModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;
        private readonly AccountServices _accountServices = new AccountServices();
        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();
        public DeleteModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
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
                var account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
                if (!account.Role.ToLower().Equals("admin"))
                {
                    return Redirect("/Index");
                }
                else
                {
                    if (id == null || _context.Accounts == null)
                    {
                        return NotFound();
                    }

                    account = _accountServices.GetAcountByID(id);

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
            return Redirect("/Index");
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }
            var account = _accountServices.GetAcountByID(id);

            if (account != null)
            {
                Account = account;
                var teacher = _context.Teachers.Where(t => t.AccountId == id).FirstOrDefault();
                if (teacher != null)
                {
                    var tc = _context.TeacherClasses.Where(t => t.TeacherId == teacher.TeacherId).ToList();
                    var td = _context.TeacherDetails.Where(t => t.TeacherId == teacher.TeacherId).ToList();
                    var w = _context.WeeklyTimeTables.Where(t => t.TeachersId == teacher.TeacherId).ToList();

                    _context.TeacherClasses.RemoveRange(tc);
                    _context.TeacherDetails.RemoveRange(td);
                    _context.WeeklyTimeTables.RemoveRange(w);
                    await _context.SaveChangesAsync();
                    _context.Teachers.Remove(teacher);
                    _context.SaveChanges();
                }
                if (_accountServices.DeleteAccount(account) > 0)
                    return RedirectToPage("./Index");
                else
                    return NotFound();
            }
            return RedirectToPage("./Index");
        }
    }
}
