using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
namespace ExcelOrWordForSV
{
    class ExcelProcess
    {
        object missing = Missing.Value;
        Application excelApp;
        string path;
        public ExcelProcess(string path)
        {
            this.path = path;
            excelApp = new Application();
        }



        public void ImportExcel(List<string[]> record)
        {
            excelApp.Visible = false;
            Workbook wBook = excelApp.Workbooks.Add(missing);
            Worksheet wSheet = wBook.Worksheets[1] as Worksheet;
            if (record.Count > 0)
            {
                for(int i = 0;i<record.Count;++i)
                    for (int j = 0; j < record[i].Count(); ++j)
                    {
                        string str = record[i][j];
                        wSheet.Cells[i + 2, j + 1] = str;
                    }

            }
            excelApp.DisplayAlerts = false;
            excelApp.AlertBeforeOverwriting = false;
            wBook.SaveAs(path, missing, missing, missing, missing, missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
            wBook.Close(false);
            excelApp.Quit();
            excelApp = null;
        }
    }
}
