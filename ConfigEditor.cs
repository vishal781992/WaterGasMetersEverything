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

namespace WaterGasTool
{
    public partial class ConfigEditor : Form
    {
        protected string FilePath = string.Empty;
        public ConfigEditor(string FileP)
        {
            InitializeComponent();
            this.FilePath = FileP;
            FileRead();
            label_Notice.Text = "The Application Closes Automatically as you Save the changes using \"Done\". ReLaunch It!>>>>>>>>>";
        }
        
        public void FileRead()
        {
            if(File.Exists(FilePath))
            {
                richTextBox1.Clear();
                string[] TempDataLine = File.ReadAllLines(FilePath);
                foreach (string str in TempDataLine)
                    richTextBox1.AppendText(str+"\r\n");
            }
        }
        public void FileWrite()
        {
            if (File.Exists(FilePath))
            {
                File.WriteAllText(FilePath,richTextBox1.Text);
                Close();
            }
        }

    }
}
