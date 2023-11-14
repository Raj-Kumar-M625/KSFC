using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public static class ExcelHelper
    {
        public static void UploadXLFile(HttpPostedFileBase fileUpload, string mPath)
        {
            mPath = mPath + fileUpload.FileName.ToString();
            fileUpload.SaveAs(mPath);
        }

        #region Reading From Excel File
        public static string[] GetExcelSheets(string mFile)
        {
            string strXlsConnString;
            strXlsConnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mFile + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
            OleDbConnection xlsConn = new OleDbConnection(strXlsConnString);
            xlsConn.Open();
            DataTable xlTable = new DataTable();
            xlTable = xlsConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            System.String strExcelSheetNames = "";
            string sheetName;
            //Loop through the excel database table names and take only the tables that ends with a $ characters. Other tables are not worksheets...
            for (int lngStart = 0; lngStart < xlTable.Rows.Count; lngStart++)
            {
                sheetName = xlTable.Rows[lngStart][2].ToString().Replace("'", ""); //Remove the single-quote surrounding the table name...
                if (sheetName.EndsWith("$")) //Yes, this is a worksheet
                {
                    strExcelSheetNames += sheetName.Substring(0, sheetName.Length - 1) + "~"; //concatenate with a single-quote delimeter... to be returned as a string array later using the split function
                }
            }

            if (strExcelSheetNames.EndsWith("~")) //the last single quote needs to be removed so that the array index ends with the last sheetname
            {
                strExcelSheetNames = strExcelSheetNames.Substring(0, strExcelSheetNames.Length - 1);
            }

            xlsConn.Close();
            xlsConn.Dispose();
            char[] chrDelimter = { '~' };
            return strExcelSheetNames.Split(chrDelimter);
        }

        public static DataSet GetXLData(string xlSheetName, string xlFileName, string AdditionalFields)
        {
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + xlFileName + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
            OleDbConnection xlConn = new OleDbConnection(connstr);
            DataSet xlTDS = new DataSet("xlDataSet");
            xlConn.Open();
            OleDbDataAdapter xlDA = new OleDbDataAdapter("Select" + AdditionalFields + " * from [" + xlSheetName + "$] ", xlConn);
            xlDA.Fill(xlTDS);
            xlConn.Close();
            xlConn.Dispose();

            RemoveEmptyRows(xlTDS.Tables[0], (AdditionalFields.Length - AdditionalFields.ToLower().Replace(" as ", "").Length) / 4);
            return xlTDS;
        }

        private static void RemoveEmptyRows(DataTable dtbl, System.Int32 intNumberOfFieldsToIgnore)
        {
            System.String strFilter = "";
            //Check at least 3/4th of the columns for null value
            System.Int32 intAvgColsToCheck = Convert.ToInt32((dtbl.Columns.Count - intNumberOfFieldsToIgnore) * 0.75);
            //Can't entertain checking less than three columns.
            if (intAvgColsToCheck < 3)
            {
                intAvgColsToCheck = dtbl.Columns.Count;
            }

            //Building the filter string that checks null value in 3/4th of the total column numbers...

            //We will be doing it in reverse, checking the last three-quarter columns
            System.Int32 lngEnd = dtbl.Columns.Count;
            lngEnd = lngEnd - intAvgColsToCheck;
            for (int lngStartColumn = dtbl.Columns.Count; lngStartColumn > lngEnd; lngStartColumn--)
            {
                strFilter += "[" + dtbl.Columns[lngStartColumn - 1].ColumnName + "] IS NULL AND "; //AND to concatenate the next column in the filter
            }

            //Remove the trailing AND
            if (strFilter.Length > 1) //At least one column is added (and thus, the trailing AND)
            {
                strFilter = strFilter.Remove(strFilter.Length - 4);
            }
            DataRow[] drows = dtbl.Select(strFilter);

            //Remove the rows that are empty...
            foreach (DataRow drow in drows)
            {
                dtbl.Rows.Remove(drow);
            }
        }

        public static void BulkCopy(DataTable dtTable, string tableName)
        {
            //using (SqlConnection SqlConn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlConnection SqlConn1 = new SqlConnection(CRMUtilities.Utils.DBConnectionString))
            {
                SqlConn1.Open();
                using (var cmd = SqlConn1.CreateCommand())
                {
                    cmd.CommandText = $"truncate table {tableName}";
                    var result = cmd.ExecuteNonQuery();
                }
                using (SqlBulkCopy bc = new SqlBulkCopy((SqlConnection)SqlConn1))
                {
                    bc.DestinationTableName = tableName;
                    bc.WriteToServer(dtTable);
                    bc.Close();
                }
                SqlConn1.Close();
            }
        }
        #endregion

        #region ExportToMultipleExcelSheets
        //private static System.Text.StringBuilder SqlScript = new System.Text.StringBuilder();
        //private static System.Text.StringBuilder SqlInsert = new System.Text.StringBuilder();
        //private static bool mPredefineFile = false;

        //public static void ImportToMultipleXLSheets(System.String SqlSelect, System.String mOutputFileName)
        //{
        //    string FolderPath;
        //    FolderPath = mOutputFileName.Remove(mOutputFileName.LastIndexOf("\\"), mOutputFileName.Length - mOutputFileName.LastIndexOf("\\"));

        //    File.Copy(FolderPath + "\\Sample.xls", mOutputFileName, true);

        //    SqlConnection SqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ArmsConnStr"].ToString());
        //    SqlConn.Open();
        //    DataSet DS = new DataSet();
        //    string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mOutputFileName + ";Extended Properties='Excel 8.0'";
        //    OleDbConnection xlConn = new OleDbConnection(connstr);

        //    try
        //    {
        //        xlConn.Open();

        //        SqlDataAdapter SqlDA = new SqlDataAdapter(SqlSelect, SqlConn);
        //        SqlDA.Fill(DS);
        //        SqlConn.Close();
        //        SqlConn.Dispose();
        //        PrepareScript(DS.Tables[0]);
        //        StartImport(DS.Tables[0], xlConn);
        //    }
        //    catch (Exception exp)
        //    {
        //        throw new Exception("ImportToMultipleXLSheets", exp.InnerException);
        //    }
        //    finally
        //    {
        //        if (xlConn != null)
        //        {
        //            if (xlConn.State == ConnectionState.Open) xlConn.Close();
        //            xlConn.Dispose();
        //        }
        //        if (SqlConn != null)
        //        {
        //            if (SqlConn.State == ConnectionState.Open) SqlConn.Close();
        //            SqlConn.Dispose();
        //        }
        //    }
        //}

        //private static void CreateXLSheets(DataTable DTable, OleDbConnection xlConn, System.String XLSheetName)
        //{
        //    // Create New Excel Sheet
        //    System.Text.StringBuilder SqlFinalScript = new System.Text.StringBuilder();
        //    OleDbCommand cmdXl = new OleDbCommand();
        //    try
        //    {
        //        SqlFinalScript.Length = 0;
        //        cmdXl.Connection = xlConn;

        //        SqlFinalScript.Append("CREATE TABLE " + XLSheetName + " (");
        //        SqlFinalScript.Append(SqlScript.ToString());

        //        cmdXl.CommandText = SqlFinalScript.ToString();
        //        cmdXl.ExecuteNonQuery();
        //    }
        //    catch (Exception xlSheetExp)
        //    {
        //        throw (new Exception("CreateXLSheetException", xlSheetExp.InnerException));
        //    }
        //    finally
        //    {
        //        cmdXl.Dispose();
        //    }
        //}

        //private static string PrepareScript(DataTable DTable)
        //{
        //    // Prepare Scripts to create excel Sheet
        //    SqlInsert.Length = 0;
        //    SqlScript.Length = 0;
        //    for (int i = 0; i < DTable.Columns.Count; i++)
        //    {
        //        SqlInsert.Append("[" + DTable.Columns[i].ColumnName + "],");

        //        SqlScript.Append("[" + DTable.Columns[i].ColumnName.Replace("'", "''") + "]");

        //        if (DTable.Columns[i].DataType.ToString().ToLower().Contains("int") || DTable.Columns[i].DataType.ToString().ToLower().Contains("decimal"))
        //            SqlScript.Append(" double");
        //        else
        //            SqlScript.Append(" text");

        //        SqlScript.Append(", ");
        //    }
        //    SqlInsert.Remove(SqlInsert.Length - 1, 1);
        //    SqlScript.Remove(SqlScript.Length - 2, 1);
        //    SqlScript.Append(") ");
        //    return SqlScript.ToString();
        //}

        //private static void StartImport(DataTable DTable, OleDbConnection xlConn)
        //{
        //    Int64 rowNo = 0, xlSheetIndex = 2, TotalNoOfRecords = 0;
        //    System.String NewXLSheetName = "Sheet";
        //    System.Text.StringBuilder strInsert = new System.Text.StringBuilder();
        //    TotalNoOfRecords = DTable.Rows.Count;
        //    OleDbCommand cmdXl = new OleDbCommand();
        //    cmdXl.Connection = xlConn;
        //    if (mPredefineFile) xlSheetIndex = 1;
        //    for (int count = 0; count < DTable.Rows.Count; count++)
        //    {
        //        strInsert.Length = 0;

        //        if (rowNo == 0 && !mPredefineFile)
        //            CreateXLSheets(DTable, xlConn, NewXLSheetName + xlSheetIndex);
        //        rowNo += 1;

        //        // TotalNoOfRecords : Total no of records return by Sql Query, ideally should be set to 65535
        //        //rowNo : current Row no in the loop 
        //        if (TotalNoOfRecords > 5000 && rowNo > 5000)
        //        {
        //            xlSheetIndex += 1;
        //            if (!mPredefineFile) CreateXLSheets(DTable, xlConn, NewXLSheetName + xlSheetIndex);
        //            rowNo = 1;
        //        }
        //        strInsert.Append("Insert Into [" + NewXLSheetName + xlSheetIndex.ToString() + "$](" + SqlInsert.ToString() + ") Values (");
        //        foreach (DataColumn dCol in DTable.Columns)
        //        {
        //            if (dCol.DataType.ToString().ToLower().Contains("int"))
        //            {
        //                if (DTable.Rows[count][dCol.Ordinal].ToString() == "")
        //                    strInsert.Append("NULL");
        //                else
        //                    strInsert.Append(DTable.Rows[count][dCol.Ordinal]);
        //            }
        //            else if (dCol.DataType.ToString().ToLower().ToLower().Contains("decimal"))
        //            {
        //                if (DTable.Rows[count][dCol.Ordinal].ToString() == "")
        //                    strInsert.Append("NULL");
        //                else
        //                    strInsert.Append(DTable.Rows[count][dCol.Ordinal]);
        //            }
        //            else
        //                strInsert.Append("\"" + DTable.Rows[count][dCol.Ordinal].ToString().Replace("'", "''") + "\"");

        //            strInsert.Append(",");
        //        }
        //        strInsert.Remove(strInsert.Length - 1, 1);
        //        strInsert.Append(");");
        //        cmdXl.CommandText = strInsert.ToString();
        //        cmdXl.ExecuteNonQuery();
        //    }
        //}

        #endregion
    }
}