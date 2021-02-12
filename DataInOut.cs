using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace WaterGasTool
{
    class DataInOut
    {
        #region Declarations

        public List<string> DevEUI      = new List<string>();
        public List<string> AppKey      = new List<string>();
        public List<string> CustVer     = new List<string>();
        public List<string> FwVer       = new List<string>();
        public List<string> Time        = new List<string>();
        public List<string> Position    = new List<string>();

        public List<string> EndsightStatus = new List<string>();
        public List<string> EndsightDevUI = new List<string>();
        public List<string> EndsightSerialNumber = new List<string>();
        public List<string> SerialNumber = new List<string>();



        #endregion Declarations
        public void CSVFileExtractor(string filename)
        {
            try
            {
                using (TextFieldParser parser = new TextFieldParser(filename))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        #region Parsing the engg file
                        string[] fields = parser.ReadFields();

                        try { this.DevEUI.Add(fields[0]); } catch { }
                        try { this.AppKey.Add(fields[1]); } catch { }
                        try { this.CustVer.Add(fields[2]); } catch { }
                        try { this.FwVer.Add(fields[3]); } catch { }
                        try { this.Time.Add(fields[4]); } catch { }
                        try { this.Position.Add(fields[5]); } catch { }

                        #endregion Parsing the engg file
                    }
                    try { this.DevEUI.RemoveAt(0); } catch { }
                    try { this.AppKey.RemoveAt(0); } catch { }
                    try { this.CustVer.RemoveAt(0); } catch { }
                    try { this.FwVer.RemoveAt(0); } catch { }
                    try { this.Time.RemoveAt(0); } catch { }
                    try { this.Position.RemoveAt(0); } catch { }
                    int DynamicCountForDevEUI = DevEUI.Count;

                    for (int tempCount = 0; tempCount < DynamicCountForDevEUI;)
                    {
                        if (string.IsNullOrEmpty(DevEUI[tempCount]))
                        {
                            try { this.DevEUI.RemoveAt(tempCount); } catch { }
                            try { this.AppKey.RemoveAt(tempCount); } catch { }
                            try { this.CustVer.RemoveAt(tempCount); } catch { }
                            try { this.FwVer.RemoveAt(tempCount); } catch { }
                            try { this.Time.RemoveAt(tempCount); } catch { }
                            try { this.Position.RemoveAt(tempCount); } catch { }
                            DynamicCountForDevEUI--;
                            tempCount--;
                        }
                        tempCount++;
                    }
                }
            }
            catch { MessageBox.Show("No File Found!: CSVFileExtractor"); }
        }

        public void EndsightCSVFileExtractor(string Endsight_filePath)
        {
            using (TextFieldParser parser = new TextFieldParser(Endsight_filePath))
            {
                try
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        #region Parsing the engg file
                        string[] fields = parser.ReadFields();
                        #endregion Parsing the engg file

                        EndsightDevUI.Add(fields[2]);
                        EndsightStatus.Add(fields[4]);
                    }
                    EndsightDevUI.RemoveAt(0);
                    EndsightStatus.RemoveAt(0);
                }
                catch 
                {
                    MessageBox.Show("No File Found!: EndsightFileParser");
                }
            }
        }

        public void AutoMergerCsvFileExtractor(string filePath)
        {
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                try
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        #region Parsing the engg file
                        string[] fields = parser.ReadFields();
                        #endregion Parsing the engg file

                        DevEUI.Add(fields[0]);
                        AppKey.Add(fields[1]);
                        FwVer.Add(fields[3]);
                        SerialNumber.Add(fields[4]);
                    }
                    DevEUI.RemoveAt(0);
                    AppKey.RemoveAt(0);
                    FwVer.RemoveAt(0);
                    SerialNumber.RemoveAt(0);
                }
                catch { }
            }
        }
    }
}
