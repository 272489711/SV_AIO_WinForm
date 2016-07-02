using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOrWordForSV
{
    public class Import
    {
        private List<string[]> _content=new List<string[]>();
        /// <summary>
        /// 文本数据读入内存
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool readFromTxt(string url)
        {
            
            try
            {
                //FileStream file = new FileStream(url, FileMode.Open);
                //file.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(url, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                        //int i = 1;
                        string[] str = line.Split('|');
                        //foreach (string s in str)
                        //{
                        //    Console.WriteLine(i + "        " + s);
                        //    i++;
                        //}

                        _content.Add(str);
                }
               
                return true;
            }
            catch (IOException e)
            {
                throw e;
            }
        }
        public List<string[]> GetContent()
        {
            return _content;
        }
        
    }
}
