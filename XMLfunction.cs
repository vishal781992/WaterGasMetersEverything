using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WaterGasTool
{
    class XMLfunction
    {
        public string TextWENeed = string.Empty;

        public string XMLRequestData(string ToSearch, [OptionalAttribute] string pathOfFile)  //helps finding the company name in the xml file
        {
            string returnString = string.Empty;
            XmlTextReader reader = new XmlTextReader(pathOfFile);
            while (reader.Read())
            {
                if (reader.Name == "RootFolder")
                {
                    reader.Read();
                    returnString = reader.Value;
                    reader.Read();
                    //if (reader.Value == ToSearch)
                    //{
                    //    do { reader.Read(); } while (reader.Name != "Text");
                    //    //reader.Read();
                    //    if (reader.Name == "Text")// reader.Read();
                    //    {
                    //        reader.Read();
                    //        returnString = reader.Value;
                    //    }
                    //}
                }
            }
            return returnString;
        }

        public string ConfigFileExtractor(string ToSearch, string pathOfFile)
        {
            string ToSearchModified = "<" + ToSearch + ">";
            string TosearchEnd = "</" + ToSearch + ">";
            string[] tempDataArray = File.ReadAllLines(pathOfFile);
            foreach (string LineIN in tempDataArray)
            {
                if (LineIN.Contains(ToSearchModified))
                {
                    TextWENeed = LineIN.Substring(ToSearchModified.Length, (LineIN.Length - TosearchEnd.Length - ToSearchModified.Length));
                }
            }
            return TextWENeed;
        }

    }

}
