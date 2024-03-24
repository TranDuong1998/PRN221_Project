using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;

namespace PRN211_Project.Pages.TeacherInClass
{
    public class DeleteModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        public DeleteModel(PRN211_Project.Models.Prn211ProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
      public TeacherClass TeacherClass { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.TeacherClasses == null)
            {
                return NotFound();
            }

            var teacherclass = await _context.TeacherClasses.FirstOrDefaultAsync(m => m.Id == id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.TeacherClasses == null)
            {
                return NotFound();
            }
            var teacherclass = await _context.TeacherClasses.FindAsync(id);

            if (teacherclass != null)
            {
                TeacherClass = teacherclass;
                _context.TeacherClasses.Remove(TeacherClass);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
