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
using System.Drawing.Printing;


namespace PRN211_Project.Pages.Schedules
{
    public class ScheduleModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private ReadDataFormFile _readDataFormFile = new ReadDataFormFile();
        private Prn211ProjectContext _context;
        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();
        private ScheduleServices scheduleServices = new ScheduleServices();
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

        public Dictionary<ScheduleData, string> Data { get; set; } = null;

        [BindProperty(SupportsGet = true)]
        public string FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ToDate { get; set; }

        public bool CheckValidDate { get; set; } = true;

        public string Message { get; set; }

        public DateTime MaxDate = DateTime.Parse(DateTime.Now.Year.ToString() + "-12-31");
        public DateTime MinDate = Convert.ToDateTime("1932-01-01");

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
            GetDataFromFile(FileUploads);
            return Page();
        }

        public async Task<IActionResult> OnPostImportFileAsync(IFormFile FileUploads)
        {
            int count = 0;
            GetDataFromFile(FileUploads);
            foreach (var item in ScheduleData)
            {
                if (!ValidationInput.ValidateAnyCode(item.Teacher))
                {
                    if (Data.ContainsKey(item))
                    {
                        Data[item] = "Error! Invalid Teacher code! Ex: \"NamNV35\"";
                    }
                    count++;
                }
                if (!ValidationInput.ValidateAnyCode(item.Room))
                {
                    if (Data.ContainsKey(item))
                    {
                        Data[item] = "Error! Invalid Room name! Ex: \"AR221\" or \"B221\"";
                    }
                    count++;
                }
                if (!ValidationInput.ValidateAnyCode(item.Class))
                {
                    if (Data.ContainsKey(item))
                    {
                        Data[item] = "Error! Invalid Class name! Ex: \"SE1632\"";
                    }
                    count++;
                }
                if (!ValidationInput.ValidateAnyCode(item.Subject))
                {
                    if (Data.ContainsKey(item))
                    {
                        Data[item] = "Error! Invalid Course code! Ex: \"Prn221\" or \"PRN221\"";
                    }
                    count++;
                }
                if (!ValidationInput.ValidateTimeSlot(item.TimeSlot))
                {
                    if (Data.ContainsKey(item))
                    {
                        Data[item] = "Error! Invalid Timeslot! Ex: \"P24\" or \"A24\"";
                    }
                    count++;
                }
            }
            if (count > 0)
            {
                return Page();
            }
            ImportData();
            return Page();
        }

        public void ImportData()
        {
            if (FromDate != null && ToDate != null)
            {
                DateTime fromDate = Convert.ToDateTime(FromDate);
                DateTime toDate = Convert.ToDateTime(ToDate);

                List<DateTime> days = new List<DateTime>();

                List<WeeklyTimeTable> timeTables = new List<WeeklyTimeTable>();

                foreach (var item in ScheduleData)
                {
                    int count = 0;
                    timeTables = _context.WeeklyTimeTables.Include(sh => sh.Rooms)
                                                          .Include(sh => sh.TimeSlot)
                                                          .Include(sh => sh.Class)
                                                          .Include(sh => sh.Course)
                                                          .Include(sh => sh.Teachers)
                                                          .Where(w => w.LearnDate >= fromDate && w.LearnDate <= toDate)
                                                          .ToList();

                    char[] characters = item.TimeSlot.ToCharArray();
                    string timeOfDay = (characters[0] == 'P') ? "PM" : "AM";

                    var r = _context.Rooms.FirstOrDefault(r => r.RoomsName.Equals(item.Room));
                    var t = _context.Teachers.FirstOrDefault(t => t.TeachersCode.Equals(item.Teacher));
                    var c = _context.ClassRooms.FirstOrDefault(c => c.ClassName.Equals(item.Class));
                    var sub = _context.Courses.FirstOrDefault(c => c.CourseCode.Equals(item.Subject));
                    var slot = _context.TimeSlots.Where(ts => (timeOfDay == "AM" ? (ts.Description == "Slot 1" || ts.Description == "Slot 2") :
                                                       (ts.Description == "Slot 3" || ts.Description == "Slot 4")))
                                                 .ToList();
                    var tc = _context.TeacherClasses.Where(tc => tc.TeacherId == t.TeacherId && tc.ClassId == c.ClassId).FirstOrDefault();
                    var td = _context.TeacherDetails.Where(td => td.TeacherId == t.TeacherId && td.CourseId == sub.CourseId).FirstOrDefault();

                    string err = "";
                    if (r == null)
                    {
                        err += "Room does not exists! ";
                        count++;
                    }
                    if (t == null)
                    {
                        err += "Teacher does not exists! ";
                        count++;
                    }
                    if (c == null)
                    {
                        err += "Class does not exists! ";
                        count++;
                    }
                    if (sub == null)
                    {
                        err += "Course does not exists! ";
                        count++;
                    }
                    if (tc == null)
                    {
                        err += "The teacher does not teach class! ";
                        count++;
                    }
                    if (td == null)
                    {
                        err += "Teachers do not teach this subject! ";
                        count++;

                    }

                    if (Data.ContainsKey(item))
                    {
                        Data[item] = "Error! " + err;
                    }

                    if (count == 0)
                    {
                        days = GetDateFromDayOfWeek(item.TimeSlot);

                        foreach (var day in days)
                        {
                            if (day >= fromDate && day <= toDate)
                            {
                                var weeklies = timeTables.Where(sh => sh.LearnDate == day).ToList();


                                if (day.DayOfWeek.ToString().Equals(GetDayOfWeekName(Convert.ToInt32(characters[1].ToString()))))
                                {
                                    if (!scheduleServices.CheckRequired(weeklies, r.RoomsId, slot[0].TimeSlotId, c.ClassId, t.TeacherId, sub.CourseId))
                                    {
                                        if (Data.ContainsKey(item))
                                        {
                                            Data[item] = "Error! Duplicate data appears!";
                                        }
                                    }
                                    else
                                    {
                                        if (Data.ContainsKey(item))
                                        {
                                            Data[item] = "Success";
                                            WeeklyTimeTable weeklyTimeTable = new WeeklyTimeTable()
                                            {
                                                RoomsId = r?.RoomsId,
                                                ClassId = c?.ClassId,
                                                TeachersId = t?.TeacherId,
                                                CourseId = sub?.CourseId,
                                                TimeSlotId = slot[0]?.TimeSlotId,
                                                LearnDate = day,
                                            };
                                            _context.WeeklyTimeTables.Add(weeklyTimeTable);
                                            _context.SaveChanges();
                                        }
                                    }
                                }
                                if (day.DayOfWeek.ToString().Equals(GetDayOfWeekName(Convert.ToInt16(characters[2].ToString()))))
                                {
                                    if (!scheduleServices.CheckRequired(weeklies, r.RoomsId, slot[1].TimeSlotId, c.ClassId, t.TeacherId, sub.CourseId))
                                    {
                                        if (Data.ContainsKey(item))
                                        {
                                            Data[item] = "Error! Duplicate data appears!";
                                        }
                                    }
                                    else
                                    {
                                        if (Data.ContainsKey(item))
                                        {
                                            Data[item] = "Success";
                                            WeeklyTimeTable weeklyTimeTable = new WeeklyTimeTable()
                                            {
                                                RoomsId = r?.RoomsId,
                                                ClassId = c?.ClassId,
                                                TeachersId = t?.TeacherId,
                                                CourseId = sub?.CourseId,
                                                TimeSlotId = slot[1]?.TimeSlotId,
                                                LearnDate = day,
                                            };
                                            _context.WeeklyTimeTables.Add(weeklyTimeTable);
                                            _context.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                CheckValidDate = false;
                Message = "Please select From date and To date before!";
            }
        }

        public async void GetDataFromFile(IFormFile FileUploads)
        {
            Data = new Dictionary<ScheduleData, string>();
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
                    Data.Add(item, "");
                }
                ViewData["File"] = FileUploads.FileName;
            }
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
