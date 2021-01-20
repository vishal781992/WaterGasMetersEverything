using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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

        public string ConnectionStringBuilder()
        {
            string connectionstring = "Server=" + this.dbNetworkServerName +
                                      " Database=" + this.dbNameOfDatabase + 
                                      "; User=" + this.dbUsername + 
                                      "; Password=" + this.dbPassword + ";";
            return connectionstring;
        }

        public string GrabADatabaseWithDevEUI(string devui, string columnToAccessFromDB)
        {
            string query = "select dbo.Meter." + columnToAccessFromDB + " from dbo.Meter where dbo.Meter.DevEUI =" + "'" + devui + "'"; //Batch, MeterID
            DataTable dt = ExecuteQuery(query, ConnectionStringBuilder());
            if (dt.Rows.Count <= 0)
                return "No data";
            foreach (DataRow dr in dt.Rows) { MessageFromDatabase = Utilities.CheckForNullString(Utilities.CheckForNull<string>(dr[columnToAccessFromDB])); }
            return MessageFromDatabase;
        }
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
        #region PostDataToAMRCheck

        public string PostDataToAMRCheck(string MeterId, DateTime date, string initialofUser, string AppKey, string FwVersion)//for AMR check
        {
            try
            {
                string query = "UPDATE dbo.Meter set AMRchkBy = " + "'" + initialofUser + "'" + ",AMRchkDate = " + "'" + date + "'" + ",AppKey = " + "'" + AppKey + "'" + ",AppEUI = " + ",ModemFirmwareRev = " + "'" + FwVersion + "'" + "where MeterID =" + "'" + MeterId + "'";
                DataTable dt = ExecuteQuery(query, ConnectionStringBuilder());

                foreach (DataRow dr in dt.Rows)
                {
                    MessageFromDatabase = Utilities.CheckForNullString(Utilities.CheckForNull<string>(dr[MeterId]));
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

    }
}
