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
    public class ParseUnKnown : UploadFileParser
    {
        protected override int Validate(string tableName,
                string fileName,
                ICollection<TableSchema> schemaInfo,
                List<ExcelUploadError> errorList,
                System.Data.DataTable dt,
                int maxErrors)
        {
            errorList.Add(new ExcelUploadError()
            {
                MessageType = "Error",
                Description = $"File Type is not supported.",
                ExpectedValue = "",
                ActualValue = ""
            });

            return 1;
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
            throw new NotImplementedException();
        }
    }
}
