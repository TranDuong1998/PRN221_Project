using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;

namespace PRN211_Project.Pages.TeacherInCourses
{
    public class DetailsModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        public DetailsModel(PRN211_Project.Models.Prn211ProjectContext context)
        {
            _context = context;
        }

        public List<TeacherDetail> TeacherDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.TeacherDetails == null)
            {
                return NotFound();
            }

            var teacherdetail = await _context.TeacherDetails.Include(t => t.Teacher)
                                                             .Include(t => t.Course)
                                                             .Where(m => m.Id == id || m.TeacherId == id || m.CourseId == id)
                                                             .OrderBy(m => m.TeacherId)
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
}
