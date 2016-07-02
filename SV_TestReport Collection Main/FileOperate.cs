using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SV_TestReport_Collection_Main
{
    class FileOperate
    {
        private List<MyTxtFile> myFileInfos;
        private DirectoryInfo sourceFolder;
        private string completedPath;
        private string failedPath;
        private Regex r;

        public FileOperate(string path,string completedPath,string failedPath)
        {
            this.completedPath = completedPath;
            this.failedPath = failedPath;
            if (!Directory.Exists(completedPath))
                Directory.CreateDirectory(completedPath);
            if (!Directory.Exists(failedPath))
                Directory.CreateDirectory(failedPath);

            r = new Regex(@"([A-Za-z0-9]{12})_[A-Za-z0-9]*_[0-9]{12,14}_[A-Za-z]*.txt");
            myFileInfos = new List<MyTxtFile>();
            sourceFolder = new DirectoryInfo(path);
            
        }

        public List<MyTxtFile> getFileInfos()
        {
            myFileInfos.Clear();
            GC.Collect();
            try
            {
                foreach (FileInfo file in sourceFolder.GetFiles("*.txt"))
                {
                    if (r.Match(file.Name).Success)
                    {
                        if (file.Name.Contains("Passed"))
                            myFileInfos.Add(new MyTxtFile(file.FullName, file.Name, TxtType.Passed));
                        if (file.Name.Contains("Error"))
                            myFileInfos.Add(new MyTxtFile(file.FullName, file.Name, TxtType.Error));
                        if (file.Name.Contains("Failed"))
                            myFileInfos.Add(new MyTxtFile(file.FullName, file.Name, TxtType.Failed));
                        if (file.Name.Contains("Terminated"))
                            myFileInfos.Add(new MyTxtFile(file.FullName, file.Name, TxtType.Terminated));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[报告目录出错]" + ex.ToString());
            }
            return myFileInfos;
        }

        public void moveFile(string fileFullName,string fileName,bool isOK)
        {
            if (isOK)
            {
                File.Copy(fileFullName, completedPath + "\\" + fileName, true);
                File.Delete(fileFullName);
            }
            else
            {
                File.Copy(fileFullName, failedPath + "\\" + fileName, true);
                File.Delete(fileFullName);
            }
        }
    }
}
