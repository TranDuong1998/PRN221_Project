using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;

namespace PRN211_Project.Pages.Schedules
{
    public class EditModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        public EditModel(PRN211_Project.Models.Prn211ProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WeeklyTimeTable WeeklyTimeTable { get; set; } = default!;

        public bool CheckValidDate { get; set; } = true;

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.WeeklyTimeTables == null)
            {
                return NotFound();
            }

            if (LoadData(id) == null)
            {
                return NotFound();
            }

            WeeklyTimeTable = LoadData(id);

            return Page();
        }

        public WeeklyTimeTable LoadData(int? id)
        {
            var weeklytimetable = _context.WeeklyTimeTables.Include(w => w.Rooms)
                                                         .Include(w => w.TimeSlot)
                                                         .Include(w => w.Class)
                                                         .Include(w => w.Course)
                                                         .Include(w => w.Teachers)
                                                         .FirstOrDefault(w => w.Id == id);
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

            return weeklytimetable;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(WeeklyTimeTable).State = EntityState.Modified;

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
                WeeklyTimeTable = LoadData(id);
                CheckValidDate = false;
                Message = "Duplicate data appears!";
                return Page();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeeklyTimeTableExists(WeeklyTimeTable.Id))
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

        private bool WeeklyTimeTableExists(int id)
        {
            return (_context.WeeklyTimeTables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
