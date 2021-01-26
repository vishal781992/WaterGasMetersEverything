using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaterGasTool
{
    public partial class MeterUploadPOP : Form
    {
        public string DBServername, DBName, DBUser, DBPass, MeterTypeCodeString, StatusCodeString, WaterFirmwareString, GasfirmwareString;
        int[] StatusCodesModem = new int[] { 0, 1, 20, 21, 40, 41, 50, 60, 90, 91 };

        public List<dynamic> MeterTypeCode = new List<dynamic>();
        public MeterUploadPOP(string Servername, string Name, string User, string Pass)
        {
            InitializeComponent(); this.DBServername = Servername; this.DBName = Name; this.DBUser = User; this.DBPass = Pass;
        }

        private void textBoxPOP_WaterFw_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            System.Windows.Forms.TextBox textbox = sender as System.Windows.Forms.TextBox;

            if (textbox == null)
                return;

            if (!char.IsControl(ch) && (!char.IsNumber(ch)) && (!char.IsLetter(ch)) && (ch != '.'))
                e.Handled = true;
        }

        private BackgroundWorker myBackgroundWorker;//myBackgroundWorker.RunWorkerAsync(2)
        private void MeterUploadPOP_Load(object sender, EventArgs e)
        {
            myBackgroundWorker = new BackgroundWorker();
            myBackgroundWorker.WorkerReportsProgress = true;
            myBackgroundWorker.WorkerSupportsCancellation = false;
            myBackgroundWorker.DoWork += myBackgroundWorker_DoWork;
            myBackgroundWorker.RunWorkerCompleted += myBackgroundWorker_RunWorkerCompleted;
            myBackgroundWorker.ProgressChanged += myBackgroundWorker_ProgressChanged;
            label1.Text = "List Updating...";
            myBackgroundWorker.RunWorkerAsync(1);
        }

        private void comboBox_StatusCode_MouseEnter(object sender, EventArgs e)
        {
            comboBoxPOP_StatusCode.DataSource = StatusCodesModem;
        }

        private void buttonPOP_Done_MouseEnter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxPOP_WaterFw.Text))
                textBoxPOP_WaterFw.Text = "Z.ZZ.Z";
            if (string.IsNullOrEmpty(textBoxPOP_GasFW.Text))
                textBoxPOP_GasFW.Text = "Z.ZZ.Y";
            textBoxPOP_WaterFw.Text = textBoxPOP_WaterFw.Text.ToUpper(); textBoxPOP_GasFW.Text = textBoxPOP_GasFW.Text.ToUpper();
            this.MeterTypeCodeString = comboBoxPOP_MeterTypeCode.Text; this.StatusCodeString = comboBoxPOP_StatusCode.Text; this.WaterFirmwareString = textBoxPOP_WaterFw.Text; this.GasfirmwareString = textBoxPOP_GasFW.Text;
            if (textBoxPOP_WaterFw.Text.Contains("Z.ZZ.Z"))
                textBoxPOP_WaterFw.Clear();
            if (textBoxPOP_GasFW.Text.Contains("Z.ZZ.Y"))
                textBoxPOP_GasFW.Clear();
        }

        #region myBackgroundWorker
        private void myBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //BackgroundWorker worker = sender as BackgroundWorker;
            SQLQueries SQL = new SQLQueries(DBServername + ";", DBName, DBUser, DBPass);
            MeterTypeCode =  SQL.GetDataFromDB("MeterTypeCode", "MeterType");

        }

        private void myBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            comboBoxPOP_MeterTypeCode.DataSource = MeterTypeCode;
            label1.Text = "List Updated!";
        }

        private void myBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }
        #endregion #region myBackgroundWorker
    }
}

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.IO;
//using System.Diagnostics;
//using System.Windows.Forms;
//using System.ComponentModel;

//namespace WaterGasTool
//{
//    public partial class MeterUploadPOP : Form
//    {
//        //private BackgroundWorker myBackgroundWorker;
//        private string DBServername, DBName, DBUser, DBPass;
//        public MeterUploadPOP(string Servername, string Name, string User, string Pass)
//        {
//            this.DBServername = Servername; this.DBName = Name; this.DBUser = User; this.DBPass = Pass;
//        }

//        public MeterUploadPOP()
//        {
//            InitializeComponent();

//            myBackgroundWorker = new BackgroundWorker();
//            myBackgroundWorker.WorkerReportsProgress = true;
//            myBackgroundWorker.WorkerSupportsCancellation = false;
//            myBackgroundWorker.DoWork += myBackgroundWorker1_DoWork;
//            myBackgroundWorker.RunWorkerCompleted += myBackgroundWorker1_RunWorkerCompleted;
//            myBackgroundWorker.ProgressChanged += myBackgroundWorker1_ProgressChanged;
//        }

//        private void comboBox_MeterTypeCode_Enter(object sender, EventArgs e)
//        {

//        }

//        private void MeterUpload_Enter(object sender, EventArgs e)
//        {

//        }

//        private BackgroundWorker myBackgroundWorker;//myBackgroundWorker.RunWorkerAsync(2)
//        #region myBackgroundWorker
//        private void myBackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
//        {
//            BackgroundWorker worker = sender as BackgroundWorker;
//            SQLQueries SQL = new SQLQueries(DBServername + ";", DBName, DBUser, DBPass);
//            //comboBox_MeterTypeCode.DataSource = SQL.GetDataFromDB("MeterTypeCode", "MeterType");

//        }

//        private void myBackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
//        {

//        }

//        private void myBackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
//        {
//        }
//        #endregion #region myBackgroundWorker
//    }
//}

