using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.TeacherInClass
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

        public List<TeacherClass> TeacherClass { get; set; } = default!;

        public int? ClassId { get; set; }
        public int? TeachId { get; set; }

        public string Url { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? teachId, int? classId)
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
                    if (id == null && teachId == null && classId == null)
                    {
                        return NotFound();
                    }

                    if (id != null)
                        Url = "./Index";
                    if (teachId != null)
                        Url = "/Teachers/Index";
                    if (classId != null)
                        Url = "/ClassRooms/Index";

                    ClassId = classId;
                    TeachId = teachId;

                    var teacherclass = await _context.TeacherClasses.Include(t => t.Teacher)
                                                                     .Include(t => t.Class)
                                                                     .Where(m => (id == null ?
                                                                     (TeachId == null ? m.ClassId == ClassId : m.TeacherId == TeachId)
                                                                     : m.Id == id))
                                                                     .ToListAsync();

                    if (teacherclass == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        TeacherClass = teacherclass;
                    }
                    return Page();
                }
            }
            return Redirect("/Index");
        }
    }
}
