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

        public List<TeacherDetail> TeacherDetail { get; set; } = default!;

        public int? SubId { get; set; }
        public int? TeachId { get; set; }

        public string Url { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? teachId, int? subId)
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
                    if (id == null && teachId == null && subId == null)
                    {
                        return NotFound();
                    }

                    if (id != null)
                        Url = "./Index";
                    if (teachId != null)
                        Url = "/Teachers/Index";
                    if (subId != null)
                        Url = "/Courses/Index";

                    SubId = subId;
                    TeachId = teachId;

                    var teacherdetail = await _context.TeacherDetails.Include(t => t.Teacher)
                                                                     .Include(t => t.Course)
                                                                     .Where(m => (id == null ?
                                                                     (TeachId == null ? m.CourseId == SubId : m.TeacherId == TeachId)
                                                                     : m.Id == id))
                                                                     .ToListAsync();
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
    }
}
