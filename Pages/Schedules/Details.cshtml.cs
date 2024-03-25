using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.Schedules
{
    public class DetailsModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();

        public DetailsModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public WeeklyTimeTable WeeklyTimeTable { get; set; } = default!;

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
                    if (id == null || _context.WeeklyTimeTables == null)
                    {
                        return NotFound();
                    }

                    var weeklytimetable = await _context.WeeklyTimeTables.Include(w => w.Rooms)
                                                                 .Include(w => w.TimeSlot)
                                                                 .Include(w => w.Class)
                                                                 .Include(w => w.Course)
                                                                 .Include(w => w.Teachers).FirstOrDefaultAsync(m => m.Id == id);
                    if (weeklytimetable == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        WeeklyTimeTable = weeklytimetable;
                    }
                    return Page();
                }
            }
            return Redirect("/Index");
        }
    }
}
