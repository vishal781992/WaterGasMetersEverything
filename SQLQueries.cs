using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System;

namespace WaterGasTool
{
    
    public static class Utilities
    {
        #region Check For Null String

        public static string CheckForNullString(string s)
        {
            string str = (s == null) ? string.Empty : s;
            return str;
        }

        #endregion Check For Null String

        #region Check For Null

        public static T CheckForNull<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }

        public static string CheckForNull(object obj)
        {
            if (!DBNull.Value.Equals(obj))
                return (string)obj;
            else return string.Empty;
        }

        #endregion Check For Null
    }
    class SQLQueries
    {
        #region BackBench functions
        protected string dbNetworkServerName = string.Empty, dbUsername = string.Empty, dbPassword = string.Empty, dbNameOfDatabase= string.Empty;
        public string MessageFromDatabase;

        List<string> DataFromDB = new List<string>();
        public SQLQueries(string ServerName,string NameDB,string Username,string Password)
        {
            this.dbNetworkServerName = ServerName;//netserver3
            this.dbUsername = Username;//power
            this.dbPassword = Password;//power
            this.dbNameOfDatabase = NameDB;//LoraGasVision
        }

        public static DataTable ExecuteQuery(string query, string connectionString)
        {
            DataTable table = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        conn.Open();

                        adapter.Fill(table);

                        conn.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace,
                    "Program Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return table;
        }
        #endregion BackBench functions

        #region ConnectionString
        public string ConnectionStringBuilder()
        {
            string connectionstring = "Server=" + this.dbNetworkServerName +
                                      " Database=" + this.dbNameOfDatabase + 
                                      "; User=" + this.dbUsername + 
                                      "; Password=" + this.dbPassword + ";";
            return connectionstring;
        }
        #endregion ConnectionString

        #region GrabWitDevEUI
        public string GrabADatabaseWithDevEUI(string devui, string columnToAccessFromDB)
        {
            string query = "select dbo.Meter." + columnToAccessFromDB + " from dbo.Meter where dbo.Meter.DevEUI =" + "'" + devui + "'"; //Batch, MeterID
            DataTable dt = ExecuteQuery(query, ConnectionStringBuilder());
            if (dt.Rows.Count <= 0)
                return "No data";
            foreach (DataRow dr in dt.Rows) { MessageFromDatabase = Utilities.CheckForNullString(Utilities.CheckForNull<string>(dr[columnToAccessFromDB])); }
            return MessageFromDatabase;
        }
        #endregion GrabWitDevEUI

        #region GrabWithMeterID
        public string GrabADatabaseWithMeterID(string MeterID, string columnToAccessFromDB) // meterId to check the MAr already exists or not
        {

            string query = "select dbo.Meter." + columnToAccessFromDB + " from dbo.Meter where dbo.Meter.MeterID =" + "'" + MeterID + "'"; //Batch, MeterID

            DataTable dt = ExecuteQuery(query, ConnectionStringBuilder());

            //if (dt.Rows.Count <= 0)
            //    return "No data";

            foreach (DataRow dr in dt.Rows)
            {
                MessageFromDatabase = Utilities.CheckForNullString(Utilities.CheckForNull<string>(dr[columnToAccessFromDB]));
            }
            return MessageFromDatabase;
        }
        #endregion GrabWithMeterID

        #region PostDataToAMRCheck

        public string PostDataToAMRCheck(string DevEUI, DateTime date, string initialofUser, string AppKey, string FwVersion, string AppEUI, string FreqChannels, string TxPower)//for AMR check
        {
            try
            {
                string query = "UPDATE dbo.Meter set AMRchkBy = " + "'" + initialofUser + "'" + ",AMRchkDate = " + "'" + date + "'" + ",AppKey = " + "'" + AppKey + "'" + ",AppEUI = " + ",ModemFirmwareRev = " + "'" + FwVersion + "'"+ ",AppEUI = " + "'" + AppEUI + "'" + ",FreqChannels = " + "'" + FreqChannels + "'" + ",TxPower = " + "'" + TxPower + "'" + "where DevEUI =" + "'" + DevEUI + "'";
                DataTable dt = ExecuteQuery(query, ConnectionStringBuilder());

                foreach (DataRow dr in dt.Rows)
                {
                    MessageFromDatabase = Utilities.CheckForNullString(Utilities.CheckForNull<string>(dr[DevEUI]));
                }
                return MessageFromDatabase;
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + Environment.NewLine +
                    e.StackTrace + Environment.NewLine + Environment.NewLine +
                    e.Source,
                    "Program Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
        }
        #endregion PostDataToAMRCheck

        #region GrabASerialNumber
        public List<string> GrabASerialNumber()//I will have to provide the range of the meters to the loop
        {
            //string query = "select dbo.Meter." + columnToAccessFromDB + " from dbo.Meter where dbo.Meter.DevEUI =" + "'" + devui + "'"; //Batch, MeterID
            string query = "select * from dbo.Meter order by [MeterID] desc";
            //string query = "select dbo.Meter." + "StatusCode" + " from dbo.Meter where dbo.Meter.MeterID =" + "'" + 20000005 + "'"; //Batch, MeterID
            DataTable dt = ExecuteQuery(query, ConnectionStringBuilder());
            if (dt.Rows.Count <= 0)
                return DataFromDB;
            foreach (DataRow dr in dt.Rows) { DataFromDB.Add(Utilities.CheckForNullString(Utilities.CheckForNull<string>(dr["MeterID"]))); }

            return DataFromDB;
        }
        #endregion GrabASerialNumber

        #region PostDataToMeter

        public bool PostDataToMeter(long SerialNum, DateTime date, string DevEUI, string FwVersion, string MeterTypeCode, string StatusCode,string AppKey)//for AMR check
        {
            try
            {
                string query = "INSERT INTO dbo.Meter " +
                    "(MeterID , MeterTypeCode , DialConstant , Dials , DemandDials , TamperProof , TimeRun ,  Hold , Obsolete , New, ReceiveDate, Batch , MemorySize , DevEUI , AppKey , Comments, StatusCode ) Values " +
                    "('"+SerialNum+"','"+MeterTypeCode+"',1,1,0,0,0,0,0,0,'"+ date + "','202020',0, '"+DevEUI+"','"+ AppKey +"','vishalComment',"+ "'"+StatusCode + "'"+")";
                //string query = "UPDATE dbo.Meter set MeterID = " + "'" + SerialNum + "'" + ",ReceiveDate = " + "'" + date + "'" +
                //    ",FirmwareRevision = " + "'" + FwVersion + "'" + ",DevEUI =" + "'" + DevEUI + "'";

                try
                {
                    DataTable dt = ExecuteQuery(query, ConnectionStringBuilder());
                }
                catch { return false; }

                return true;
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + Environment.NewLine +
                    e.StackTrace + Environment.NewLine + Environment.NewLine +
                    e.Source,
                    "Program Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }
        #endregion PostDataToMeter

        #region GetDataFromDB

        public List<dynamic> GetDataFromDB(string ToFindColumn, string TableName)//for AMR check
        {
            //DataFromDB.Clear();
            List<dynamic> dataFromDB = new List<dynamic>();

            try
            {
                string query = "SELECT "+ ToFindColumn +" FROM dbo."+ TableName;

                DataTable dt = ExecuteQuery(query, ConnectionStringBuilder());

                if (dt.Rows.Count >= 0)
                {
                    foreach (DataRow dr in dt.Rows) 
                    {
                        dataFromDB.Add(Utilities.CheckForNullString(Utilities.CheckForNull<dynamic>(dr[ToFindColumn]))); 
                    }
                    return dataFromDB;
                }
                return null;
            }

            catch (Exception e)
            {
                return null;
            }
        }
        #endregion GetDataFromDB



    }
}
