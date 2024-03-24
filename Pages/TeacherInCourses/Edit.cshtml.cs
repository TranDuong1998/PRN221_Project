using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;

namespace PRN211_Project.Pages.TeacherInCourses
{
    public class EditModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        public EditModel(PRN211_Project.Models.Prn211ProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TeacherDetail TeacherDetail { get; set; } = default!;
        public string Message;
        public bool Status = false;
        public void LoadData()
        {
            var teachers = _context.Teachers.ToList();
            var courses = _context.Courses.ToList();

            ViewData["CourseId"] = courses.Select(c => new SelectListItem { Value = c.CourseId.ToString(), Text = c.CourseCode }).ToList();
            ViewData["TeacherId"] = teachers.Select(t => new SelectListItem { Value = t.TeacherId.ToString(), Text = t.TeachersCode }).ToList();
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.TeacherDetails == null)
            {
                return NotFound();
            }

            var teacherdetail =  await _context.TeacherDetails.Include(t => t.Teacher)
                                                             .Include(t => t.Course).FirstOrDefaultAsync(m => m.Id == id);
            if (teacherdetail == null)
            {
                return NotFound();
            }
            TeacherDetail = teacherdetail;
            LoadData();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            LoadData();
            _context.Attach(TeacherDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherDetailExists(TeacherDetail.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TeacherDetailExists(int id)
        {
          return (_context.TeacherDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
