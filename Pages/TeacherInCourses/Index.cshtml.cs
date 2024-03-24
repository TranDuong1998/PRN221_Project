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
    public class IndexModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        public IndexModel(PRN211_Project.Models.Prn211ProjectContext context)
        {
            _context = context;
        }

        public IList<TeacherDetail> TeacherDetail { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.TeacherDetails != null)
            {
                TeacherDetail = await _context.TeacherDetails
                .Include(t => t.Course)
                .Include(t => t.Teacher).ToListAsync();
            }
        }
    }
}
