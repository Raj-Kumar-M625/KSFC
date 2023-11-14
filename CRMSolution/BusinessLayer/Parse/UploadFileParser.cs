using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using CRMUtilities;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Diagnostics;
using System.Data.SqlClient;

namespace BusinessLayer
{
    public abstract class UploadFileParser : IParse
    {
        protected abstract int Validate(string tableName,
                string fileName,
                ICollection<TableSchema> schemaInfo,
                List<ExcelUploadError> errorList,
                System.Data.DataTable dt,
                int maxErrors);

        protected abstract int ValidateDateType(
                                int rowPosition,
                                int cellPosition,
                                string cv,
                                TableSchema cellSchema,
                                List<ExcelUploadError> errorList,
                                System.Data.DataRow dr
                                );

        public void Parse(ExcelUploadStatus row, int maxErrors)
        {
            bool detailLogging = Utils.SiteConfigData.DetailLoggingForExcelFileUpload;
            bool deleteFileAfterUpload = Utils.SiteConfigData.DeleteExcelFileAfterUpload;

            List<ExcelUploadError> errorList = new List<ExcelUploadError>();

            System.Data.DataTable dt = null;
            string savedFileName = row.UploadFileName;
            int errorCount = 0;
            try
            {
                Business.LogError(nameof(UploadFileParser), $"{row.UploadTable} : {row.UploadFileName} ");

                ICollection<TableSchema> schemaInfo = Business.GetTableSchema(row.UploadTable);

                // Create DataTable based on schema
                dt = CreateDataTable(row.UploadTable, schemaInfo);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                errorCount = Validate(row.UploadTable, row.UploadFileName, schemaInfo, errorList, dt, maxErrors);
                sw.Stop();

                errorList.Add(new ExcelUploadError()
                {
                    MessageType = "Info",
                    Description = $"Parsing the input file took {sw.ElapsedMilliseconds / 1000} seconds"
                });

                if (errorCount == 0)
                {
                    errorList.Add(new ExcelUploadError()
                    {
                        MessageType = "Info",
                        Description = "No errors"
                    });

                    // save datatable in sql server
                    BulkCopy(dt, row.UploadTable);
                }
                else
                {
                    errorList.Add(new ExcelUploadError()
                    {
                        MessageType = "Warning",
                        Description = $"File not uploaded due to {errorCount} error(s)"
                    });
                }

                // save record to indicate progress, in db
                ExcelUploadStatus eus = new ExcelUploadStatus()
                {
                    Id = row.Id,
                    RecordCount = errorCount == 0 ? dt.Rows.Count : 0,
                    IsParsed = true,
                    ErrorCount = errorCount
                };

                Business.UpdateExcelUploadStatus(eus);

                Business.SaveParseErrors(row.Id, errorList);
            }
            catch (Exception ex)
            {
                errorCount++;

                ExcelUploadStatus eus = new ExcelUploadStatus()
                {
                    Id = row.Id,
                    RecordCount = errorCount == 0 ? dt.Rows.Count : 0,
                    IsParsed = true,
                    ErrorCount = errorCount
                };

                Business.UpdateExcelUploadStatus(eus);

                //ViewBag.Message = $"An error occured while uploading data file. {ex.ToString()}";
                errorList.Add(new ExcelUploadError()
                {
                    MessageType = "Error",
                    Description = $"{ex.ToString()}"
                });

                Business.SaveParseErrors(row.Id, errorList);
                Business.LogError(nameof(UploadFileParser), ex);
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                // delete the file that was saved on server - if opted to do so or error count > 0
                if (System.IO.File.Exists(savedFileName) && (deleteFileAfterUpload || errorCount > 0))
                {
                    System.IO.File.Delete(savedFileName);
                }

                if (errorCount > 0)
                {
                    Task.Run(async () =>
                    {
                        await Business.LogAlert("EFileUpload", $"{errorCount} errors in {row.LocalFileName}");
                    });
                }
            }
        }

        protected virtual string GetExcelColumnLabel(int cellPosition)
        {
            if (cellPosition < 1 || cellPosition > 702)
            {
                return "";
            }

            cellPosition -= 1;

            int p2 = cellPosition % 26;
            int p1 = (cellPosition - p2) / 26;
            p2++;

            if (p1 < 0 || p1 > 26 || p2 < 1 || p2 > 26)
            {
                return "";
            }

            return $"{CellMnemonics[p1]}{CellMnemonics[p2]}".Trim();
        }

