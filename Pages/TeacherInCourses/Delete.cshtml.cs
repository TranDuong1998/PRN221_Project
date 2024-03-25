using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.TeacherInCourses
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
        public TeacherDetail TeacherDetail { get; set; } = default!;

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
                    if (id == null || _context.TeacherDetails == null)
                    {
                        return NotFound();
                    }

                    var teacherdetail = await _context.TeacherDetails.FirstOrDefaultAsync(m => m.Id == id);

                    if (teacherdetail == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        TeacherDetail = teacherdetail;
                    }
                    return Page();
                }
            }
            return Redirect("/Index");

        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.TeacherDetails == null)
            {
                return NotFound();
            }
            var teacherdetail = await _context.TeacherDetails.FindAsync(id);

            if (teacherdetail != null)
            {
                TeacherDetail = teacherdetail;
                _context.TeacherDetails.Remove(TeacherDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
