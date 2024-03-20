using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN211_Project.Entities;
using PRN211_Project.Services;
using System.ComponentModel.DataAnnotations;
using System.IO;
using OfficeOpenXml;
using PRN211_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;


namespace PRN211_Project.Pages.Schedules
{
    public class ScheduleModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private ReadDataFormFile _readDataFormFile = new ReadDataFormFile();
        private Prn211ProjectContext _context;
        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();
        public ScheduleModel(IWebHostEnvironment environment, Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _environment = environment;
            _context = context;
            _httpContext = httpContext;
        }

        [BindProperty(SupportsGet = true)]
        public IFormFile FileUploads { get; set; }

        [BindProperty]
        public string FilePath { get; set; }

        public List<ScheduleData> ScheduleData { get; set; } = default;

        public Dictionary<ScheduleData, int> Data { get; set; } = null;

        public async Task<IActionResult> OnGet()
        {
            //if (_httpContext.HttpContext!.Session.GetString("Account") != null)
            //{
            //    var account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
            //    if (!account.Role.ToLower().Equals("admin"))
            //    {
            //        return Redirect("/Index");
            //    }
            //    else
            //    {
            return Page();
            //    }
            //}
            //return Redirect("/Index");

        }

        public async Task<IActionResult> OnPostViewDataAsync(IFormFile FileUploads)
        {
            Data = new Dictionary<ScheduleData, int>();
            if (FileUploads != null && FileUploads.Length != 0)
            {
                FilePath = Path.Combine(FileUploads.FileName);

                using (var stream = new MemoryStream())
                {
                    await FileUploads.CopyToAsync(stream);

                    if (FilePath.Contains(".csv"))
                    {
                        ScheduleData = _readDataFormFile.GetScheduleDataFromCSV(FileUploads);
                    }
                    else if (FilePath.Contains(".json"))
                    {
                        ScheduleData = _readDataFormFile.GetScheduleDataFromJSON(FileUploads);
                    }
                    else if (FilePath.Contains(".xlsx") || FilePath.Contains(".xls"))
                    {
                        ScheduleData = _readDataFormFile.GetScheduleDataFromExcel(stream);
                    }
                }
                foreach (var item in ScheduleData)
                {
                    Data.Add(item, 1);
                }
            }
            else
            {

            }
            return Page();
        }

        public async Task<IActionResult> OnPostImportFileAsync(IFormFile FileUploads)
        {
            Data = new Dictionary<ScheduleData, int>();

            var sh = _context.WeeklyTimeTables.Include(sh => sh.Rooms)
                                              .Include(sh => sh.TimeSlot)
                                              .Include(sh => sh.Class)
                                              .Include(sh => sh.Course)
                                              .Include(sh => sh.Teachers)
                                              .ToList();

            List<DateTime> days = new List<DateTime>();

            if (FileUploads != null && FileUploads.Length != 0)
            {
                FilePath = Path.Combine(FileUploads.FileName);

                using (var stream = new MemoryStream())
                {
                    await FileUploads.CopyToAsync(stream);

                    if (FilePath.Contains(".csv"))
                    {
                        ScheduleData = _readDataFormFile.GetScheduleDataFromCSV(FileUploads);
                    }
                    else if (FilePath.Contains(".json"))
                    {
                        ScheduleData = _readDataFormFile.GetScheduleDataFromJSON(FileUploads);
                    }
                    else if (FilePath.Contains(".xlsx") || FilePath.Contains(".xls"))
                    {
                        ScheduleData = _readDataFormFile.GetScheduleDataFromExcel(stream);
                    }
                }

                foreach (var item in ScheduleData)
                {
                    char[] characters = item.TimeSlot.ToCharArray();
                    string timeOfDay = (characters[0] == 'P') ? "PM" : "AM";

                    var r = _context.Rooms.FirstOrDefault(r => r.RoomsName.Equals(item.Room));
                    var t = _context.Teachers.FirstOrDefault(t => t.TeachersCode.Equals(item.Teacher));
                    var c = _context.ClassRooms.FirstOrDefault(c => c.ClassName.Equals(item.Class));
                    var sub = _context.Courses.FirstOrDefault(c => c.CourseCode.Equals(item.Subject));
                    var slot = _context.TimeSlots.Where(ts =>
                                                           (timeOfDay == "AM" ? (ts.Description == "Slot 1" || ts.Description == "Slot 2") :
                                                                         (ts.Description == "Slot 3" || ts.Description == "Slot 4")))
                                                           .ToList();

                    days = GetDateFromDayOfWeek(item.TimeSlot);

                    foreach (var day in days)
                    {
                        foreach (var s in slot)
                        {
                            var shedules = sh.Where(sh => sh.LearnDate.Equals(day) && sh.TimeSlotId == s.TimeSlotId).ToList();

                        }
                    }
                }
            }
            else
            {

            }
            return Page();
        }

        public List<DateTime> GetDateFromDayOfWeek(string input)
        {
            char[] characters = input.ToCharArray();
            int[] daysOfWeek = new int[characters.Length - 1];
            for (int i = 1; i < characters.Length; i++)
            {
                daysOfWeek[i - 1] = int.Parse(characters[i].ToString());
            }

            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            List<DateTime> dates = new List<DateTime>();

            while (startDate.Month == DateTime.Now.Month)
            {
                for (int i = 0; i < daysOfWeek.Length; i++)
                {
                    string day = GetDayOfWeekName(daysOfWeek[i]);

                    if (startDate.DayOfWeek.ToString() == day)
                    {
                        dates.Add(startDate);
                    }
                }
                startDate = startDate.AddDays(1);
            }
            return dates;
        }

        public string GetDayOfWeekName(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 2: return "Monday";
                case 3: return "Tuesday";
                case 4: return "Wednesday";
                case 5: return "Thursday";
                case 6: return "Friday";
                case 7: return "Saturday";
                case 8: return "Sunday";
                default: return "";
            }
        }
    }
}
