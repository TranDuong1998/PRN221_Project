using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.Schedules
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
                var account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
                if (!account.Role.ToLower().Equals("admin"))
                {
                    return Redirect("/Index");
                }
                else
                {
                    LoadData();
                    return Page();
                }
            }
            return Redirect("/Index");
        }

        [BindProperty]
        public WeeklyTimeTable WeeklyTimeTable { get; set; } = default!;

        public bool CheckValidDate { get; set; } = true;

        public string Message { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            LoadData();
            if (!ModelState.IsValid || _context.WeeklyTimeTables == null || WeeklyTimeTable == null)
            {
                return Page();
            }

            if (_context.WeeklyTimeTables.Where(w => w.LearnDate == WeeklyTimeTable.LearnDate &&
                                                     (w.RoomsId == WeeklyTimeTable.RoomsId && w.TimeSlotId == WeeklyTimeTable.TimeSlotId && w.TeachersId == WeeklyTimeTable.TeachersId &&
                                                      w.ClassId == WeeklyTimeTable.ClassId && w.CourseId != WeeklyTimeTable.CourseId) ||
                                                     (w.RoomsId == WeeklyTimeTable.RoomsId && w.TimeSlotId == WeeklyTimeTable.TimeSlotId && w.TeachersId == WeeklyTimeTable.TeachersId &&
                                                      w.ClassId != WeeklyTimeTable.ClassId && w.CourseId == WeeklyTimeTable.CourseId) ||
                                                     (w.RoomsId == WeeklyTimeTable.RoomsId && w.TimeSlotId == WeeklyTimeTable.TimeSlotId && w.TeachersId != WeeklyTimeTable.TeachersId &&
                                                      w.ClassId == WeeklyTimeTable.ClassId && w.CourseId == WeeklyTimeTable.CourseId) ||
                                                     (w.RoomsId == WeeklyTimeTable.RoomsId && w.TimeSlotId == WeeklyTimeTable.TimeSlotId && w.TeachersId == WeeklyTimeTable.TeachersId &&
                                                      w.ClassId == WeeklyTimeTable.ClassId && w.CourseId == WeeklyTimeTable.CourseId) ||
                                                     (w.RoomsId == WeeklyTimeTable.RoomsId && w.TimeSlotId == WeeklyTimeTable.TimeSlotId && w.ClassId != WeeklyTimeTable.ClassId)).FirstOrDefault() != null)
            {
                CheckValidDate = false;
                Message = "Duplicate data appears!";
                return Page();
            }
            _context.WeeklyTimeTables.Add(WeeklyTimeTable);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public void LoadData()
        {
            var rooms = _context.Rooms.ToList();
            var teachers = _context.Teachers.ToList();
            var classes = _context.ClassRooms.ToList();
            var subs = _context.Courses.ToList();
            var slots = _context.TimeSlots.ToList();

            ViewData["ClassId"] = classes.Select(c => new SelectListItem { Value = c.ClassId.ToString(), Text = c.ClassName }).ToList();
            ViewData["CourseId"] = subs.Select(s => new SelectListItem { Value = s.CourseId.ToString(), Text = s.CourseCode }).ToList();
            ViewData["RoomsId"] = rooms.Select(r => new SelectListItem { Value = r.RoomsId.ToString(), Text = r.RoomsName }).ToList();
            ViewData["TeachersId"] = teachers.Select(t => new SelectListItem { Value = t.TeacherId.ToString(), Text = t.TeachersCode }).ToList();
            ViewData["TimeSlotId"] = slots.Select(s => new SelectListItem { Value = s.TimeSlotId.ToString(), Text = s.Description }).ToList();
        }
    }
}
