using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CusDll
{
    class MyFunction
    {
        public static string EncodeS(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encryptedPassword = Convert.ToBase64String(b);
            return encryptedPassword;
        }
        public static string DecodeS(string encrString)
        {
            string decryptedPassword;
            if (encrString != "Admin")
            {
                byte[] b = Convert.FromBase64String(encrString);
                decryptedPassword = System.Text.ASCIIEncoding.ASCII.GetString(b);

            }
            else
            {
                decryptedPassword = encrString;
            }
            return decryptedPassword;
        }
        static SqlConnection conn = new SqlConnection();
        public static void SaveFunction(string[] ColumnList, string[] DataList, string TableName)
        {
            int ColumnCount = ColumnList.Length;
            string str = "Insert Into " + TableName + " (";
            for (int i = 0; i < ColumnCount; i++)
            {
                str += ColumnList[i] + ",";
            }
            str = str.Substring(0, str.Length - 1);
            str += ") Values (";

            for (int i = 0; i < ColumnCount; i++)
            {
                str += "@" + ColumnList[i] + ",";
            }
            str = str.Substring(0, str.Length - 1);
            str += ")";

            SqlCommand cmd = new SqlCommand(str,conn);

            for (int i = 0; i < ColumnCount; i++)
            {
                if (DataList[i] != "")
                {
                    cmd.Parameters.AddWithValue("@" + ColumnList[i], DataList[i]);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@" + ColumnList[i], DBNull.Value);
                }


            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

        }
        public static void UpdateToFunction(string[] ColumnList, string[] DataList, string TableName, string filter)
        {
            int ColumnCount = ColumnList.Length;
            string str = "Update " + TableName + " set ";
            for (int i = 0; i < ColumnCount; i++)
            {
                str += ColumnList[i] + "=@" + ColumnList[i] + ",";
            }
            str = str.Substring(0, str.Length - 1);
            if (filter != "") str += " Where " + filter;
            SqlCommand cmd = new SqlCommand(str, conn);
            for (int i = 0; i < ColumnCount; i++)
            {
                if (DataList[i] != "")
                {
                    cmd.Parameters.AddWithValue("@" + ColumnList[i], DataList[i]);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@" + ColumnList[i], DBNull.Value);
                }
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void DeleteFunction(string TableName, string Filter)
        {
            string str = "Delete From " + TableName;
            if (Filter != "") str += " Where " + Filter;
            SqlCommand cmd = new SqlCommand(str, conn);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void DataInsertUpdateDel(string query) //query so tar cmd ka insert update del
        {
            SqlConnection conn = new SqlConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public DataSet LoadTopColDesc(int num, string tblname, string Ocol)
        {
            string query = "Select top(" + num + ") * from " + tblname + " order by " + Ocol + " Desc ";
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adt.Fill(ds, tblname);
            conn.Close();
            return ds;
        }
        public DataSet LoadAllColDesc(string tblname, string Ocol)
        {
            string query = "Select * from " + tblname + " order by " + Ocol + " Desc ";
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adt.Fill(ds, tblname);
            conn.Close();
            return ds;
        }
        public DataSet LoadAllCollikeSearch(string tblname, string colname, string input)
        {
            string query = "Select * from " + tblname + " where " + colname + "  like N'%" + input + "%' ";
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adt.Fill(ds, tblname);
            conn.Close();
            return ds;
        }
        public DataSet LoadAllColEqSearch(string tblname, string colname, string input)
        {
            string query = "Select * from " + tblname + " where " + colname + " =N'" + input + "' ";
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adt.Fill(ds, tblname);
            conn.Close();
            return ds;
        }
        public DataSet LoadAllNoOneCol(string tblname, string Ocol, string Data)
        {
            string query = "Select * from " + tblname + " where  " + Ocol + "!=N'" + Data + "' order by " + Ocol + " Desc ";
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adt.Fill(ds, tblname);
            conn.Close();
            return ds;
        }
        public DataSet LoadDDl(string tblname, string colOne, string coltwo)
        {
            string query = "Select " + colOne + "," + coltwo + " from " + tblname + " ";
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adt.Fill(ds, tblname);
            conn.Close();
            return ds;
        }

        public DataSet CreateDataSet(string query, int Tout)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            cmd.CommandTimeout = Tout;
            DataSet ds = new DataSet();
            adt.Fill(ds);
            conn.Close();
            return ds;
        }
        public bool CheckData(string Query)
        {
            DataSet retds = null;
            retds = CreateDataSet(Query,50);
            if (retds.Tables[0].Rows.Count > 0)
            { return true; }
            else
            { return false; }
        }
        public string SearchQty(string tbl, string ColIn, string ColOut, string Data)
        {
            string OutPutData = "မရှိပါ";
            string Query = "Select * from " + tbl + " where " + ColIn + "=N'" + Data + "'";
            DataSet ds = CreateDataSet(Query,100);
            if (ds.Tables[0].Rows.Count > 0)
            {
                OutPutData = ds.Tables[0].Rows[0][ColOut].ToString();
            }
            return OutPutData;
        }
        public string SearchQtyByTCol(string tbl, string ColIn, string ColInT, string ColOut, string DataO, string DataT)
        {
            string OutPutData = "မရှိပါ";
            string Query = "Select * from " + tbl + " where " + ColIn + "=N'" + DataO + "' and " + ColInT + "=N'" + DataT + "' ";
            DataSet ds = CreateDataSet(Query,100);
            if (ds.Tables[0].Rows.Count > 0)
            {
                OutPutData = ds.Tables[0].Rows[0][ColOut].ToString();
            }
            return OutPutData;
        }

     
    }
}
