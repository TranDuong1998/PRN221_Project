using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.Courses
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

        public Course Course { get; set; } = default!; 

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
                    if (id == null || _context.Courses == null)
                    {
                        return NotFound();
                    }

                    var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == id);
                    if (course == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        Course = course;
                    }
                    return Page();
                }
            }
            return Redirect("/Index");
        }
    }
}
