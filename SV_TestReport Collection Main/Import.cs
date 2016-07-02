using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV_TestReport_Collection_Main
{
    public class Import
    {
        private string connection;
        private List<string[]> _content=new List<string[]>();

        public Import(string ConnStr)
        {
            connection = ConnStr;
        }
        /// <summary>
        /// 文本数据读入内存
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool readFromTxt(string url)
        {
            
            try
            {
                if (_content.Count != 0)
                {
                    _content.Clear();
                }
                //FileStream file = new FileStream(url, FileMode.Open);
                //file.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(url, Encoding.Default))
                {
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
                }
                //Console.WriteLine("------------------EV---------------------");
                //for (int i = 1; i < _content[0].Count() && i < _content[1].Count(); i++)
                //{
                //    Console.WriteLine(i + "      " + _content[0][i - 1] + "-----" + _content[1][i - 1]);
 
                //}
                //if (_content[0].Count() != _content[1].Count())
                //    Console.WriteLine("!!!!!" + (_content[0].Count() - 1) + "--------" + (_content[1].Count() - 1));

                //Console.WriteLine("------------------ME---------------------");
                //for (int i = 1; i < _content[3].Count(); i++)
                //{
                //    Console.WriteLine(i + "      " + _content[3][i - 1] + "-----" + _content[4][i - 1]);
                //}
                return true;
            }
            catch (IOException e)
            {
                throw e;
            }
        }

        #region ImportToMEtable( string mode, out int count)
        /// <summary>
        /// 将数据导入ME表
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool ImportToMEtable( TxtType type, out int count)
        {
            count = 0;
            try { Constants.SetEVandME(type); }
            catch (Exception ex)
            {
                throw (ex);
            }

            using (SqlConnection conn = new SqlConnection(connection))
            {

                conn.Open();
                using (SqlTransaction myTrans = conn.BeginTransaction())//建立一个事务
                {
                    try
                    {
                        SqlCommand myComm = conn.CreateCommand();
                        myComm.Transaction = myTrans;
                        String sql = "InsertToME"; 
                            //"insert into [ME_MSTR Data] values(@a1,@a2,@a3,@a4,@a5,@a6,@a7,@a8,@a9,@a10,@a11,@a12,@a13,@a14,@a15,@a16,@a17,@a18,@a19,@a20,@a21,@a22,@a23)";

                        int n = 0;
                        while (n < _content.Count() && _content[n][0].IndexOf("Step_Order") < 0)//以Step_order作为关键字来开始解析测试数据
                        { n++; }

                        SqlParameter returnPar = new SqlParameter("@return", SqlDbType.Int);
                        returnPar.Direction = ParameterDirection.Output;

                        SqlParameter a1 = new SqlParameter("a1", null);
                        SqlParameter a2 = new SqlParameter("a2", null);
                        SqlParameter a3 = new SqlParameter("a3", null);
                        SqlParameter a4 = new SqlParameter("a4", null);
                        SqlParameter a5 = new SqlParameter("a5", null);
                        SqlParameter a6 = new SqlParameter("a6", null);
                        SqlParameter a7 = new SqlParameter("a7", null);
                        SqlParameter a8 = new SqlParameter("a8", null);
                        SqlParameter a9 = new SqlParameter("a9", null);
                        SqlParameter a10 = new SqlParameter("a10", null);
                        SqlParameter a11 = new SqlParameter("a11", null);
                        SqlParameter a12 = new SqlParameter("a12", null);
                        SqlParameter a13 = new SqlParameter("a13", null);
                        SqlParameter a14 = new SqlParameter("a14", null);
                        SqlParameter a15 = new SqlParameter("a15", null);
                        SqlParameter a16 = new SqlParameter("a16", null);
                        SqlParameter a17 = new SqlParameter("a17", null);
                        SqlParameter a18 = new SqlParameter("a18", null);
                        SqlParameter a19 = new SqlParameter("a19", null);
                        SqlParameter a20 = new SqlParameter("a20", null);
                        SqlParameter a21 = new SqlParameter("a21", null);
                        SqlParameter a22 = new SqlParameter("a22", null);
                        SqlParameter a23 = new SqlParameter("a23", null);

                        SqlParameter[] paras = new SqlParameter[]  //作为数据表中的字段匿名
                            {
                                a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11,a12,a13,a14,a15,a16,a17,a18,a19,a20,a21,a22,a23,returnPar
                            };
                        myComm.CommandType = CommandType.StoredProcedure;
                        myComm.Parameters.AddRange(paras);
                        myComm.CommandText = sql;

                        for (int i = n + 1; i < _content.Count(); ++i)//批量插入数据
                        {
                            a1.Value = GetContent(i,Constants.MElist[0]);
                            a2.Value = GetContent(i, Constants.MElist[1]);
                            a3.Value = GetContent(i, Constants.MElist[2]);
                            a4.Value = GetContent(i, Constants.MElist[3]);
                            a5.Value = GetContent(i, Constants.MElist[4]);
                            a6.Value = GetContent(i, Constants.MElist[5]);
                            a7.Value = GetContent(i, Constants.MElist[6]);
                            a8.Value = GetContent(i, Constants.MElist[7]);
                            a9.Value = GetContent(i, Constants.MElist[8]);
                            a10.Value = GetContent(i, Constants.MElist[9]);
                            a11.Value = GetContent(i, Constants.MElist[10]);
                            a12.Value = GetContent(i, Constants.MElist[11]);
                            a13.Value = GetContent(i, Constants.MElist[12]);
                            a14.Value = GetContent(i, Constants.MElist[13]);
                            a15.Value = GetContent(i, Constants.MElist[14]);
                            a16.Value = GetContent(i, Constants.MElist[15]);
                            a17.Value = GetContent(i, Constants.MElist[16]);
                            a18.Value = GetContent(i, Constants.MElist[17]);
                            a19.Value = GetContent(i, Constants.MElist[18]);
                            a20.Value = GetContent(i, Constants.MElist[19]);
                            a21.Value = GetContent(i, Constants.MElist[20]);
                            a22.Value = GetContent(i, Constants.MElist[21]);
                            a23.Value = GetContent(i, Constants.MElist[22]);

                            myComm.ExecuteNonQuery();
                            if ((int)returnPar.Value >= 1) count++;
                        }
                        myTrans.Commit();//提交事务
                    }
                    catch (Exception e)
                    {
                        myTrans.Rollback();//事务回滚
                        Console.WriteLine("[导入失败]" + e.ToString());
                        count = 0;
                        throw e;
                    }

                    return true;
                }
            }
        } 
        #endregion



        #region public bool ImportToEVtable(out int count)
        /// <summary>
        /// 将数据导入EV表
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool ImportToEVtable(TxtType type, out int count)
        {
            count = 0;

            try { Constants.SetEVandME(type); }
            catch (Exception ex)
            {
                throw (ex);
            }
            using (SqlConnection conn = new SqlConnection(connection))
            {

                conn.Open();
                using (SqlTransaction myTrans = conn.BeginTransaction())//建立一个事务
                {
                    try
                    {
                        SqlCommand myComm = conn.CreateCommand();
                        myComm.Transaction = myTrans;
                        String sql = "insert into [EV_MSTR Data] values(@a1,@a2,@a3,@a4,@a5,@a6,@a7,@a8,@a9,@a10,@a11,@a12,@a13,@a14,@a15,@a16,@a17,@a18,@a19,@a20,@a21,@a22,@a23,@a24,@a25,@a26,@a27,@a28,@a29,@a30,@a31)";

                        int n = 0;
                        while (n < _content.Count() && _content[n][0].IndexOf("UUT_Order") < 0)//以UUT_Order作为关键字来开始解析测试数据
                        { n++; }

                        int end = n + 1;
                        while (end < _content.Count())
                        {
                            try
                            {
                                int.Parse(_content[end][0]);
                                ++end;
                            }
                            catch (Exception ex)
                            {
                                break;
                            }
                        }

                        SqlParameter returnPar = new SqlParameter("@return", SqlDbType.Int);
                        returnPar.Direction = ParameterDirection.Output;

                        SqlParameter a1 = new SqlParameter("a1", null);
                        SqlParameter a2 = new SqlParameter("a2", null);
                        SqlParameter a3 = new SqlParameter("a3", null);
                        SqlParameter a4 = new SqlParameter("a4", null);
                        SqlParameter a5 = new SqlParameter("a5", null);
                        SqlParameter a6 = new SqlParameter("a6", null);
                        SqlParameter a7 = new SqlParameter("a7", null);
                        SqlParameter a8 = new SqlParameter("a8", null);
                        SqlParameter a9 = new SqlParameter("a9", null);
                        SqlParameter a10 = new SqlParameter("a10", null);
                        SqlParameter a11 = new SqlParameter("a11", null);
                        SqlParameter a12 = new SqlParameter("a12", null);
                        SqlParameter a13 = new SqlParameter("a13", null);
                        SqlParameter a14 = new SqlParameter("a14", null);
                        SqlParameter a15 = new SqlParameter("a15", null);
                        SqlParameter a16 = new SqlParameter("a16", null);
                        SqlParameter a17 = new SqlParameter("a17", null);
                        SqlParameter a18 = new SqlParameter("a18", null);
                        SqlParameter a19 = new SqlParameter("a19", null);
                        SqlParameter a20 = new SqlParameter("a20", null);
                        SqlParameter a21 = new SqlParameter("a21", null);
                        SqlParameter a22 = new SqlParameter("a22", null);
                        SqlParameter a23 = new SqlParameter("a23", null);
                        SqlParameter a24 = new SqlParameter("a24", null);
                        SqlParameter a25 = new SqlParameter("a25", null);
                        SqlParameter a26 = new SqlParameter("a26", null);
                        SqlParameter a27 = new SqlParameter("a27", null);
                        SqlParameter a28 = new SqlParameter("a28", null);
                        SqlParameter a29 = new SqlParameter("a29", null);
                        SqlParameter a30 = new SqlParameter("a30", null);
                        SqlParameter a31 = new SqlParameter("a31", null);
                        SqlParameter[] paras = new SqlParameter[]  //作为数据表中的字段匿名
                            {
                                a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11,a12,a13,a14,a15,a16,a17,a18,a19,a20,a21,a22,a23,a24,a25,a26,a27,a28,a29,a30,a31,returnPar
                            };

                        myComm.Parameters.AddRange(paras);
                        myComm.CommandType = CommandType.StoredProcedure;
                        myComm.CommandText = "InsertToEV";
                            //sql;

                        for (int i = n + 1; i < end; ++i)//批量插入数据
                        {
                            a1.Value = GetContent(i, Constants.EVlist[0]);
                            a2.Value = GetContent(i, Constants.EVlist[1]);
                            a3.Value = GetContent(i, Constants.EVlist[2]);
                            a4.Value = GetContent(i, Constants.EVlist[3]);
                            a5.Value = GetContent(i, Constants.EVlist[4]);
                            a6.Value = GetContent(i, Constants.EVlist[5]);
                            a7.Value = GetContent(i, Constants.EVlist[6]);
                            a8.Value = GetContent(i, Constants.EVlist[7]);
                            a9.Value = GetContent(i, Constants.EVlist[8]);
                            a10.Value = GetContent(i, Constants.EVlist[9]);
                            a11.Value = GetContent(i, Constants.EVlist[10]);
                            a12.Value = GetContent(i, Constants.EVlist[11]);
                            a13.Value = GetContent(i, Constants.EVlist[12]);
                            a14.Value = GetContent(i, Constants.EVlist[13]);
                            a15.Value = GetContent(i, Constants.EVlist[14]);
                            a16.Value = GetContent(i, Constants.EVlist[15]);
                            a17.Value = GetContent(i, Constants.EVlist[16]);
                            a18.Value = GetContent(i, Constants.EVlist[17]);
                            a19.Value = GetContent(i, Constants.EVlist[18]);
                            a20.Value = GetContent(i, Constants.EVlist[19]);
                            a21.Value = GetContent(i, Constants.EVlist[20]);
                            a22.Value = GetContent(i, Constants.EVlist[21]);
                            a23.Value = GetContent(i, Constants.EVlist[22]);
                            a24.Value = GetContent(i, Constants.EVlist[23]);
                            a25.Value = GetContent(i, Constants.EVlist[24]);
                            a26.Value = GetContent(i, Constants.EVlist[25]);
                            a27.Value = GetContent(i, Constants.EVlist[26]);
                            a28.Value = GetContent(i, Constants.EVlist[27]);
                            a29.Value = GetContent(i, Constants.EVlist[28]);
                            a30.Value = GetContent(i, Constants.EVlist[29]);
                            a31.Value = GetContent(i, Constants.EVlist[30]);

                            myComm.ExecuteNonQuery();
                                
                            if ((int)returnPar.Value >= 1) count++;
                        }
                        myTrans.Commit();//提交事务
                        //count = end - n - 1;
                    }
                    catch (Exception e)
                    {
                        count = 0;
                        myTrans.Rollback();//事务回滚
                        Console.WriteLine("[导入失败]" + e.ToString());
                        Console.ReadLine();
                        throw e;
                    }

                    return true;
                }
                
            }
        } 
        #endregion


        private object GetContent(int i,int l)
        {
            if (l == -1)
            {
                return "";
                    //DBNull.Value;
            }
            else
            {
                return _content[i][l];
            }
        }
     
    }
}
