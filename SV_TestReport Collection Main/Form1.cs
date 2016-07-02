using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Configuration;

namespace SV_TestReport_Collection_Main
{
    public partial class Form1 : Form
    {
        private SynchronizationContext syn;
        private void errorExit(object state)
        {
            this.Close();
            this.Dispose(true);
        }
        private Import import;
        private FileOperate fileOP;
        
        private BackgroundWorker worker = null;
        private delegate void UpdateListView(object subItem);
        private void UpdateListViewMethod(object subItem)//更新UI方法
        {
            logLv.Items.Add((ListViewItem)subItem);
        }
        private delegate void UpdateFileName(string name);
        private void UpdateFileNameMethod(string name)
        {
            textBox2.Text = name ;
        }

        private BackgroundWorker worker1 = null;
        private delegate void UpdateTxtCount(string count);
        private void UpdateTxtCountMethod(string count)
        {
            fileCount.Text = count;
        }

        private List<string[]> _content = new List<string[]>();

        public Form1()
        {
            syn = SynchronizationContext.Current;
            //MessageBox.Show();
            import = new Import(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            InitializeComponent();
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(ThreadFunction);
            worker1 = new BackgroundWorker();
            worker1.DoWork += new DoWorkEventHandler(ThreadCount);
            //worker.WorkerReportsProgress = true;
            //worker.ProgressChanged += (WorkerForChanged);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timeTxt.Text = ConfigurationManager.AppSettings["ScanTime"];
            textBox1.Text = ConfigurationManager.AppSettings["ReportPath"];
            string completedPath = ConfigurationManager.AppSettings["CompletedPath"];
            string failedPath=ConfigurationManager.AppSettings["FailedPath"];
            fileOP=new FileOperate(textBox1.Text,completedPath,failedPath);
            worker1.RunWorkerAsync();
        }

        ////文件路径输入框点击事件
        //private void textBox1_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog fileDialog = new OpenFileDialog();
        //    fileDialog.Filter = "(txt文本)|*.txt";
        //    if (fileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        string extension = Path.GetExtension(fileDialog.FileName);
        //        string strExt = ".txt";
        //        if (strExt != extension)
        //        {
        //            MessageBox.Show("请上传txt格式的文本数据！");
        //        }
        //        else
        //        {

        //            textBox1.Text = fileDialog.FileName;
        //            textBox2.Text = fileDialog.FileName.Substring(fileDialog.FileName.LastIndexOf('\\')+1);
        //        }


        //    }

        //}

        //Begin按钮事件
        private void beginBtn_Click(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();

            beginBtn.Enabled = false;
            timeTxt.Enabled = false;
        }

        //private void WorkerForChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    logLv.Items.Add((ListViewItem)e.UserState);
        //}


        //线程运行代码
        private void ThreadFunction(object sender,DoWorkEventArgs e)
        {
            while (true)
            {
                bool isOK = true;
                if (fileOP.getFileInfos().Count() > 0)
                {
                    //if (!File.Exists(textBox1.Text))
                    //{
                    //    MessageBox.Show("文件不存在！");
                    //    return;
                    //}
                    //else
                    //{
                    int count = 0;
                    long toEVtime = 0, toMEtime = 0;
                    Stopwatch sw = new Stopwatch();
                    MyTxtFile currFile = fileOP.getFileInfos()[0];
                    this.Invoke(new UpdateFileName(UpdateFileNameMethod), currFile.fileName);
                    import.readFromTxt(currFile.fullFileName);

                    try
                    {

                        ListViewItem lvi = new ListViewItem();

                        sw.Start();
                        import.ImportToEVtable(currFile.txtType, out count);
                        sw.Stop();
                        toEVtime = sw.ElapsedMilliseconds;

                        lvi.Text = (DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "|" + "导入EV表记录：" + count + "|耗时：" + toEVtime + "ms");
                        //logLv.Items.Add(lvi);
                        //worker.ReportProgress(1, lvi);
                        this.BeginInvoke(new UpdateListView(UpdateListViewMethod), lvi);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("[Import to EV ERROR]" + ex.ToString());
                        isOK = false;
                    }
                    try
                    {

                        ListViewItem lvi = new ListViewItem();

                        sw.Start();
                        import.ImportToMEtable(currFile.txtType, out count);
                        sw.Stop();
                        toMEtime = sw.ElapsedMilliseconds;

                        lvi.Text = (DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "|" + "导入ME表记录：" + count + "|耗时：" + toEVtime + "ms");
                        //logLv.Items.Add(lvi);
                        //worker.ReportProgress(1, lvi);  
                        //委托主线程更新UI
                        this.BeginInvoke(new UpdateListView(UpdateListViewMethod), lvi);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("[Import to ME ERROR]" + ex.ToString());
                        isOK = false;

                    }
                    fileOP.moveFile((fileOP.getFileInfos())[0].fullFileName, fileOP.getFileInfos()[0].fileName,isOK);

                }
                Thread.Sleep((int)(float.Parse(timeTxt.Text)*1000));
            }
        }
        //文件计数线程运行代码
        private void ThreadCount(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    this.Invoke(new UpdateTxtCount(UpdateTxtCountMethod), fileOP.getFileInfos().Count().ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    syn.Post(errorExit, null);
                    
                }
                Thread.Sleep(1000);
            }
        }


        //窗口大小变化事件（最小化是缩到托盘）
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                this.Hide();

                InitializNotifyicon();

            }
        }

        //双击托盘图标事件
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
                this.WindowState = FormWindowState.Normal;
            }
        }
        //初始化托盘菜单
        private void InitializNotifyicon()
        {
            MenuItem[] menuItems = new MenuItem[3];
            menuItems[0] = new MenuItem();
            menuItems[0].Text = "显示窗口";
            menuItems[0].Click += new System.EventHandler(this.notifyIcon1_showform);
            menuItems[1] = new MenuItem("-");

            menuItems[2] = new MenuItem();
            menuItems[2].Text = "退出系统";
            menuItems[2].Click += new System.EventHandler(this.exit);
            menuItems[2].DefaultItem = true;

            ContextMenu cm = new ContextMenu(menuItems);

            notifyIcon1.ContextMenu = cm;

        }

        //还原窗口处理
        private void notifyIcon1_showform(object sender, System.EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
                this.WindowState = FormWindowState.Normal;
            }
        }

        //系统退出处理
        private void exit(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("确认退出吗？", "退出系统", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                notifyIcon1.Visible = false;
                this.Close();
                this.Dispose(true);
            }
        }

        //窗口关闭事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
            this.Form1_SizeChanged(null, null);

        }

        //Exit按钮事件
        private void exitBtn_Click(object sender, EventArgs e)
        {
            exit(sender, e);
        }
      

        // 时间输入框数字检测
        private void timeTxt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float.Parse(timeTxt.Text);
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["ScanTime"].Value = timeTxt.Text;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入数字！");
                timeTxt.Text = ConfigurationManager.AppSettings["ScanTime"];
            }
        }

     

 
        
    }
}
