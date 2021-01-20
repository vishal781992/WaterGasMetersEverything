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
    class Loghandler
    {
        protected string Filename = string.Empty;
        public Loghandler(string FileNAME)
        {
            this.Filename = FileNAME;
        }
        public string TimeNow()
        {
            string AccurateTime = DateTime.Now.ToString("|MM.dd|HH.mm.ss|> ");
            return AccurateTime;
        }

        public void WriteToFile(bool date, string data)
        {
            if (date)
                File.AppendAllText(Filename,TimeNow());

            File.AppendAllText(Filename,data + "\r\n");
        }
    }
}
