using OfficeOpenXml;
using PRN211_Project.Entities;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using CsvHelper;
using System.Globalization;
using Newtonsoft.Json;

namespace PRN211_Project.Services
{
    public class ReadDataFormFile
    {
        public string FilePath { get; set; }

        public List<ScheduleData> ScheduleData { get; set; }

        public List<ScheduleData> GetScheduleDataFromCSV(IFormFile filePath)
        {
            if (filePath != null && filePath.Length > 0)
            {
                using (var reader = new StreamReader(filePath.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    ScheduleData = csv.GetRecords<ScheduleData>().ToList();
                }
            }
            else
            {
                ScheduleData = new List<ScheduleData>();
            }
            return ScheduleData;
        }

        public List<ScheduleData> GetScheduleDataFromJSON(IFormFile filePath)
        {
            if (filePath != null && filePath.Length > 0)
            {
                using (var reader = new StreamReader(filePath.OpenReadStream()))
                {
                    var jsonString = reader.ReadToEnd();
                    ScheduleData = JsonConvert.DeserializeObject<List<ScheduleData>>(jsonString);
                }
            }
            else
            {
                ScheduleData = new List<ScheduleData>();
            }
            return ScheduleData;
        }

        public List<ScheduleData> GetScheduleDataFromExcel(MemoryStream filePath)
        {
            try
            {
                using (var package = new ExcelPackage(filePath))
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets
                                                      .FirstOrDefault(sheet =>
                                                                        sheet.Name.ToLower().Equals("weekly", StringComparison.OrdinalIgnoreCase)); ;

                    if (worksheet != null)
                    {
                        // Doc du lieu sheet
                        ScheduleData = new List<ScheduleData>();
                        int rowCount = worksheet.Dimension.End.Row;

                        int lastRowWithData = rowCount;
                        while (lastRowWithData > 0 && worksheet.Cells[lastRowWithData, 1].Value == null)
                        {
                            lastRowWithData--;
                        }

                        for (int row = 2; row <= lastRowWithData; row++)
                        {
                            ScheduleData.Add(new ScheduleData
                            {
                                Room = worksheet.Cells[row, 2].Value?.ToString(),
                                TimeSlot = worksheet.Cells[row, 3].Value?.ToString(),
                                Class = worksheet.Cells[row, 4].Value?.ToString(),
                                Subject = worksheet.Cells[row, 5].Value?.ToString(),
                                Teacher = worksheet.Cells[row, 6].Value?.ToString(),
                                Date = Convert.ToDateTime(worksheet.Cells[row, 7].Value?.ToString()),
                            });
                        }
                    }
                    else
                    {
                        ScheduleData = new List<ScheduleData>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading Excel file: {ex.Message}");
            }

            return ScheduleData;
        }


    }
}
