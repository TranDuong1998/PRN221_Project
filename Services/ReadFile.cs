using GalaSoft.MvvmLight.Command;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace PRN221_Project.Services
{
    public class ReadFile : INotifyPropertyChanged
    {
        RelayCommand importCommand;
        RelayCommand readCommand;

        private string filePath;

        public string FilePath
        {
            get { return filePath; }
            set
            {
                if (value != filePath)
                {
                    filePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand ImportFileCommand { get => importCommand; set => importCommand = value; }

        public RelayCommand ReadFileCommand { get => readCommand; set => readCommand = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ReadFile()
        {
            importCommand = new RelayCommand(ImportFile);
            readCommand = new RelayCommand(ReadDataFromFile);
        }

        void ImportFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        void ReadDataFromFile()
        {
            if (FilePath.Contains(".xlsx"))
            {
                ReadExcelFile(FilePath);
            }
            else if (FilePath.Contains(".json"))
            {
                ReadJsonFile(FilePath);
            }
            else if (FilePath.Contains(".csv"))
            {
                ReadCsvFile(FilePath);
            }
            else
            {
                MessageBox.Show("Please choose file Excel \".xlsx\" | JSON \".json\" | CSV \".csv\"", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ReadExcelFile(string filePath)
        {
            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    List<List<string>> excelData = new List<List<string>>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        List<string> rowData = new List<string>();

                        for (int col = 2; col <= colCount; col++)
                        {
                            rowData.Add(worksheet.Cells[row, col].Value?.ToString() ?? "");
                        }

                        excelData.Add(rowData);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading Excel file: {ex.Message}");
            }
        }

        public void ReadCsvFile(string filePath)
        {
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    // Process fields as needed
                }
            }
        }

        public void ReadJsonFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string jsondata = sr.ReadToEnd();
                var data = JsonSerializer.Deserialize<List<string>>(jsondata);
            }
        }
    }

   
}
