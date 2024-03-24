using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.Slots
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
        public TimeSlot TimeSlot { get; set; } = default!;

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
                    if (id == null || _context.TimeSlots == null)
                    {
                        return NotFound();
                    }

                    var timeslot = await _context.TimeSlots.FirstOrDefaultAsync(m => m.TimeSlotId == id);

                    if (timeslot == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        TimeSlot = timeslot;
                    }
                    return Page();
                }
            }
            return Redirect("/Index");
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.TimeSlots == null)
            {
                return NotFound();
            }
            var timeslot = await _context.TimeSlots.FindAsync(id);

            if (timeslot != null)
            {
                TimeSlot = timeslot;

                var w = _context.WeeklyTimeTables.Where(t => t.TimeSlotId==TimeSlot.TimeSlotId).ToList();

                _context.WeeklyTimeTables.RemoveRange(w);
                await _context.SaveChangesAsync();
                _context.TimeSlots.Remove(TimeSlot);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
