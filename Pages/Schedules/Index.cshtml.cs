using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.Schedules
{
    public class IndexModel : PageModel
    {
        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();
        private AccountServices _accountServices = new AccountServices();

        public Account Account;
        private readonly Prn211ProjectContext _context;

        public string Title { get; set; }

        public IndexModel(Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        private int PageSize = 5;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public IList<WeeklyTimeTable> WeeklyTimeTable { get; set; } = default!;

        public string[,] WeeklyTable { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime ViewDate { get; set; } = DateTime.Now;

        public async Task<IActionResult> OnGetAsync(int pageIndex, DateTime viewDate)
        {
            if (_httpContext.HttpContext!.Session.GetString("Account") != null)
            {
                Account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
                if (Account.Role.ToLower().Equals("admin"))
                    Title = "Shedule Management";
                else
                    Title = "Shedule";

                TotalPages = (int)Math.Ceiling((double)_context.Rooms.Count() / PageSize);

                CurrentPage = Math.Max(1, Math.Min(PageIndex, TotalPages));

                var slots = _context.TimeSlots.ToList();
                var rooms = _context.Rooms.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                var learnDate = _context.WeeklyTimeTables.OrderBy(w => w.LearnDate).ToList();
                var teacher = _context.Teachers.ToList();
                var classRoom = _context.ClassRooms.ToList();

                viewDate = ViewDate;
                ViewDate = viewDate;

                Col = slots.Count;
                Row = rooms.Count;

                WeeklyTimeTable = new List<WeeklyTimeTable>();

                WeeklyTable = new string[Row + 1, Col + 1];
                WeeklyTable[0, 0] = "Room";

                for (int i = 1; i <= Row; i++)
                {

                    WeeklyTable[i, 0] = rooms[i - 1].RoomsName;

                }

                for (int j = 1; j <= Col; j++)
                {
                    WeeklyTable[0, j] = $"{slots[j - 1].Description} \\ " +
                        $"({(slots[j - 1].StartTime == null ? "" : slots[j - 1].StartTime?.ToString("hh\\:mm")) + " - " + (slots[j - 1].EndTime == null ? "" : slots[j - 1].EndTime?.ToString("hh\\:mm"))})";
                }

                for (int i = 1; i <= Row; i++)
                {
                    for (int j = 1; j <= Col; j++)
                    {
                        foreach (var l in learnDate)
                        {
                            var Weekdaly = _context.WeeklyTimeTables
                                               .Include(w => w.Class)
                                               .Include(w => w.Course)
                                               .Include(w => w.Rooms)
                                               .Include(w => w.Teachers)
                                               .Include(w => w.TimeSlot)
                                               .Where(w => w.LearnDate == viewDate &&
                                                          w.RoomsId == rooms[i - 1].RoomsId &&
                                                          w.TimeSlotId == slots[j - 1].TimeSlotId)
                                               .OrderBy(w => w.LearnDate)
                                               .Skip((CurrentPage - 1) * PageSize).Take(PageSize)
                                               .FirstOrDefault();
                            if (Weekdaly != null)
                                WeeklyTable[i, j] = $"{Weekdaly.Class.ClassName}\\{Weekdaly.Course.CourseCode}\\{Weekdaly.Teachers.TeachersCode}";
                            else
                                WeeklyTable[i, j] = "-";
                        }
                    }
                }

                return Page();
            }
            else
                return Redirect("/Index");
        }

    }
}
