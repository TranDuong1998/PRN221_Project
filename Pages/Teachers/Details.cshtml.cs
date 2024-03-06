using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.Teachers
{
    public class DetailsModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();

        public DetailsModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public Account Account;

        public string Title { get; set; }
        public Teacher Teacher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (_httpContext.HttpContext!.Session.GetString("Account") != null)
            {
                var account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
                if (!account.Role.ToLower().Equals("admin") && !account.Role.ToLower().Equals("teacher"))
                {
                    return Redirect("/Index");
                }
                else
                {
                    if (account.Role.ToLower().Equals("admin"))
                        Title = "Teacher Management";
                    else
                        Title = "Teacher Details";
                    Account = account;
                    if (id == null || _context.Teachers == null)
                    {
                        return NotFound();
                    }

                    var teacher = await _context.Teachers.Include(t => t.Account).FirstOrDefaultAsync(m => m.TeacherId == id);
                    if (teacher == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        Teacher = teacher;
                    }
                    return Page();
                }
            }
            return Redirect("/Index");
        }
    }
}
