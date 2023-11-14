using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DatabaseLayer;
using System.Globalization;
using CRMUtilities;
using Newtonsoft.Json;
using System.Xml;
using System.Reflection;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;

namespace BusinessLayer
{
    public class ParseCSV : UploadFileParser
    {
        protected override int Validate(string tableName,
                string fileName,
                ICollection<TableSchema> schemaInfo,
                List<ExcelUploadError> errorList,
                System.Data.DataTable dt,
                int maxErrors)
        {
            int expectedColumnCount = schemaInfo.Count;
            int errorCount = 0;

            int rowCount = 0;

            char[] separators = Utils.FileUploadCSVSeparator();
            char[] trimChars = Utils.FileUploadTrimChars();

            using (StreamReader sr = new StreamReader(fileName))
            {
                string row;
                while( (row = sr.ReadLine()) != null)
                {
                    if (errorCount >= maxErrors)
                    {
                        return errorCount;
                    }

                    rowCount++;

                    // ignore empty rows;
                    if (String.IsNullOrEmpty(row) || String.IsNullOrWhiteSpace(row))
                    {
                        continue;
                    }

                    string[] columns = row.Split(separators);

                    // file must have expected number of columns.
                    int columnCount = columns.Length;
                    if (columnCount != schemaInfo.Count)
                    {
                        errorList.Add(new ExcelUploadError()
                        {
                            MessageType = "Error",
                            Description = $"Invalid number of columns in input file at row {rowCount}",
                            ExpectedValue = $"{schemaInfo.Count}",
                            ActualValue = $"{columnCount}"
                        });
                        errorCount++;
                        continue;
                    }

                    // if errors in first row itself - return;
                    if (rowCount == 1 && errorCount > 0)
                    {
                        return errorCount;
                    }

                    // first row is header row.
                    if ( rowCount == 1)
                    {
                        // validate column headers
                        int c = 0;
                        foreach (var sc in schemaInfo.OrderBy(x => x.Position))
                        {
                            c++;
                            string colHeader = columns[sc.Position - 1].Trim(trimChars);
                            if (colHeader == null || sc.ColumnName.Equals(colHeader, StringComparison.OrdinalIgnoreCase) == false)
                            {
                                errorCount++;
                                errorList.Add(new ExcelUploadError()
                                {
                                    CellReference = GetCellReference(1, c),
                                    MessageType = "Error",
                                    Description = $"Invalid column name",
                                    ExpectedValue = $"{sc.ColumnName}",
                                    ActualValue = (colHeader == null) ? "null" : colHeader
                                });
                            }
                        }

                        // if there are errors in column header - don't process data
                        if (errorCount > 0)
                        {
                            errorList.Add(new ExcelUploadError()
                            {
                                MessageType = "Warning",
                                Description = $"Ensure you are loading the correct file"
                            });

                            return errorCount;
                        }

                        continue;
                    } // end of header validation

                    // if row is empty or does not have expected columns - report as error;
                    System.Data.DataRow dr = dt.NewRow();
                    for (int c = 1; c <= expectedColumnCount; c++)
                    {
                        string processingCellValue = columns[c - 1].Trim(trimChars);

                        // validate the cell value with schema
                        TableSchema cellSchema = schemaInfo.Where(x => x.Position == c).FirstOrDefault();
                        if (cellSchema == null)
                        {
                            // condition should not occur
                            errorList.Add(new ExcelUploadError()
                            {
                                MessageType = "Error",
                                Description = $"Schema info for column {GetExcelColumnLabel(c)} not found."
                            });
                            errorCount++;
                        }
                        else
                        {
                            errorCount += ValidateCellValueWithSchema(rowCount, c, processingCellValue, cellSchema, errorList, dr);
                        }
                    }

                    // if there are no errors - add row to datatable;
                    if (errorCount == 0)
                    {
                        dt.Rows.Add(dr);
                    }
                }

                if (rowCount <= 2)
                {
                    errorList.Add(new ExcelUploadError()
                    {
                        MessageType = "Error",
                        Description = "Input file does not have any data."
                    });
                    errorCount++;
                    return errorCount;
                }

                errorList.Add(new ExcelUploadError()
                {
                    MessageType = "Info",
                    Description = $"Process has read {rowCount} rows of data."
                });

                return errorCount;
            }
        }

        protected override int ValidateDateType(
                                int rowPosition,
                                int cellPosition,
                                string cv,
                                TableSchema cellSchema,
                                List<ExcelUploadError> errorList,
                                System.Data.DataRow dr
                                )
        {
            int returnValue = 0;
            string text = cv;
            if (String.IsNullOrWhiteSpace(text))
            {
                text = "0";
            }
            DateTime n;
            var culture = CultureInfo.CreateSpecificCulture("en-GB");
            bool status = DateTime.TryParse(text, culture, DateTimeStyles.None, out n);
            //bool status = DateTime.TryParse(text, out n);
            if (status == false)
            {
                errorList.Add(new ExcelUploadError()
                {
                    CellReference = GetCellReference(rowPosition, cellPosition),
                    MessageType = "Error",
                    Description = $"Invalid value for {cellSchema.ColumnName}",
                    ExpectedValue = $"{cellSchema.DataTypeDisplayName}",
                    ActualValue = cv
                });

                returnValue = 1;
            }
            else
            {
                dr[cellPosition - 1] = n;
            }

            return returnValue;
        }
    }
}
