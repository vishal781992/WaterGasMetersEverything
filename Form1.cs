using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Net;
using Microsoft.VisualBasic.FileIO;

namespace WaterGasTool
{
    public partial class Form1 : Form
    {

        #region Declaration

        List<string> filenameArray          = new List<string>();
        public List<string> DevEUI_Main     = new List<string>();
        public List<string> AppKey_Main     = new List<string>();
        public List<string> CustVer_Main    = new List<string>();
        public List<string> FwVer_Main      = new List<string>();
        public List<string> Time_Main       = new List<string>();
        public List<string> Position_Main   = new List<string>();
        public List<string> SerialNumber_Main = new List<string>();
        public List<string> SrNum_Main      = new List<string>();
        public List<long> SrNum_Long        = new List<long>();

        public List<string> EndsightStatus_M  = new List<string>();
        public List<string> EndsightDevUI_M   = new List<string>();
        public List<string> EndsightSerialNumber_M = new List<string>();

        public List<string> statusCode = new List<string>();
        public List<string> tempEndMeterID = new List<string>();

        string[] TempArrayForDisplayContent = new string[10];       //for displaying the table
        public string[] Duplicates          = new string[700];
        string[] tempAryforDuplicates       = new string[30];
        string[] displayConfigArray         = new string[1];

        public string   serverURL           = string.Empty, 
                        serviceProfileID    = string.Empty,
                        deviceProfileID     = string.Empty, CSPassword = string.Empty,
                        LogFileAddress      = string.Empty;
        public string   DBServername, DBName, DBUser, DBPass, VersionFromConfig, endsightRootFolder;
        string          Endsight_File1FullPath, Endsight_File1Name;
        string          CompleteLogAddress;
        string          FileAddress;

        const string version                = "V "+"0.0.2";
        const string DataMergerFileStorage  = @"\\netserver3\DATA\Loraproduction_Engineering\LGW\Data\";
        const string ConfigFilepath         = @"\\netserver3\DATA\Loraproduction_Engineering\LGW\config\ConfigFile.txt";
        const string FileFormat             = ".csv";
        public const string FileOutputForErrorSheets_tab2 = @"\\netserver3\DATA\Loraproduction_Engineering\LGW\Endsight\ErrorSheets";
        public string FormatStringType      = string.Empty;
        public int  CounterForUpdatedMetersToDB = 0, CounterForCorrectDataSet = 0;

        bool flag_ListviewOption        = false;
        bool flag_NoAuthentication      = true;

        ListViewItem itm;

        #endregion Declaration
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label_upgradeAvl.Visible = false;
            if (File.Exists(ConfigFilepath))
                GetdataFromConfig();
            else
            {
                WriteDataToConfigOnce();
                Close();
                Thread.Sleep(200);
                GetdataFromConfig();
            }
            this.Text = "LoraGasWater "+" (BETA) "+ version;
            string Month = DateTime.Now.ToString("MMMM");
            string year = DateTime.Now.ToString("yyyy");

            CompleteLogAddress = LogFileAddress + @"\Log_"+ Month+"_"+ year+".txt";
            if (!File.Exists(CompleteLogAddress))
                File.Create(CompleteLogAddress);

            monthCalendar1.Visible = false; listView1.Visible = false; label_authenticator.Visible = false;
            textBox_SerialNumber_minVal.Visible = false; textBox_SerialNumber_maxVal.Visible = false;
            label3.Visible = false;progressBar_universal.Visible = false; label_MeterRange.Visible = false; label_Path.Visible = false; label_HeadingToPath.Visible = false;
            label_authenticator.Visible = false; listView_AMR.Visible = false; label7.Visible = false; label8.Visible = false; progressBar_AMR.Visible = false; label9.Visible = false;
            label_Date.Text = "Date: "+DateTime.Now.ToString("MM/dd/yyyy");

            string Host = Dns.GetHostName();
            string IP_Comp = Dns.GetHostByName(Host).AddressList[0].ToString();
            Loghandler LG = new Loghandler(CompleteLogAddress);
            LG.WriteToFile(true, "Host: " + Host + ", IP: " + IP_Comp+", App Opened");

            //documentation
            richTextBox_Documentation.Text = "Links to the Important folders are as follows.\r\n" +
                "\r\nFolder to access the latest software setup::\r\n" +
                @"\\netserver3\DATA\Loraproduction_Engineering\LGW\Software_setup_File" +
                "\r\nFolder to access merged Files::\r\n" +
                @"\\netserver3\DATA\Loraproduction_Engineering\LGW\Data" +
                "\r\nParent Network Folder for all the Computers::\r\n" +
                @"\\netserver3\DATA\Loraproduction_Engineering\LGW"+
                "\r\nDocumentation is saved here::\r\n" +
                @"\\netserver3\DATA\Loraproduction_Engineering\LGW\Documentation";
            richTextBox_Documentation.AppendText("\r\n\r\nAny queries:: Vishal@visionmetering.com (mention IP address fro your computer while writing the Email)");
            richTextBox_Documentation.AppendText("\r\n\r\nversion currently running: "+version+" and version Available is "+ VersionFromConfig);
            richTextBox_Documentation.AppendText("\r\n\r\nIP address: " + IP_Comp + " and Your ComputerName: " + Host);

            richTextBox_AMR.Text = "Please verify that the Endsight root folder is correct before Doing AMR check.\r\nEndsight RootFolder: "+ endsightRootFolder;
        }

