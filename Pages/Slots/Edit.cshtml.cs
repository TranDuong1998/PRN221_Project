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

namespace PRN211_Project.Pages.Slots
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
                    TimeSlot = timeslot;
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

            

            _context.Attach(TimeSlot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeSlotExists(TimeSlot.TimeSlotId))
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

        private bool TimeSlotExists(int id)
        {
            return (_context.TimeSlots?.Any(e => e.TimeSlotId == id)).GetValueOrDefault();
        }

        private bool TimeSlotExists(TimeSpan checkTime)
        {
            return (_context.TimeSlots?.Any(e => e.StartTime >= checkTime && e.EndTime <= checkTime)).GetValueOrDefault();
        }
    }
}
