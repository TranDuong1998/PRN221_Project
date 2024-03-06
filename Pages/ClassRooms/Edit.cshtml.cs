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

namespace PRN211_Project.Pages.ClassRooms
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
        public ClassRoom ClassRoom { get; set; } = default!;
        public bool CheckAccount = true;
        public string Message;
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
                    if (id == null || _context.ClassRooms == null)
                    {
                        return NotFound();
                    }

                    var classroom = await _context.ClassRooms.FirstOrDefaultAsync(m => m.ClassId == id);
                    if (classroom == null)
                    {
                        return NotFound();
                    }
                    ClassRoom = classroom;
                    return Page();
                }
            }
            return Redirect("/Index");
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ClassRoomExists(ClassRoom.ClassId, ClassRoom.ClassName))
            {
                CheckAccount = false;
                Message = "Classroom dose exists!";
                return Page();
            }

            _context.Attach(ClassRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassRoomExists(ClassRoom.ClassId))
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

        private bool ClassRoomExists(int id)
        {
            return (_context.ClassRooms?.Any(e => e.ClassId == id)).GetValueOrDefault();
        }

        private bool ClassRoomExists(int id, string name)
        {
            return (_context.ClassRooms?.Any(e => e.ClassId != id && e.ClassName.ToLower().Equals(name))).GetValueOrDefault();
        }

    }
}
