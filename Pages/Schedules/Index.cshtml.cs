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

        private int PageSize = 10;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public IList<WeeklyTimeTable> WeeklyTimeTable { get; set; } = default!;

        public string[,] WeeklyTable { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageIndex)
        {
            if (_httpContext.HttpContext!.Session.GetString("Account") != null)
            {
                Account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
                if (Account.Role.ToLower().Equals("admin"))
                    Title = "Shedule Management";
                else
                    Title = "Shedule";

                var slots = _context.TimeSlots.ToList();
                var rooms = _context.Rooms.ToList();
                var learnDate = _context.WeeklyTimeTables.OrderBy(w => w.LearnDate).ToList();
                var teacher = _context.Teachers.ToList();
                var classRoom = _context.ClassRooms.ToList();

                Col = slots.Count;
                Row = rooms.Count;

                WeeklyTimeTable = new List<WeeklyTimeTable>();

                WeeklyTable = new string[Row + 1, Col + 1];
                WeeklyTable[0, 0] = "Room \\ TimeSlot";

                for (int i = 1; i <= Row; i++)
                {

                    WeeklyTable[i, 0] = rooms[i - 1].RoomsName;

                }

                for (int j = 1; j <= Col; j++)
                {
                    if (j - 1 < learnDate.Count)
                    {
                        WeeklyTable[0, j] = $"{slots[j - 1].Description} - {(learnDate[j - 1].LearnDate == null ? "" : learnDate[j - 1].LearnDate.Value.ToString("dd/MM/yyyy"))}";
                    }
                    else
                        WeeklyTable[0, j] = slots[j - 1].Description;
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
                                               .Where(w => w.LearnDate == l.LearnDate &&
                                                          w.RoomsId == rooms[i - 1].RoomsId &&
                                                          w.TimeSlotId == slots[j - 1].TimeSlotId)
                                               .OrderBy(w => w.LearnDate)
                                               .FirstOrDefault();
                            if (Weekdaly != null)
                                WeeklyTimeTable.Add(Weekdaly);
                        }
                    }
                }

                foreach (var entry in WeeklyTimeTable)
                {
                    int rowIndex = rooms.FindIndex(r => r.RoomsId == entry.RoomsId) + 1;
                    int colIndex = slots.FindIndex(s => s.TimeSlotId == entry.TimeSlotId) + 1;

                    WeeklyTable[rowIndex, colIndex] = $"{entry.Teachers.TeachersCode}-{entry.Class.ClassName}-{entry.Course.CourseCode}";
                }

                return Page();
            }
            else
                return Redirect("/Index");
        }
    }
}
