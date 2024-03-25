using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.TeacherInClass
{
    public class EditModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();

        public EditModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        [BindProperty]
        public TeacherClass TeacherClass { get; set; } = default!;

        public string Message;
        public bool Status = false;
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
                    if (id == null || _context.TeacherClasses == null)
                    {
                        return NotFound();
                    }

                    var teacherclass = LoadData(id);
                    if (teacherclass == null)
                    {
                        return NotFound();
                    }
                    TeacherClass = teacherclass;

                    return Page();
                }
            }
            return Redirect("/Index");

        }

        public TeacherClass LoadData(int? id)
        {
            var teachers = _context.Teachers.ToList();
            var classes = _context.ClassRooms.ToList();

            ViewData["ClassId"] = classes.Select(c => new SelectListItem { Value = c.ClassId.ToString(), Text = c.ClassName }).ToList();
            ViewData["TeacherId"] = teachers.Select(t => new SelectListItem { Value = t.TeacherId.ToString(), Text = t.TeachersCode }).ToList();
            var teacherclass = _context.TeacherClasses.FirstOrDefault(m => m.Id == id);
            return teacherclass;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TeacherClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherClassExists(TeacherClass.Id))
                {
                    TeacherClass = LoadData(id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TeacherClassExists(int id)
        {
            return (_context.TeacherClasses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
