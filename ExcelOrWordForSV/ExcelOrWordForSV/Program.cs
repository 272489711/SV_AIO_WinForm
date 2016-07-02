using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOrWordForSV
{
    class Program
    {
        static void Main(string[] args)
        {
            Import im = new Import();
            Console.WriteLine("文本URL：");
            string path = Console.ReadLine();
            im.readFromTxt(path);

            ExcelProcess exlP = new ExcelProcess(path.Substring(0, path.Count() - 5));

            exlP.ImportExcel(im.GetContent());
            Console.WriteLine("导入成功！");
            Console.Read();

        }
    }
}