        protected virtual int ValidateCellValueWithSchema(int rowPosition, int cellPosition,
            string cv, TableSchema cellSchema, List<ExcelUploadError> errorList, System.Data.DataRow dr)
        {
            if (String.IsNullOrWhiteSpace(cv) && cellSchema.IsStringDataType)
            {
                if (cellSchema.IsNullable == 1)
                {
                    dr[cellPosition - 1] = "";
                    return 0;
                }
                else
                {
                    errorList.Add(new ExcelUploadError()
                    {
                        CellReference = GetCellReference(rowPosition, cellPosition),
                        MessageType = "Error",
                        Description = $"Invalid value for {cellSchema.ColumnName}",
                        ExpectedValue = $"{cellSchema.DataTypeDisplayName}",
                        ActualValue = $"'{cv}'"
                    });
                    return 1;
                }
            }

            // perform validations
            int returnValue = 0;
            if (cellSchema.IsStringDataType)
            {
                string cvAsString = cv;
                int cvSize = cvAsString.Length;
                if (cvSize > cellSchema.CharMaxLen)
                {
                    errorList.Add(new ExcelUploadError()
                    {
                        CellReference = GetCellReference(rowPosition, cellPosition),
                        MessageType = "Error",
                        Description = $"String is too large for {cellSchema.ColumnName}",
                        ExpectedValue = $"{cellSchema.DataTypeDisplayName}",
                        ActualValue = cvAsString
                    });
                    returnValue = 1;
                }
                else
                {
                    dr[cellPosition - 1] = cvAsString;
                }
            }
            else if (cellSchema.IsDecimalDataType)
            {
                decimal decimalNumber = 0;
                double doubleNumber = 0;
                string text = cv;
                if (String.IsNullOrWhiteSpace(text))
                {
                    text = "0.0";
                }
                bool decimalConvertStatus = decimal.TryParse(text, out decimalNumber);
                bool doubleConvertStatus = double.TryParse(text, out doubleNumber);

                // number is also attempted to be parsed as double
                // as decimal.tryParse("2E-3", out decimalNumber) 
                // does not convert the number to 0.002

                if (decimalConvertStatus == false && doubleConvertStatus == false)
                {
                    // try to convert with double

                    errorList.Add(new ExcelUploadError()
                    {
                        CellReference = GetCellReference(rowPosition, cellPosition),
                        MessageType = "Error",
                        Description = $"Invalid value for {cellSchema.ColumnName}",
                        ExpectedValue = $"{cellSchema.DataTypeDisplayName}",
                        ActualValue = text
                    });

                    returnValue = 1;
                }
                else
                {
                    if (decimalConvertStatus)
                    {
                        dr[cellPosition - 1] = decimalNumber;
                    }
                    else if (doubleConvertStatus)
                    {
                        dr[cellPosition - 1] = doubleNumber;
                    }
                }
            }
            else if (cellSchema.IsNumberDataType)
            {
                long n = 0;
                string text = cv;
                if (String.IsNullOrWhiteSpace(text))
                {
                    text = "0";
                }
                bool status = long.TryParse(text, out n);
                if (status == false)
                {
                    errorList.Add(new ExcelUploadError()
                    {
                        CellReference = GetCellReference(rowPosition, cellPosition),
                        MessageType = "Error",
                        Description = $"Invalid value for {cellSchema.ColumnName}",
                        ExpectedValue = $"{cellSchema.DataTypeDisplayName}",
                        ActualValue = text
                    });

                    returnValue = 1;
                }
                else
                {
                    dr[cellPosition - 1] = n;
                }
            }
            else if (cellSchema.IsDateDataType)
            {
                // date type parsing is different in Excel and in csv file
                // excel gives a double, csv has date in yyyy-mm-dd format.
                returnValue = ValidateDateType(rowPosition, cellPosition, cv, cellSchema, errorList, dr);
            }
            else
            {
                errorList.Add(new ExcelUploadError()
                {
                    CellReference = GetCellReference(rowPosition, cellPosition),
                    MessageType = "Error",
                    Description = $"Data type {cellSchema.DataType} defined in the schema is not recognized",
                    ExpectedValue = $"",
                    ActualValue = cv
                });

                returnValue = 1;
            }

            return returnValue;
        }

        protected virtual string GetCellReference(int rowPosition, int cellPosition) =>
            $"{GetExcelColumnLabel(cellPosition)}{rowPosition}";

        private static char[] CellMnemonics = new char[] { ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private System.Data.DataTable CreateDataTable(string tableName, ICollection<TableSchema> schemaInfo)
        {
            System.Data.DataTable dt = new System.Data.DataTable(tableName);

            foreach (var ts in schemaInfo.OrderBy(x => x.Position))
            {
                Type t;
                if (ts.IsStringDataType)
                {
                    t = typeof(String);
                }
                else if (ts.IsDecimalDataType)
                {
                    t = typeof(decimal);
                }
                else if (ts.IsNumberDataType)
                {
                    t = typeof(long);
                }
                else if (ts.IsDateDataType)
                {
                    t = typeof(DateTime);
                }
                else
                {
                    t = typeof(String);
                }

                dt.Columns.Add(new System.Data.DataColumn(ts.ColumnName, t));
            }

            return dt;
        }

        public static void BulkCopy(System.Data.DataTable dtTable, string tableName)
        {
            //using (SqlConnection SqlConn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlConnection SqlConn1 = new SqlConnection(CRMUtilities.Utils.DBConnectionString))
            {
                SqlConn1.Open();
                using (var cmd = SqlConn1.CreateCommand())
                {
                    cmd.CommandText = $"truncate table {tableName}";
                    cmd.CommandTimeout = 200;
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
    }
}
