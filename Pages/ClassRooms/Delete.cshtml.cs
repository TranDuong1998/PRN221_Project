using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.ClassRooms
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
        public ClassRoom ClassRoom { get; set; } = default!;
        public bool CheckAccount = true;
        public string Message;
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
                    if (id == null || _context.ClassRooms == null)
                    {
                        return NotFound();
                    }

                    var classroom = await _context.ClassRooms.FirstOrDefaultAsync(m => m.ClassId == id);

                    if (classroom == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        ClassRoom = classroom;
                    }
                    return Page();
                }
            }
            else
                return Redirect("/Index");
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.ClassRooms == null)
            {
                return NotFound();
            }
            var classroom = await _context.ClassRooms.FindAsync(id);

            if (classroom != null)
            {
                ClassRoom = classroom;

                var ct = _context.TeacherClasses.Where(c => c.ClassId == ClassRoom.ClassId).ToList();
                var w = _context.WeeklyTimeTables.Where(c => c.ClassId == ClassRoom.ClassId).ToList();

                _context.TeacherClasses.RemoveRange(ct);
                _context.WeeklyTimeTables.RemoveRange(w);
                await _context.SaveChangesAsync();
                _context.ClassRooms.Remove(ClassRoom);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
