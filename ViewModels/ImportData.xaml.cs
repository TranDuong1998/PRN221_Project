using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRN221_Project.ViewModels
{
    /// <summary>
    /// Interaction logic for ImportData.xaml
    /// </summary>
    public partial class ImportData : Window
    {
        public List<string> listData = new List<string>();

        public ImportData()
        {
            InitializeComponent();
        }

        private void btnChooseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel | *.xlsx, *.xls |CSV | *.csv |JSON | *.json";

            if (openFileDialog.ShowDialog() == true)
            {
                txtFileName.Text = openFileDialog.FileName;
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (txtFileName.Text == null)
            {
                MessageBox.Show("Please choose file", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtFileName.Text.Contains(".xlsx") || txtFileName.Text.Contains(".xls"))
            {
                ReadExcelFile(txtFileName.Text);
            }
            else if (txtFileName.Text.Contains(".json"))
            {
                ReadJsonFile(txtFileName.Text);
            }
            else if (txtFileName.Text.Contains(".csv"))
            {
                ReadCsvFile(txtFileName.Text);
            }
            else
            {
                MessageBox.Show("Please choose file Excel \".xlsx\" | \".xls\" |JSON \".json\" | CSV \".csv\"", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
