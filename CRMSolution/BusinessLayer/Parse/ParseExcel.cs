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

namespace BusinessLayer
{
    public class ParseExcel : UploadFileParser
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

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

                // validate the number of sheets
                if (workbookPart.WorksheetParts.Count() != 1)
                {
                    errorList.Add(new ExcelUploadError()
                    {
                        MessageType = "Error",
                        Description = "Invalid number of sheets in input file.",
                        ActualValue = $"{workbookPart.WorksheetParts.Count()}",
                        ExpectedValue = $"1"
                    });
                    errorCount++;
                    return errorCount;
                }

                // cache shared strings from xlsx workbook;
                OpenXmlSharedStrings = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>();

                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                int rowCount = sheetData.Elements<Row>().Count();

                // first row is header row.  There should be minimum 2 rows
                if (rowCount <= 1)
                {
                    errorList.Add(new ExcelUploadError()
                    {
                        MessageType = "Error",
                        Description = "Input Excel file does not have any data."
                    });
                    errorCount++;
                    return errorCount;
                }

                IEnumerable<Row> allExcelRows = sheetData.Elements<Row>();

                // validate number of columns
                Row firstRow = allExcelRows.First();
                int columnCount = firstRow.Elements<Cell>().Count();
                if (columnCount != schemaInfo.Count)
                {
                    errorList.Add(new ExcelUploadError()
                    {
                        MessageType = "Error",
                        Description = $"Invalid number of columns in input Excel file",
                        ExpectedValue = $"{schemaInfo.Count}",
                        ActualValue = $"{columnCount}"
                    });
                    errorCount++;
                    return errorCount;
                }

                // validate column headers
                IEnumerable<Cell> headerColumns = firstRow.Elements<Cell>();
                foreach (var sc in schemaInfo.OrderBy(x => x.Position))
                {
                    Cell singleHeaderCell = headerColumns.ElementAt(sc.Position - 1);
                    // get the value from single header cell
                    string colHeader = GetCellValue(singleHeaderCell);
                    if (colHeader == null || sc.ColumnName.Equals(colHeader, StringComparison.OrdinalIgnoreCase) == false)
                    {
                        errorCount++;
                        errorList.Add(new ExcelUploadError()
                        {
                            CellReference = singleHeaderCell.CellReference, // GetCellReference(1, sc.Position),
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

                int totalRows = allExcelRows.Count();
                int rowToStartAt = 2;

                int rowsRead = 0;
                // start from row 2 - as first row is header row - ignore it;
                // if there are 50 errors - stop processing any further
                for (int r = rowToStartAt; r <= totalRows && errorCount < maxErrors; r++)
                {
                    rowsRead++;
                    System.Data.DataRow dr = dt.NewRow();
                    Row processingRow = allExcelRows.ElementAt(r - 1);
                    int rowNumberInSheet = (int)processingRow.RowIndex.Value;
                    IEnumerable<Cell> processingColumns = processingRow.Elements<Cell>();
                    int c = 0;
                    for (c = 1; c <= expectedColumnCount; c++)
                    {
                        //Cell processingCell = null;

                        // if there are empty rows in between, we have to go by processingrow.RowIndex

                        string absoluteCellReference = GetCellReference(rowNumberInSheet, c); // A1 B1 C1 etc.
                        Cell processingCell = processingColumns.FirstOrDefault(x =>
                                        x.CellReference.HasValue &&
                                        x.CellReference.Value.Equals(absoluteCellReference, StringComparison.OrdinalIgnoreCase));

                        //if (processingColumns.Count() >= c)
                        //{
                        //    processingCell = processingColumns.ElementAt(c - 1);
                        //}
                        string processingCellValue = GetCellValue(processingCell);

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
                            errorCount += ValidateCellValueWithSchema(rowNumberInSheet, c, processingCellValue, cellSchema, errorList, dr);
                        }
                    }

                    // if there are no errors - add row to datatable;
                    if (errorCount == 0)
                    {
                        dt.Rows.Add(dr);
                    }
                }

                errorList.Add(new ExcelUploadError()
                {
                    MessageType = "Info",
                    Description = $"Process has read {rowsRead} rows of data."
                });

                return errorCount;
            }
        }

        private IEnumerable<SharedStringItem> OpenXmlSharedStrings = null;

        private SharedStringItem GetSharedStringItemById(int id)
        {
            return OpenXmlSharedStrings?.ElementAt(id);
        }

        private string GetCellValue(Cell c)
        {
            if (c == null)
            {
                return "";
            }

            string text = "****";
            if (c.DataType != null && c.DataType == CellValues.SharedString)
            {
                int id = -1;
                if (Int32.TryParse(c.InnerText, out id))
                {
                    SharedStringItem item = GetSharedStringItemById(id);
                    if (item == null)
                    {
                        text = "****";
                    }
                    else if (item.Text != null)
                    {
                        text = item.Text.Text;
                    }
                    else if (item.InnerText != null)
                    {
                        text = item.InnerText;
                    }
                    else if (item.InnerXml != null)
                    {
                        text = item.InnerXml;
                    }
                }
            }
            else
            {
                text = c?.CellValue?.Text;
            }

            if (String.IsNullOrEmpty(text) == false)
            {
                text = text.Trim();
            }

            return text;
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
            double n = 0;
            string text = cv;
            if (String.IsNullOrWhiteSpace(text))
            {
                text = "0";
            }
            bool status = double.TryParse(text, out n);
            int returnValue = 0;
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
                dr[cellPosition - 1] = DateTime.FromOADate(n);
            }

            return returnValue;
        }
    }
}
