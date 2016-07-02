using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SV_TestReport_Collection_Main
{
    public static class Constants
    {
        private enum EV { RunKey = 0, SourceMark = 1, ProductID = 2, EquipMentFunction = 3, SericalNumber = 4, StationName = 5, WPT = 6, CC = 7, WPN = 8, EventDateTime = 9, TestProgramName = 10, UserLoginName = 11, ExecutionTime = 12, TestSocket = 13, BatchSerialNumber = 14, ErrorCode = 15, ErrorMessage = 16, ProcessKey = 17, PreSerial = 18, DefinesThenProcessType = 19, CallSpecialDefinedFunction = 20, DurationOfEventInSeconds = 21, Dummy1 = 22, Dummy2 = 23, Dummy3 = 24, ResultOfRepairaction = 25, CountOfRepairedBoards = 26, Dummy4 = 27, Dummy5 = 28, Dummy6 = 29, TestStatus = 30 }
        private enum ME { StepOrder = 0, Source = 1, EventDateTime = 2, ErrorCode = 3, ErrorMessage = 4, TotalTime = 5, TestStepName = 6, TestStatus = 7, ReportText = 8, NumLoops = 9, NumPassed = 10, NumFailed = 11, StepGroup = 12, StepType = 13, StepPassFail = 14, MultipleSubName = 15, Units = 16, Comp = 17, StepData = 18, LimitLow = 19, LimitHigh = 20, ResultString = 21, LimitsString = 22 }   
        private static List<string> myEVtxt = new List<string>();
        private static List<string> myMEtxt = new List<string>();
        public static List<int> EVlist = new List<int>();
        public static List<int> MElist = new List<int>();

        //public static int Me01 = 0;
        //public static int Me02 = 1;
        //public static int Me03 = 3;
        //public static int Me04 = 4;
        //public static int Me05 = 5;
        //public static int Me06 = 7;
        //public static int Me07 = 8;
        //public static int Me08 = 9;
        //public static int Me09 = 10;
        //public static int Me10 = 13;
        //public static int Me11 = 14;
        //public static int Me12 = 15;
        //public static int Me13 = 19;
        //public static int Me14 = 20; 
        //public static int Me15 = 21;
        //public static int Me16 = 22;
        //public static int Me17 = 23;
        //public static int Me18 = 24;
        //public static int Me19 = 25;
        //public static int Me20 = 26;
        //public static int Me21 = 27;
        //public static int Me22 = 28;
        //public static int Me23 = 29;

      
        private static void SetEVlist()
        {
            EVlist.Clear();
            
            foreach (string s in Enum.GetNames(typeof(EV)))  //循环数据库表字段，确保数据库表中所有字段都经过了处理
            {
                bool isFind = false;
                for (int i = 0; i < myEVtxt.Count();++i )
                {
                    if (s == myEVtxt[i])
                    {
                        //EVlist.Add((int)(Enum.Parse(typeof(EV), s, true)));  //匹配文本中的对应字段是第几个字段
                        EVlist.Add(i);
                        isFind = true;
                        break;
                    }
                   
                }
                if(!isFind)
                    EVlist.Add(-1);   //文本中找不到对应的字段,则将文本中的字段位置标记为-1；
            }



        }

        public static void SetEVandME(TxtType type)
        {
            string configPath="";
            switch (type)
            {
                case TxtType.Passed: configPath = "Passed.ini"; break;
                case TxtType.Failed: configPath = "Failed.ini"; break;
                case TxtType.Error: break;
                case TxtType.Terminated: break;
                default: break;
            }

            #region 读取字段配置文件
            try
            {
                string line;
                myEVtxt.Clear();
                myMEtxt.Clear();
                int which=-1;
                using (StreamReader sr = new StreamReader(configPath, Encoding.Default))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("--EV--"))
                        {
                            which = 0;
                            continue;
                        }
                        if (line.Contains("--ME--"))
                        {
                            which = 1;
                            continue;
                        }
                        if (which == 1)
                        {
                            myMEtxt.Add(line);
                        }
                        if (which == 0)
                        {
                            myEVtxt.Add(line);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            } 
            #endregion
            SetEVlist();
            SetMElist();


        }

        private static void SetMElist()
        {
            MElist.Clear();
            foreach (string s in Enum.GetNames(typeof(ME))) //循环数据库表字段，确保数据库表中所有字段都经过了处理
            {
                bool isFind = false;
                for (int i = 0; i < myMEtxt.Count(); ++i)
                {
                    if (s == myMEtxt[i])
                    {
                       // MElist.Add((int)(Enum.Parse(typeof(ME), s, true)));//匹配文本中的对应字段是第几个字段
                        MElist.Add(i);
                        isFind = true;
                        break;
                    }
                      
      
                }
                if(!isFind)
                    MElist.Add(-1);       //文本中找不到对应的字段,则将文本中的字段位置标记为-1；   
            }
        }
    }
}
