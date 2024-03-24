using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.TeacherInClass
{
    public class CreateModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();

        public CreateModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IActionResult OnGet()
        {
            if (_httpContext.HttpContext!.Session.GetString("Account") != null)
            {
                LoadData();
                var account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
                if (!account.Role.ToLower().Equals("admin"))
                {
                    return Redirect("/Index");
                }
                else
                    return Page();
            }
            else
                return Redirect("/Index");
        }

        public void LoadData()
        {
            var teachers = _context.Teachers.ToList();
            var classes = _context.ClassRooms.ToList();

            ViewData["ClassId"] = classes.Select(c => new SelectListItem { Value = c.ClassId.ToString(), Text = c.ClassName }).ToList();
            ViewData["TeachersId"] = teachers.Select(t => new SelectListItem { Value = t.TeacherId.ToString(), Text = t.TeachersCode }).ToList();
        }

        [BindProperty]
        public TeacherClass TeacherClass { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            LoadData();
          if (!ModelState.IsValid || _context.TeacherClasses == null || TeacherClass == null)
            {
                return Page();
            }

            _context.TeacherClasses.Add(TeacherClass);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
