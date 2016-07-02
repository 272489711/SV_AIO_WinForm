using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV_TestReport_Collection_Main
{
    public enum TxtType{Passed,Failed,Terminated,Error} 
    class MyTxtFile:IEquatable<MyTxtFile>
    {
        public MyTxtFile(string fullname)
        {
            fullFileName = fullname;
        }
        public MyTxtFile(string fullname, string name, TxtType type)
        {
            fullFileName = fullname;
            fileName = name;
            txtType = type;
        }
        public string fullFileName { get; set; }
        public string fileName { get; set; }
        public TxtType txtType { get; set; }
        public override string ToString()
        {
            return "FileFullName:"+fullFileName+" ReportType:"+txtType;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            MyTxtFile objAsMyTxt = obj as MyTxtFile;
            if (objAsMyTxt == null) return false;
            else return base.Equals(objAsMyTxt);
        }
        public bool Equals(MyTxtFile other)
        {
            if (other == null) return false;
            return (this.fullFileName.Equals(other.fullFileName));
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
