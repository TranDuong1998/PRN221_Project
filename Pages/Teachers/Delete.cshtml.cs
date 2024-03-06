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
    public class DeleteModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();
        public DeleteModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (_httpContext.HttpContext!.Session.GetString("Account") != null)
            {
                var Account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
                if (!Account.Role.ToLower().Equals("admin"))
                {
                    return Redirect("/Index");
                }
                else
                {
                    if (id == null || _context.Teachers == null)
                    {
                        return NotFound();
                    }

                    var teacher = await _context.Teachers.FirstOrDefaultAsync(m => m.TeacherId == id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher != null)
            {
                Teacher = teacher;
                _context.Teachers.Remove(Teacher);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
