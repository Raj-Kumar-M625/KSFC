using BusinessLayer;
using CRMUtilities;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using _Excel = Microsoft.Office.Interop.Excel;

namespace EpicCrmWebApi
{
    [Authorize(Roles = "Admin")]
    public class ExcelController : BaseDashboardController
    {
        [CheckRightsAuthorize(Feature = FeatureEnum.UploadDataFeature)]
        public ActionResult Index()
        {
            PutDataInViewBag();

            return View();
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.UploadDataFeature)]
        public ActionResult DeletePendingUpload(long uploadId)
        {
            if (uploadId > 0)
            {
                try
                {
                    Business.DeletePendingUpload(uploadId);
                }
                catch(Exception ex)
                {
                    Business.LogError($"{nameof(DeletePendingUpload)}", ex);
                }
            }
            
            return RedirectToAction("Index");
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult GetParseErrors(long id)
        {
            ICollection<ExcelUploadError> items = Business.GetParseErrors(id);
            return PartialView("_ShowParseErrors", items);
        }

        /// <summary>
        /// Upload and store only
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="IsCompleteRefresh"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.UploadDataFeature)]
        public ActionResult Upload3(string tableName, bool IsCompleteRefresh)
        {
            PutDataInViewBag();
            ViewBag.DataType = tableName;

            bool detailLogging = Utils.SiteConfigData.DetailLoggingForExcelFileUpload;

            HttpPostedFileBase file = Request.Files["UploadedFile"];

            List<ErrorObject> errorList = new List<ErrorObject>();
            if (String.IsNullOrEmpty(tableName) || file == null || file.ContentLength == 0)
            {
                errorList.Add(new ErrorObject()
                {
                    MessageType = "Error",
                    Description = "Please upload the file with correct type name selected!"
                });

                ViewBag.ErrorList = errorList;
                return View("Index");
            }

            string fileName = file.FileName;
            string fileType = fileName.Split(new char[] { '.' })[1];

            string savedFileName = Helper.NewSaveFileName(Utils.SiteConfigData.SiteName ?? "", tableName, fileType);
            try
            {
                if (detailLogging)
                {
                    Business.LogError(nameof(ExcelController), $"{tableName} : {file.FileName} : {savedFileName} : {file.ContentLength}");
                }

                file.SaveAs(savedFileName);

                // save record to indicate progress, in db
                // Here we want to save the upload type that is displayed to the user
                // and not the table name;
                var optionsList = ViewBag.OptionsList as ICollection<CodeTableEx>;
                var optionRec = optionsList?.Where(x => x.Code == tableName).FirstOrDefault();
                DomainEntities.ExcelUploadStatus eus = new DomainEntities.ExcelUploadStatus()
                {
                    TenantId = Utils.SiteConfigData.TenantId,
                    UploadType = optionRec?.CodeName ?? "",
                    UploadTable = tableName,
                    UploadFileName = savedFileName,
                    IsCompleteRefresh = IsCompleteRefresh,
                    RecordCount = 0,
                    RequestedBy = CurrentUserStaffCode,
                    IsPosted = false,
                    LocalFileName = fileName,
                    IsParsed = false,
                    ErrorCount = 0,
                    IsLocked = false,
                    LockTimestamp = DateTime.MinValue
                };

                Business.CreateExcelUploadStatus(eus);

                errorList.Add(new ErrorObject()
                {
                    MessageType = "Info",
                    Description = "File is uploaded and will be processed; Please check processing status on this page, after few minutes."
                });

                // refresh excel upload status in view bag
                PutExcelUploadStatusInViewBag();

                ViewBag.ErrorList = errorList;
            }
            catch (Exception ex)
            {
                //ViewBag.Message = $"An error occured while uploading data file. {ex.ToString()}";
                errorList.Add(new ErrorObject()
                {
                    MessageType = "Error",
                    Description = $"{ex.ToString()}"
                });

                ViewBag.ErrorList = errorList;
                Business.LogError(nameof(ExcelController), ex.ToString());
            }

            return View("Index");
        }

        /// <summary>
        /// Load/store/process/return status
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="IsCompleteRefresh"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.UploadDataFeature)]
        public ActionResult Upload2(string tableName, bool IsCompleteRefresh)
        {
            PutDataInViewBag();
            ViewBag.DataType = tableName;

            //bool detailLogging = Utils.ParseBoolString(ConfigurationManager.AppSettings["DetailLoggingForExcelFileUpload"]);
            //bool deleteFileAfterUpload = Utils.ParseBoolString(ConfigurationManager.AppSettings["DeleteExcelFileAfterUpload"]);

            bool detailLogging = Utils.SiteConfigData.DetailLoggingForExcelFileUpload;
            bool deleteFileAfterUpload = Utils.SiteConfigData.DeleteExcelFileAfterUpload;

            HttpPostedFileBase file = Request.Files["UploadedFile"];

            //StringBuilder sb = new StringBuilder();
            List<ErrorObject> errorList = new List<ErrorObject>();

            if (String.IsNullOrEmpty(tableName) || file == null || file.ContentLength == 0)
            {
                errorList.Add(new ErrorObject()
                {
                    MessageType = "Error",
                    Description = "Please upload Excel file with correct type name selected!"
                });

                ViewBag.ErrorList = errorList;
                return View("Index");
            }

            System.Data.DataTable dt = null;
            string savedFileName = NewXlsxFileName(Utils.SiteConfigData.SiteName ?? "", tableName);
            int errorCount = 0;
            try
            {
                if (detailLogging)
                {
                    Business.LogError(nameof(ExcelController), $"{tableName} : {file.FileName} : {savedFileName} : {file.ContentLength}");
                }

                file.SaveAs(savedFileName);

                ICollection<TableSchema> schemaInfo = Business.GetTableSchema(tableName);

                // Create DataTable based on schema
                dt = CreateDataTable(tableName, schemaInfo);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                errorCount = ValidateWithOpenXml(tableName, savedFileName, schemaInfo, errorList, dt);
                sw.Stop();

                errorList.Add(new ErrorObject()
                {
                    MessageType = "Info",
                    Description = $"Parsing the input file took {sw.ElapsedMilliseconds / 1000} seconds"
                });
                
                if (errorCount == 0)
                {
                    errorList.Add(new ErrorObject()
                    {
                        MessageType = "Info",
                        Description = "No errors"
                    });

                    // save datatable in sql server
                    ExcelHelper.BulkCopy(dt, tableName);

                    // save record to indicate progress, in db
                    // Here we want to save the upload type that is displayed to the user
                    // and not the table name;
                    var optionsList = ViewBag.OptionsList as ICollection<CodeTableEx>;
                    var optionRec = optionsList?.Where(x => x.Code == tableName).FirstOrDefault();
                    DomainEntities.ExcelUploadStatus eus = new DomainEntities.ExcelUploadStatus()
                    {
                        TenantId = Utils.SiteConfigData.TenantId,
                        UploadType = optionRec?.CodeName ?? "",
                        UploadTable = tableName,
                        UploadFileName = savedFileName,
                        IsCompleteRefresh = IsCompleteRefresh,
                        RecordCount = dt.Rows.Count,
                        RequestedBy = CurrentUserStaffCode,
                        IsPosted = false
                    };

                    Business.CreateExcelUploadStatus(eus);

                    // refresh excel upload status in view bag
                    PutExcelUploadStatusInViewBag();
                }
                else
                {
                    errorList.Add(new ErrorObject()
                    {
                        MessageType = "Warning",
                        Description = $"File not uploaded due to {errorCount} error(s)"
                    });
                }

                ViewBag.ErrorList = errorList;
            }
            catch (Exception ex)
            {
                //ViewBag.Message = $"An error occured while uploading data file. {ex.ToString()}";
                errorList.Add(new ErrorObject()
                {
                    MessageType = "Error",
                    Description = $"{ex.ToString()}"
                });
                
                ViewBag.ErrorList = errorList;
                Business.LogError(nameof(ExcelController), ex.ToString());
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
            }

            return View("Index");
        }

        private int ValidateWithOpenXml(string tableName,
                string fileName,
                ICollection<TableSchema> schemaInfo,
                List<ErrorObject> errorList,
                System.Data.DataTable dt)
        {
            int expectedColumnCount = schemaInfo.Count;

            int errorCount = 0;

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

                // validate the number of sheets
                if (workbookPart.WorksheetParts.Count() != 1)
                {
                    errorList.Add(new ErrorObject()
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
                    errorList.Add(new ErrorObject()
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
                    errorList.Add(new ErrorObject()
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
                        errorList.Add(new ErrorObject()
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
                    errorList.Add(new ErrorObject()
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
                for (int r = rowToStartAt; r <= totalRows && errorCount < 50; r++)
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
                            errorList.Add(new ErrorObject()
                            {
                                MessageType = "Error",
                                Description = $"Schema info for column {Helper.GetExcelColumnLabel(c)} not found."
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

                errorList.Add(new ErrorObject()
                {
                    MessageType = "Info",
                    Description = $"Process has read {rowsRead} rows of data."
                });

                return errorCount;
            }
        }

        private DataTable CreateDataTable(string tableName, ICollection<TableSchema> schemaInfo)
        {
            System.Data.DataTable dt = new System.Data.DataTable(tableName);

            foreach(var ts in schemaInfo.OrderBy(x=> x.Position))
            {
                Type t;
                if (ts.IsStringDataType)
                {
                    t = typeof(String);
                }
                else if( ts.IsDecimalDataType)
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

        private void PutDataInViewBag()
        {
            // get tenant name
            var tenantRecord = Business.GetTenantRecord(Utils.SiteConfigData.TenantId);
            ViewBag.TenantName = tenantRecord?.Name ?? "";

            // default datatype is empty
            ViewBag.DataType = "";

            ICollection<CodeTableEx> availableExcelUploads = Business.GetCodeTable("ExcelUpload");
            ViewBag.OptionsList = availableExcelUploads;

            Dictionary<string, ICollection<TableSchema>> schemaDictionary = new Dictionary<string, ICollection<TableSchema>>();
            foreach(var aeu in availableExcelUploads)
            {
                // retrieve schema for each table
                schemaDictionary.Add(aeu.Code, Business.GetTableSchema(aeu.Code));
            }
            ViewBag.SchemaDictionary = schemaDictionary;

            ViewBag.AllowCSVUpload = Utils.SiteConfigData.AllowCSVUpload;
            ViewBag.AllowExcelUpload = Utils.SiteConfigData.AllowExcelUpload;

            PutExcelUploadStatusInViewBag();

            ViewBag.IsSetupSuperAdmin = IsSetupSuperAdmin;
        }

        private void PutExcelUploadStatusInViewBag()
        {
            // retrieve status of last uploads
            ICollection<DomainEntities.ExcelUploadStatus> uploadStatuses = Business.GetExcelUploadStatus(Utils.SiteConfigData.TenantId);
            ViewBag.ExcelUploadStatus = uploadStatuses.Select(x => new ExcelUploadStatus()
            {
                Id = x.Id,
                UploadType = x.UploadType,
                IsCompleteRefresh = x.IsCompleteRefresh,
                RecordCount = x.RecordCount,
                RequestedBy = x.RequestedBy,
                UploadFileName = x.UploadFileName,
                RequestTimestamp = Helper.ConvertUtcTimeToIst(x.RequestTimestamp),
                PostingTimestamp = Helper.ConvertUtcTimeToIst(x.PostingTimestamp),
                IsPosted = x.IsPosted,

                LocalFileName = x.LocalFileName,
                IsParsed = x.IsParsed,
                ErrorCount = x.ErrorCount,
                IsLocked = x.IsLocked,
                LockTimestamp = Helper.ConvertUtcTimeToIst(x.LockTimestamp)

            }).ToList();
        }

        private int ValidateCellValueWithSchema(int rowPosition, int cellPosition,
            string cv, TableSchema cellSchema, List<ErrorObject> errorList, System.Data.DataRow dr)
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
                    errorList.Add(new ErrorObject()
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
                    errorList.Add(new ErrorObject()
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

                    errorList.Add(new ErrorObject()
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
                    errorList.Add(new ErrorObject()
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
                double n = 0;
                string text = cv;
                if (String.IsNullOrWhiteSpace(text))
                {
                    text = "0";
                }
                bool status = double.TryParse(text, out n);
                if (status == false)
                {
                    errorList.Add(new ErrorObject()
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
            }
            else
            {
                errorList.Add(new ErrorObject()
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

        private string GetCellReference(int rowPosition, int cellPosition) => 
            $"{Helper.GetExcelColumnLabel(cellPosition)}{rowPosition}";

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
    }
}