        #region Button Clicks
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox_dateShow.Text))
            {
                listView1.Visible = false; flag_ListviewOption = false; checkBox_ViewList.Checked = false;
                progressBar_universal.Visible = true;
                progressBar_universal.Minimum = 0; progressBar_universal.Maximum = 100;
                progressBar_universal.Value = progressBar_universal.Minimum;
                progressBar_universal.Value += 10;//1
                VarClearFuntion();
                progressBar_universal.Value += 10;//2
                DateSpecificFileSelect();
                progressBar_universal.Value += 10;//3
                if (File.Exists(DataMergerFileStorage + "AutoMergerFor_" + textBox_dateShow.Text + FileFormat))
                    File.Delete(DataMergerFileStorage + "AutoMergerFor_" + textBox_dateShow.Text + FileFormat);
                progressBar_universal.Value += 10;//4
                FIleMerger();
                progressBar_universal.Value += 10;//5
                DataInOut Din = new DataInOut();
                Din.CSVFileExtractor(DataMergerFileStorage + "AutoMergerFor_" + textBox_dateShow.Text + FileFormat);
                #region saving to main
                this.DevEUI_Main = Din.DevEUI;
                this.AppKey_Main = Din.AppKey;
                this.CustVer_Main = Din.CustVer;
                this.FwVer_Main = Din.FwVer;
                #endregion saving to main end
                progressBar_universal.Value += 10;//6
                Get_DevEUI();
                progressBar_universal.Value += 10;//7
                DuplicateFinder();
                progressBar_universal.Value += 10;//8
                //listView1.Clear();
                DisplayListView();

                progressBar_universal.Value = progressBar_universal.Maximum; Thread.Sleep(2000);
                progressBar_universal.Visible = false;
                flag_ListviewOption = true; checkBox_ViewList.Checked = true; checkBox_ViewList.ForeColor = Color.Green;
                checkBox_MeterRange.Visible = true;

                foreach (string SerialNum in SerialNumber_Main)
                {
                    if (long.TryParse(SerialNum, out long SerialNum_long))
                        SrNum_Long.Add(SerialNum_long);
                }
                try
                {

                    SrNum_Long.Sort();
                    label_MeterRange.Visible = true;
                    label_MeterRange.Text = "Serial No. Range " + SrNum_Long[0] + " <--> " + SrNum_Long[SrNum_Long.Count - 1];
                }
                catch { label_MeterRange.Text = "Serial No. Range not available"; checkBox_MeterRange.Visible = false; }

                //log
                Loghandler LG = new Loghandler(CompleteLogAddress);
                LG.WriteToFile(true, textBox_dateShow.Text+" Files are merged");
                LG.WriteToFile(true, DevEUI_Main.Count+" Records are merged into 1 file.");
            }
            else { MessageBox.Show("Select the date, First!"); Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, "Error: Select the date, First!"); }
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            int result = 0; int result01 = 0;int counterForSuccessPOST = 0; int counterForUNSuccessPOST = 0; int counterForValidSerial = 0;
            
            progressBar_universal.Minimum = 0; progressBar_universal.Maximum = DevEUI_Main.Count+50;
            progressBar_universal.Value = progressBar_universal.Minimum;
            if(!string.IsNullOrEmpty(textBox_ApplName.Text))
                textBox_ApplName.Text = "LGW_" + textBox_ApplName.Text;
           
           if(!AuthenticationPopup())//flag_NoAuthentication
            {
                progressBar_universal.Visible = true;
                flag_NoAuthentication = true;
                if (string.IsNullOrEmpty(textBox_ApplDesp.Text))
                    textBox_ApplDesp.Text = "Date__" + textBox_dateShow.Text;

                progressBar_universal.Value += 10;//1
                APICalls Chirp = new APICalls(serverURL, serviceProfileID, deviceProfileID);
                if (int.Equals(await Chirp.ChirPostLogin(), 21))
                {
                    progressBar_universal.Value += 10;//2
                    richTextBoxTab1.Clear();
                    richTextBoxTab1.AppendText("Logged in on ChirpStack!\r\n"); Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, "Success: Logged in on ChirpStack!");
                    richTextBoxTab1.AppendText("\r\nserverURL: "+ serverURL);
                    if (int.Equals(await Chirp.ChirpGetApplicationName(textBox_ApplName.Text), 61))
                    {
                        result01 = 61;
                        progressBar_universal.Value += 10;//3
                        richTextBoxTab1.AppendText("The Application Name "+ textBox_ApplName.Text + " Already exists, Using Existing!\r\n");
                        richTextBoxTab1.AppendText("the Application ID is:: " + Chirp.ChirpPostApplID + "\r\n");

                        LG.WriteToFile(true, "The Application Name is: " + textBox_ApplName.Text);
                        LG.WriteToFile(true, "the Application ID is: " + Chirp.ChirpPostApplID);
                    }
                    else
                    {
                        result = await Chirp.ChirpPostApplication(textBox_ApplName.Text, textBox_ApplDesp.Text);
                        richTextBoxTab1.AppendText("The Application Name created.\r\n");
                        richTextBoxTab1.AppendText("the Application ID is:: " + Chirp.ChirpPostApplID + "\r\n");

                        LG.WriteToFile(true, "The Application Name is: " + textBox_ApplName.Text);
                        LG.WriteToFile(true, "the Application ID is: " + Chirp.ChirpPostApplID);
                    }
                    if (int.Equals(result, 41) || int.Equals(result01, 61))
                    {
                        result = 0; result01 = 0;
                        if (string.IsNullOrEmpty(textBox_SerialNumber_minVal.Text) || string.IsNullOrEmpty(textBox_SerialNumber_maxVal.Text))
                        {
                            textBox_SerialNumber_minVal.Text = "0000000001";
                            textBox_SerialNumber_maxVal.Text = "999999999";
                        }

                        double Serial_Min_Double = 0, Serial_Max_Double = 0;
                        Serial_Min_Double = int.Parse(textBox_SerialNumber_minVal.Text);
                        Serial_Max_Double = int.Parse(textBox_SerialNumber_maxVal.Text);

                        for (int counterForDevices = 0; counterForDevices < DevEUI_Main.Count; counterForDevices++)
                        {
                            double Serial_Double;
                            try
                            {
                                Serial_Double = double.Parse(SerialNumber_Main[counterForDevices]); counterForValidSerial++;
                            }
                            catch { Serial_Double = 000000000; }
                            try
                            {
                                if (((Serial_Double >= Serial_Min_Double) && (Serial_Double <= Serial_Max_Double)) || (checkBox_MeterRange.Checked == false))
                                {
                                    if (Serial_Double != 0)
                                    {
                                        //result returns 11 for success.
                                        result = await Chirp.ChirpPostNewDevice(DevEUI_Main[counterForDevices],
                                            AppKey_Main[counterForDevices], CustVer_Main[counterForDevices], FwVer_Main[counterForDevices],
                                            SerialNumber_Main[counterForDevices]);
                                    }
                                    if (result == 11)
                                        counterForSuccessPOST++;

                                }
                                if (result != 11 || ((Serial_Double > Serial_Max_Double) || (Serial_Double < Serial_Min_Double)))
                                {
                                    result = 0; counterForUNSuccessPOST++;
                                    richTextBoxTab1.AppendText("The DevEUI: " + DevEUI_Main[counterForDevices] + " is not uploaded.(" + SerialNumber_Main[counterForDevices] + ")\r\n");
                                }
                                progressBar_universal.Value += 1; result = 0;
                            }
                            catch { richTextBoxTab1.AppendText("Some parsing Error. " + DevEUI_Main[counterForDevices] + "\r\n"); 
                                LG.WriteToFile(true, "Error: Some parsing Error."); }
                        }
                    }
                    richTextBoxTab1.AppendText("meters Success: " + counterForSuccessPOST + "\r\n");
                    richTextBoxTab1.AppendText("meters UNSuccess: " + counterForUNSuccessPOST + "\r\n");
                    richTextBoxTab1.AppendText("Total Meters with valid Serial Numbers: " + counterForValidSerial + "\r\n");
                    richTextBoxTab1.AppendText("Total Meters: " + SerialNumber_Main.Count + "\r\n");

                    LG.WriteToFile(true, "Success: MS:"+ counterForSuccessPOST+", MUS:"+ counterForUNSuccessPOST+", ValS#:"+ counterForValidSerial+", Tot:" + SerialNumber_Main.Count);

                    progressBar_universal.Value = progressBar_universal.Maximum; Thread.Sleep(5000); checkBox_ViewList.Checked = false; checkBox_ViewList.ForeColor = Color.Black;
                    progressBar_universal.Visible = false;

                    counterForSuccessPOST = 0; counterForUNSuccessPOST = 0; counterForValidSerial = 0; label_authenticator.Visible = false;
                }
           }
        }
        private void button_CF_Click(object sender, EventArgs e)
        {
            ConfigEditor CF = new ConfigEditor(ConfigFilepath);

            if(!AuthenticationPopup())//flag_NoAuthentication
            {
                flag_NoAuthentication = true;
                DialogResult dialogR = CF.ShowDialog();
                label_authenticator.Visible = true;
                if (dialogR == DialogResult.Cancel) { }
                if (dialogR == DialogResult.OK)
                {
                    if (!AuthenticationPopup())//flag_NoAuthentication
                    {
                        CF.FileWrite(); flag_NoAuthentication = true;
                         Loghandler LG = new Loghandler(CompleteLogAddress);
                        LG.WriteToFile(true, "Success: Config File Modified. Credentials Correct");
                        label_authenticator.Text = "Application Restart suggested. "; label_authenticator.ForeColor = Color.Red; button1.Visible = false; button2.Visible = false; Close();
                    }

                }
            }

        }
        private void button_AMR_Browse_Click(object sender, EventArgs e)
        {
            listView_AMR.Visible = false;
            string filePath = string.Empty; var fileContent = string.Empty;
            string Endsight_filePath = this.endsightRootFolder; var Endsight_fileContent = string.Empty;
            DataInOut DIN = new DataInOut();

            VarClearFuntion();

            Endsight_filePath = LatestFileSort(Endsight_filePath);
            richTextBox_AMR.Text = "Endsight File: " + Endsight_File1Name;

            richTextBox_AMR.AppendText("\r\nbrowse this Directory for Production files: " + DataMergerFileStorage + @"\Output-->Date." + "\r\nyou can Copy the path.");
            if (Endsight_filePath != null)
            {
                
                DIN.EndsightCSVFileExtractor(Endsight_filePath); 

                this.EndsightStatus_M = DIN.EndsightStatus;
                this.EndsightSerialNumber_M = DIN.EndsightSerialNumber;
                this.EndsightDevUI_M = DIN.EndsightDevUI;
            }

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = DataMergerFileStorage;
                openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog1.FileName;
                    richTextBox_AMR.AppendText("\r\n" + openFileDialog1.FileName);
                    textBox_ProductionFile.Text = filePath.Substring(filePath.LastIndexOf("\\") + 1, (filePath.Length - filePath.LastIndexOf("\\")) - 1);
                    if (filePath != string.Empty)
                    {
                        DIN.MergerCSVFileExtractor(filePath);this.DevEUI_Main = DIN.DevEUI;
                        this.AppKey_Main = DIN.AppKey; this.FwVer_Main = DIN.FwVer;
                        this.SerialNumber_Main = DIN.SerialNumber;
                    }
                }
            }
            if (filePath != string.Empty)
                richTextBox_AMR.AppendText("\r\nProduction File is selected!");
            else
                richTextBox_AMR.AppendText("\r\nNothing is Selected, Try Browsing again!\r\nThe Help is Here.");
        }
        #endregion Button Clicks

        #region Functions
        public void DateSpecificFileSelect()
        {
            filenameArray.Clear();
            string Folder = FileAddress;   //textBox4_MergeFileinput.Text;
            var files = new DirectoryInfo(Folder).GetFiles("*.csv");

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(textBox_dateShow.Text))//file.LastWriteTime > lastupdated)
                {
                    if (!filenameArray.Contains(file.Name))
                    {
                        filenameArray.Add(Folder +"\\"+ file.Name);
                    }
                }
            }
        }
        public void DuplicateFinder()                                   //this function helps find the duplicates in the file 
        {
            try
            {
                for (int reference = 0; reference < DevEUI_Main.Count - 1; reference++)//(int reference = DevUI.Count - 1; reference > 0; reference--)//
                {
                    int numOfDuplicates = 1;    //placing here makes it sure to reset for every element in the list to generate the number of duplicates Prog has deleted for user
                    for (int comparingTo = reference + 1; comparingTo < DevEUI_Main.Count; comparingTo++)  //(int comparingTo = reference + 1; comparingTo < DevUI.Count;comparingTo--)
                    {

                        if (DevEUI_Main[reference] == DevEUI_Main[comparingTo])
                        {
                            if (comparingTo == DevEUI_Main.Count - 1) //this function helps to eliminate the last duplicate element without overflow
                            {
                                try
                                {

                                    DevEUI_Main[reference] = DevEUI_Main[comparingTo];
                                    AppKey_Main[reference] = AppKey_Main[comparingTo];
                                    CustVer_Main[reference] = CustVer_Main[comparingTo];
                                    FwVer_Main[reference] = FwVer_Main[comparingTo];

                                    Duplicates[reference] = numOfDuplicates + " Dupl. Deleted";
                                    DevEUI_Main.RemoveAt(comparingTo);
                                    AppKey_Main.RemoveAt(comparingTo);
                                    CustVer_Main.RemoveAt(comparingTo);
                                    FwVer_Main.RemoveAt(comparingTo);
                                    try { SerialNumber_Main.RemoveAt(comparingTo); } catch { }
                                    ++numOfDuplicates;

                                    break;
                                }
                                catch { }
                            }
                            while (DevEUI_Main[reference] == DevEUI_Main[comparingTo])
                            {
                                try
                                {
                                    tempAryforDuplicates[0] = DevEUI_Main[comparingTo];
                                    tempAryforDuplicates[1] = AppKey_Main[comparingTo];
                                    tempAryforDuplicates[2] = CustVer_Main[comparingTo];
                                    tempAryforDuplicates[3] = FwVer_Main[comparingTo];

                                    Duplicates[reference] = numOfDuplicates + " Dupl. Deleted";
                                    DevEUI_Main.RemoveAt(comparingTo);
                                    AppKey_Main.RemoveAt(comparingTo);
                                    CustVer_Main.RemoveAt(comparingTo);
                                    FwVer_Main.RemoveAt(comparingTo);
                                    try { SerialNumber_Main.RemoveAt(comparingTo); } catch { }
                                    ++numOfDuplicates;
                                }
                                catch { }
                            }
                            DevEUI_Main[reference] = tempAryforDuplicates[0];
                            AppKey_Main[reference] = tempAryforDuplicates[1];
                            CustVer_Main[reference] = tempAryforDuplicates[2];
                            FwVer_Main[reference] = tempAryforDuplicates[3];
                        }
                    }
                }
                for(int counter=0;counter<DevEUI_Main.Count;counter++)
                {
                    string TempLine = DevEUI_Main[counter] + "," + AppKey_Main[counter] + "," + CustVer_Main[counter] + "," + FwVer_Main[counter] + "," + SerialNumber_Main[counter];
                    if (counter == 0)
                    {
                        //FormatStringType is used to give the column names
                        File.WriteAllText(DataMergerFileStorage + "AutoMergerFor_" + textBox_dateShow.Text + FileFormat, FormatStringType + "\r\n");
                        File.AppendAllText(DataMergerFileStorage + "AutoMergerFor_" + textBox_dateShow.Text + FileFormat, TempLine + "\r\n");
                    }

                        
                    else
                        File.AppendAllText(DataMergerFileStorage + "AutoMergerFor_" + textBox_dateShow.Text + FileFormat, TempLine + "\r\n");

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Duplicate Finded,\r\n(Index detected)\r\nDone\r\n");
                //MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void FIleMerger()
        {
            int counterTopTemp = 0;
            foreach (string file in filenameArray)
            {
                string[] TempStorage1 = File.ReadAllLines(file);
                int counterTemp = 0;
                foreach(string tempStringLine in TempStorage1)
                {
                    if (counterTemp == 0 && counterTopTemp > 0) { }

                    else if (counterTopTemp == 0 && counterTemp == 0)
                    {
                        FormatStringType = tempStringLine;
                        File.AppendAllText(DataMergerFileStorage + "AutoMergerFor_" + textBox_dateShow.Text + FileFormat, tempStringLine + "\r\n"); 
                    }
                        
                    else
                    {
                        if(!tempStringLine.StartsWith(","))
                            File.AppendAllText(DataMergerFileStorage + "AutoMergerFor_" + textBox_dateShow.Text + FileFormat, tempStringLine + "\r\n");
                    }
                       
                    counterTemp++;
                }
                counterTopTemp++;
            }
            #region commented
            /*
             *   for(int counter = 0;counter<=TempStorage1.Length;)
                {
                    if(TempStorage1[counter].Contains("DevEUI"))
                    {
                        Array.Clear(TempStorage1, counter, 1);
                        counter = 0;
                    }
                    else if (TempStorage1[counter]==null)
                    {
                        counter++;
                    }
                    else
                    {
                        counter++;
                    }
                }
             */
            #endregion commented
        }
        public void VarClearFuntion()
        {
            DevEUI_Main.Clear();
            AppKey_Main.Clear();
            CustVer_Main.Clear();
            FwVer_Main.Clear();
            Time_Main.Clear();
            Position_Main.Clear();
            SerialNumber_Main.Clear();
            SrNum_Main.Clear();
            SrNum_Long.Clear();
            EndsightStatus_M.Clear();
            EndsightSerialNumber_M.Clear();
            EndsightDevUI_M.Clear();
          
            Array.Clear(Duplicates,0,Duplicates.Length);

            //listView1.Clear();
        }
        public void Get_DevEUI()
        {
            SQLQueries SQL = new SQLQueries(DBServername+";", DBName, DBUser, DBPass);
            foreach (string Dev in DevEUI_Main)
            {
                SerialNumber_Main.Add(SQL.GrabADatabaseWithDevEUI(Dev, "MeterID"));
            }
        }
        private void button_AMR_Start_Click(object sender, EventArgs e)
        {
            //authentication
            flag_NoAuthentication = true;
            Authenticator AU = new Authenticator(this.CSPassword);
            DialogResult dialogR = AU.ShowDialog();
            label_authenticator.Visible = true;
            if (dialogR == DialogResult.Cancel)
            {
                if (AU.Flag_AuthenticationSucccess)
                {
                    label_authenticator.Text = "The Credentials are Correct."; label_authenticator.ForeColor = Color.Green;
                    flag_NoAuthentication = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, label_authenticator.Text);
                }
                else if (AU.Flag_AuthenticationSucccess && string.IsNullOrEmpty(textBox_ApplName.Text))
                {
                    label_authenticator.Text = "The Credentials are Correct, Empty Slots."; label_authenticator.ForeColor = Color.Red;
                    progressBar_universal.Visible = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, "Error: " + label_authenticator.Text);
                }
                else if (!AU.Flag_AuthenticationSucccess)
                {
                    label_authenticator.Text = "The Credentials are incorrect."; flag_NoAuthentication = true; label_authenticator.ForeColor = Color.Red;
                    progressBar_universal.Visible = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, "Error: " + label_authenticator.Text);
                }
                AU.Dispose();
            }
            //authentication ends
            if (!flag_NoAuthentication)
            {
                progressBar_AMR.Visible = true; progressBar_AMR.Maximum = (10 * DevEUI_Main.Count)+100; progressBar_AMR.Minimum = 0; progressBar_AMR.Value = progressBar_AMR.Minimum; label9.Visible = true;

                progressBar_AMR.Value += 10;

                if (!string.IsNullOrEmpty(textBox_Initials.Text) && textBox_Initials.Text.Length > 1)
                {
                    progressBar_AMR.Value += 10;//progress bar
                    richTextBox_AMR.Text = "Compare and Export Button pressed";
                    string date = DateTime.Now.ToString(); ;//2 for systemtime
                                                            //Directory.CreateDirectory(FileOutputForErrorSheets_tab2 + date);

                    string ExportFileName = "ErrorSheet_For_" + textBox_ProductionFile.Text;//date + "_" + FileCounter + ".csv";
                    string path = FileOutputForErrorSheets_tab2 + @"\" + ExportFileName;
                    File.Delete(path);

                    EndsightComparefunction(path); //comparing the data here.
                    progressBar_AMR.Value += 10;//progress bar
                    listView_AMR.Clear();
                    listView_AMR.Columns.Add("DBn", 50, HorizontalAlignment.Left);
                    listView_AMR.Columns.Add("DevEUI", 150, HorizontalAlignment.Left);
                    listView_AMR.Columns.Add("Prod.SerialNo", 150, HorizontalAlignment.Left);
                    listView_AMR.Columns.Add("Ends.SerialNo", 150, HorizontalAlignment.Left);
                    listView_AMR.Columns.Add("Status Code", 100, HorizontalAlignment.Left);
                    listView_AMR.Visible = true;
                    try     //try for file directory search
                    {
                        if (!File.Exists(path) || File.Exists(path))
                        {
                            progressBar_AMR.Value += 10;//progress bar
                            displayConfigArray[0] = "Prod_DevEUI" + "," + "Prod_SerialNumber" + "," + "End_SerialNum" + "," + "StatusCode";
                            File.AppendAllLines(path, displayConfigArray);
                            for (int count = 1; count < DevEUI_Main.Count; count++)
                            {
                                try { TempArrayForDisplayContent[0] = "" + count; } catch { }
                                try { TempArrayForDisplayContent[1] = DevEUI_Main[count]; } catch { }
                                try { TempArrayForDisplayContent[2] = SerialNumber_Main[count]; } catch { }
                                try { TempArrayForDisplayContent[3] = tempEndMeterID[count]; } catch { TempArrayForDisplayContent[3] = "--"; }
                                try { TempArrayForDisplayContent[4] = statusCode[count]; } catch { TempArrayForDisplayContent[4] = "--"; }
                                if (TempArrayForDisplayContent[3] == null) { TempArrayForDisplayContent[3] = "ND_Endst"; }
                                if (TempArrayForDisplayContent[4] == null) { TempArrayForDisplayContent[4] = "Not OK_Mnul"; }

                                displayConfigArray[0] = TempArrayForDisplayContent[1] + "," + TempArrayForDisplayContent[2] + "," + TempArrayForDisplayContent[3] + "," + TempArrayForDisplayContent[4];
                                File.AppendAllLines(path, displayConfigArray);
                                //displayConfigArray[0] = TempArrayForDisplayContent[1] + "\t" + TempArrayForDisplayContent[2] + "\t" + TempArrayForDisplayContent[3] + "\t" + TempArrayForDisplayContent[4];

                                itm = new ListViewItem(TempArrayForDisplayContent);
                                listView_AMR.Items.Add(itm);
                                checkBox_AMR_ListView.Checked = true;
                            }
                            File.AppendAllText(path, "All Short terms:: ND-No Data, Endst-Endsight, Mnul-Manual Entry, DMIU- Default MeterID unmatched for matched DevEUI with Production records, DSC-Default Statuscode OK");
                        }
                        progressBar_AMR.Value += 10;//progress bar
                        richTextBox_AMR.Text = "the file is exported to\r\n" + path + "\r\n" + CounterForCorrectDataSet + "-------Datasets are Correct And can be used to AMR check.";
                        //richTextBox_AMR.AppendText("\r\n" + globalCounterForEndsight.Count + "-------globalCounterForEndsight.Debug use!");
                        richTextBox_AMR.AppendText("\r\n" + CounterForUpdatedMetersToDB + " Meters Updated in AMR Check, Others might be already checked!");//Amr checkedon these meters
                        try { richTextBox_AMR.AppendText("\r\n" + FwVer_Main[2] + " -------ModemFirmwareRev updated in DataBase."); }
                        catch { }
                        MessageBox.Show(CounterForUpdatedMetersToDB + "-------meters are updated to DB.");

                    }
                    catch (Exception ex)    //catch for file directory search
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else { MessageBox.Show("Enter the Initials.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            }

            progressBar_AMR.Value =progressBar_AMR.Maximum;//progress bar
            Thread.Sleep(500); progressBar_AMR.Visible = false; label9.Visible = false;
        }
        private void button_AMR_Start_Enter(object sender, EventArgs e)
        {
            textBox_Initials.Text = textBox_Initials.Text.ToUpper();
        }
        public void DisplayListView()
        {
            listView1.Visible = true;
            try
            {
                listView1.Clear();
                listView1.Columns.Add("Count", 50, HorizontalAlignment.Center);
                listView1.Columns.Add("DevEUI", 150, HorizontalAlignment.Center);
                listView1.Columns.Add("AppKey", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("CustVer", 50, HorizontalAlignment.Center);
                listView1.Columns.Add("FwVer", 50, HorizontalAlignment.Center);
                listView1.Columns.Add("SerialNumber", 100, HorizontalAlignment.Center);
                //listView.Columns.Add("DuChck", 80, HorizontalAlignment.Center);
                //listView.Columns.Add("BatchID", 80, HorizontalAlignment.Center);
                File.Delete(DataMergerFileStorage + "AutoMergerFor_" + textBox_dateShow.Text + FileFormat);
                File.WriteAllText(DataMergerFileStorage + "AutoMergerFor_" + textBox_dateShow.Text + FileFormat, "DevEUI,AppKey,CustVer,FwVer,SerialNumber"+"\r\n");
                for (int count = 0; count < DevEUI_Main.Count; count++)
                {
                    TempArrayForDisplayContent[0] = count + 1 + ".";   //the columns are designed above
                    TempArrayForDisplayContent[1] = DevEUI_Main[count];
                    TempArrayForDisplayContent[2] = AppKey_Main[count];
                    TempArrayForDisplayContent[3] = CustVer_Main[count];
                    TempArrayForDisplayContent[4] = FwVer_Main[count];
                    TempArrayForDisplayContent[5] = SerialNumber_Main[count];
                    TempArrayForDisplayContent[6] = Duplicates[count];

                    itm = new ListViewItem(TempArrayForDisplayContent);
                    listView1.Items.Add(itm);
                    File.AppendAllText(DataMergerFileStorage + "AutoMergerFor_" + textBox_dateShow.Text + FileFormat,
                        TempArrayForDisplayContent[1]+","+ TempArrayForDisplayContent[2] + ","+ TempArrayForDisplayContent[3] + ","+ TempArrayForDisplayContent[4] + ","+ TempArrayForDisplayContent[5] + ","+ TempArrayForDisplayContent[6] + "\r\n");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public string LatestFileSort(string FileInputDir)
        {
            DateTime lastupdated = DateTime.MinValue;
            string Folder = FileInputDir;

            var files = new DirectoryInfo(Folder).GetFiles("*.csv");

            foreach (FileInfo file in files)
            {
                if (file.LastWriteTime > lastupdated)
                {
                    lastupdated = file.LastWriteTime;
                    Endsight_File1FullPath = file.FullName;
                    Endsight_File1Name = file.Name;
                }
            }
            return Endsight_File1FullPath;
        }
        public void GetdataFromConfig()
        {
            XMLfunction XML = new XMLfunction();

            this. VersionFromConfig = XML.ConfigFileExtractor("versionUpgrade", ConfigFilepath);
            this.FileAddress = XML.ConfigFileExtractor("RootFolder", ConfigFilepath);
            this.serverURL = XML.ConfigFileExtractor("ServerURL", ConfigFilepath);
            this.serviceProfileID = XML.ConfigFileExtractor("ServiceProfileID", ConfigFilepath);
            this.deviceProfileID = XML.ConfigFileExtractor("DeviceProfileID", ConfigFilepath);
            this.CSPassword = XML.ConfigFileExtractor("password", ConfigFilepath);
            this.LogFileAddress = XML.ConfigFileExtractor("LogFileDestination", ConfigFilepath);
            this.DBServername = XML.ConfigFileExtractor("DBServerName", ConfigFilepath);
            this.DBName = XML.ConfigFileExtractor("DBName", ConfigFilepath);
            this.DBUser = XML.ConfigFileExtractor("DBUser", ConfigFilepath);
            this.DBPass = XML.ConfigFileExtractor("DBPass", ConfigFilepath);
            this.endsightRootFolder = XML.ConfigFileExtractor("endsight", ConfigFilepath);
            if (CSPassword.Length > 5)
            {
                MessageBox.Show("Error: This is Blocking Error.\r\nNothing will work until you change the password to less than 5 characters.\r\nYou can do that by contacting admin at\r\nvishal@visionmetering.com");Close();
            }
            if(!string.Equals(version,VersionFromConfig))
            {
                MessageBox.Show("Warning:\r\nNew version is Available,\r\nUpgrade for better performance!");
                label_upgradeAvl.Visible = true; label_upgradeAvl.Text = "New version is Available";
            }
        }
        public void WriteDataToConfigOnce()
        {
            try
            {
                File.WriteAllText(ConfigFilepath, @"<RootFolder>E:\Output</RootFolder>"+"\r\n");
                File.AppendAllText(ConfigFilepath, @"<user>admin</user>" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"{not used anywhere Yet}" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"<password>7890</password>" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"{NO more than 4 characters as password}" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"<ServerURL>http://117.100.100.20:8080/api/</ServerURL>" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"<ServiceProfileID>f9f80426-52c6-4dfe-9b58-1c376d6e0720</ServiceProfileID>" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"{important for Chirpstack, Dont edit these fields}" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"<DeviceProfileID>a8fdb64e-7493-49c3-8ee9-f7ed20ef267c</DeviceProfileID>" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"{important for Chirpstack, Dont edit these fields}" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"<LogFileDestination>\\netserver3\DATA\Loraproduction_Engineering\LGW\Log</LogFileDestination>" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"{Important DataBase Info. Below >>>>>>>>>>>>>>}" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"<DBServerName>Netserver3</DBServerName>" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"<DBName>LoraGasVision</DBName>" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"<DBUser>power</DBUser>" + "\r\n");
                File.AppendAllText(ConfigFilepath, @"<DBPass>power</DBPass>" + "\r\n");

            }
            catch { }

        }
        public void EndsightComparefunction(string path)  //tab 2 function
        {
            SQLQueries SQL = new SQLQueries(DBServername + ";", DBName, DBUser, DBPass);
            //ColumnNominclature(path);//columns would be named here instead of after this function.

            for (int counter = 0; counter < EndsightDevUI_M.Count; counter++)
            {
                string EndsightDevEuiTemp = EndsightDevUI_M[counter].ToLowerInvariant();
                if (DevEUI_Main.Contains(EndsightDevEuiTemp)) { }
            }
            progressBar_AMR.Value += 10;//progress bar
            for (int reference = 0; reference < DevEUI_Main.Count; reference++)
            {
                for (int comparingTo = 0; comparingTo < EndsightDevUI_M.Count; comparingTo++)
                {
                    while (DevEUI_Main[reference] == EndsightDevUI_M[comparingTo].ToLowerInvariant())//devEui matching?
                    {
                        if (SerialNumber_Main[reference] == EndsightSerialNumber_M[comparingTo])//meterID matching?
                        {
                            if (EndsightStatus_M[comparingTo].ToUpper().Contains("OK"))//status code matching?
                            {
                                Tab2_FileAppend(reference, comparingTo, path);//why it is here? to append the correct data flaged OK into File to AMR check.
                                DateTime dateTime = DateTime.Now;//local time taken from the computer

                                if (string.IsNullOrEmpty(SQL.GrabADatabaseWithMeterID(SerialNumber_Main[reference], "AMRchkBy")) || checkBox_AMR_updateAll.Checked)      //check if the return is true or false  CountForreturnDbData <= 1
                                {
                                    //MeterId, date, initialofUser, AppKey, DatabaseChange, FwVersion, TxPower, FreqChannels, AppEUI
                                    SQL.PostDataToAMRCheck(SerialNumber_Main[reference], dateTime, textBox_Initials.Text.ToUpper(), AppKey_Main[reference], FwVer_Main[reference]);
                                    CounterForUpdatedMetersToDB++;
                                    progressBar_AMR.Value += 10;//progress bar
                                }
                                DevEUI_Main.RemoveAt(reference); SerialNumber_Main.RemoveAt(reference); AppKey_Main.RemoveAt(reference); FwVer_Main.RemoveAt(reference);
                                if (reference != 0) { reference--; }
                                CounterForCorrectDataSet++;
                            }
                            else { statusCode[reference] = EndsightStatus_M[comparingTo] + ":" + "DSC"; }//commented break
                        }
                        else { tempEndMeterID[reference] = EndsightSerialNumber_M[comparingTo] + ":" + "DMIU"; }//commented break
                        break;
                    }
                }
            }
        }
        public void Tab2_FileAppend(int reference, int compareTo, string path)
        {
            string[] displayConfigArray = new string[1];
            TempArrayForDisplayContent[1] = DevEUI_Main[reference];
            TempArrayForDisplayContent[2] = SerialNumber_Main[reference];
            TempArrayForDisplayContent[3] = EndsightSerialNumber_M[compareTo];
            TempArrayForDisplayContent[4] = EndsightStatus_M[compareTo] + "_Endst";
            TempArrayForDisplayContent[5] = FwVer_Main[reference];
            if (TempArrayForDisplayContent[3] == null) { TempArrayForDisplayContent[3] = "ND_Endst"; }//EndsightMeterID_ Nd is No data
            if (TempArrayForDisplayContent[4] == null) { TempArrayForDisplayContent[4] = "OK_Mnul"; }//Status Code Mnul is Manual entry

            displayConfigArray[0] = TempArrayForDisplayContent[1] + "," + TempArrayForDisplayContent[2] + "," + TempArrayForDisplayContent[3] + "," + TempArrayForDisplayContent[4] + "," + TempArrayForDisplayContent[5];
            File.AppendAllLines(path, displayConfigArray);
        }
        public bool AuthenticationPopup()
        {
            //authentication
            flag_NoAuthentication = true;
            Authenticator AU = new Authenticator(this.CSPassword);
            DialogResult dialogR = AU.ShowDialog();
            label_authenticator.Visible = true;
            if (dialogR == DialogResult.Cancel)
            {
                if (AU.Flag_AuthenticationSucccess)
                {
                    label_authenticator.Text = "The Credentials are Correct."; label_authenticator.ForeColor = Color.Green;
                    flag_NoAuthentication = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, label_authenticator.Text);
                }
                else if (AU.Flag_AuthenticationSucccess && string.IsNullOrEmpty(textBox_ApplName.Text))
                {
                    label_authenticator.Text = "The Credentials are Correct, Empty Slots."; label_authenticator.ForeColor = Color.Red;
                    progressBar_universal.Visible = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, "Error: " + label_authenticator.Text);
                }
                else if (!AU.Flag_AuthenticationSucccess)
                {
                    label_authenticator.Text = "The Credentials are incorrect."; flag_NoAuthentication = true; label_authenticator.ForeColor = Color.Red;
                    progressBar_universal.Visible = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, "Error: " + label_authenticator.Text);
                }
                AU.Dispose();
            }
            return flag_NoAuthentication;
            //authentication ends
        }

        #endregion Functions

        #region .NET functions
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox_dateShow.Text = monthCalendar1.SelectionRange.Start.ToString("yyyy_MM_dd");
            monthCalendar1.Visible = false;
            label_Path.Visible = true; label_HeadingToPath.Visible = true;
            label_Path.Text = FileAddress;
        }
        private void textBox_dateShow_MouseEnter(object sender, EventArgs e)
        {
            monthCalendar1.Visible = true;
            label_Path.Visible = false; label_HeadingToPath.Visible = false;
        }
        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox_ViewList.Checked && flag_ListviewOption)
            {
                listView1.Visible = true; checkBox_ViewList.ForeColor = Color.Green;
            }
            else if(!checkBox_ViewList.Checked && flag_ListviewOption)
            {
                listView1.Visible = false; checkBox_ViewList.ForeColor = Color.Black;
            }
            else if(!checkBox_ViewList.Checked && flag_ListviewOption)
            {
                listView1.Visible = true; checkBox_ViewList.ForeColor = Color.Black;
            }
        }
        private void textBox_ApplName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            System.Windows.Forms.TextBox textbox = sender as System.Windows.Forms.TextBox;

            if (textbox == null)
                return;

            if (!char.IsControl(ch) && (!char.IsNumber(ch)) && !char.IsLetterOrDigit(ch))
                e.Handled = true;
        }
        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox_MeterRange.Checked)
            {
                textBox_SerialNumber_minVal.Visible = true; textBox_SerialNumber_maxVal.Visible = true; label3.Visible = true;
            }
            else
            {
                textBox_SerialNumber_minVal.Visible = false; textBox_SerialNumber_maxVal.Visible = false; label3.Visible = false;
            }
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        private void checkBox1_CheckStateChanged_1(object sender, EventArgs e)
        {
            listView_AMR.Visible = checkBox_AMR_ListView.Checked ? true : false;
        }
        private void button_CF_MouseEnter(object sender, EventArgs e)
        {
            label7.Visible = true;
        }
        private void button_CF_MouseLeave(object sender, EventArgs e)
        {
            label7.Visible = false;
        }
        private void button2_MouseEnter(object sender, EventArgs e)
        {
            label8.Visible = true;
        }
        private void button2_MouseLeave(object sender, EventArgs e)
        {
            label8.Visible = false;
        }
        private void textBox_Initials_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            System.Windows.Forms.TextBox textbox = sender as System.Windows.Forms.TextBox;

            if (textbox == null)
                return;

            if (!char.IsControl(ch) && !char.IsLetter(ch))
                e.Handled = true;
        }

        #endregion .NET functions

        #region Commented AuthenticationCode
        ////authentication
        //flag_NoAuthentication = true;
        //Authenticator AU = new Authenticator(this.CSPassword);
        //DialogResult dialogR = AU.ShowDialog();
        //label_authenticator.Visible = true;
        //if (dialogR == DialogResult.Cancel)
        //{
        //    if (AU.Flag_AuthenticationSucccess)
        //    {
        //        label_authenticator.Text = "The Credentials are Correct."; label_authenticator.ForeColor = Color.Green;
        //        flag_NoAuthentication = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, label_authenticator.Text);
        //    }
        //    else if (AU.Flag_AuthenticationSucccess && string.IsNullOrEmpty(textBox_ApplName.Text))
        //    {
        //        label_authenticator.Text = "The Credentials are Correct, Empty Slots."; label_authenticator.ForeColor = Color.Red;
        //        progressBar_universal.Visible = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, "Error: "+ label_authenticator.Text);
        //    }
        //    else if (!AU.Flag_AuthenticationSucccess)
        //    {
        //        label_authenticator.Text = "The Credentials are incorrect."; flag_NoAuthentication = true; label_authenticator.ForeColor = Color.Red;
        //        progressBar_universal.Visible = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, "Error: "+ label_authenticator.Text);
        //    }
        //    AU.Dispose();
        //}

        ////authentication ends
        /// ////authentication
        //flag_NoAuthentication = true;
        //Authenticator AU1 = new Authenticator(this.CSPassword);
        //DialogResult dialogR11 = AU1.ShowDialog();
        //label_authenticator.Visible = true;
        //if (dialogR11 == DialogResult.Cancel)
        //{
        //    if (AU1.Flag_AuthenticationSucccess)
        //    {
        //        label_authenticator.Text = "The Credentials are Correct."; label_authenticator.ForeColor = Color.Green;
        //        flag_NoAuthentication = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, label_authenticator.Text);
        //    }
        //    else if (AU1.Flag_AuthenticationSucccess && string.IsNullOrEmpty(textBox_ApplName.Text))
        //    {
        //        label_authenticator.Text = "The Credentials are Correct, Empty Slots."; label_authenticator.ForeColor = Color.Red;
        //        progressBar_universal.Visible = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, "Error: " + label_authenticator.Text);
        //    }
        //    else if (!AU1.Flag_AuthenticationSucccess)
        //    {
        //        label_authenticator.Text = "The Credentials are incorrect."; flag_NoAuthentication = true; label_authenticator.ForeColor = Color.Red;
        //        progressBar_universal.Visible = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, "Error: " + label_authenticator.Text);
        //        LG.WriteToFile(true, "Warning: Tried Changing Config File. Credentials either incorrect or cancelled.");
        //    }
        //    AU1.Dispose();
        //}
        ////authentication ends
        ///////authentication
        //flag_NoAuthentication = true;
        //Authenticator AU = new Authenticator(this.CSPassword);
        //DialogResult dialogR1 = AU.ShowDialog();
        //label_authenticator.Visible = true;
        //if (dialogR1 == DialogResult.Cancel)
        //{
        //    if (AU.Flag_AuthenticationSucccess)
        //    {
        //        label_authenticator.Text = "The Credentials are Correct."; label_authenticator.ForeColor = Color.Green;
        //        flag_NoAuthentication = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, label_authenticator.Text);
        //    }
        //    else if (AU.Flag_AuthenticationSucccess && string.IsNullOrEmpty(textBox_ApplName.Text))
        //    {
        //        label_authenticator.Text = "The Credentials are Correct, Empty Slots."; label_authenticator.ForeColor = Color.Red;
        //        progressBar_universal.Visible = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, "Error: " + label_authenticator.Text);
        //    }
        //    else if (!AU.Flag_AuthenticationSucccess)
        //    {
        //        label_authenticator.Text = "The Credentials are incorrect."; flag_NoAuthentication = true; label_authenticator.ForeColor = Color.Red;
        //        progressBar_universal.Visible = false; Loghandler LG = new Loghandler(CompleteLogAddress); LG.WriteToFile(true, "Error: " + label_authenticator.Text);
        //        LG.WriteToFile(true, "Warning: Tried Changing Config File. Credentials either incorrect or cancelled.");
        //    }
        //    AU.Dispose();
        //}
        ////authentication ends
        #endregion Commented
    }
}
