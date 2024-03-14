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


namespace PRN211_Project.Pages.Schedules
{
    public class ScheduleModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private ReadDataFormFile _readDataFormFile = new ReadDataFormFile();
        private Prn211ProjectContext _context;

        public ScheduleModel(IWebHostEnvironment environment, Prn211ProjectContext context)
        {
            _environment = environment;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public IFormFile FileUploads { get; set; }

        [BindProperty]
        public string FilePath { get; set; }

        public List<ScheduleData> ScheduleData { get; set; } = default;

        public Dictionary<ScheduleData, int> Data { get; set; } = null;

        public async void OnGet()
        {
            if (FileUploads != null && FileUploads.Length != 0)
            {

            }
            else
            {
                Data = new Dictionary<ScheduleData, int>();
            }
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
                    Data.Add(item, 0);
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
                    var r = _context.Rooms.FirstOrDefault(r => r.RoomsName.Equals(item.Room));
                    
                }
            }
            else
            {

            }
            return Page();
        }
    }
}
