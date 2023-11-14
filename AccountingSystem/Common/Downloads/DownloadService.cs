using ArrayToPdf;
using ClosedXML.Excel;
using Microsoft.AspNetCore.StaticFiles;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Common.Downloads
{
    /// <summary>
    /// Purpose: Bind the List Value to DataTable
    /// Author: Sandeep; Date: 16/05/2022
    /// ModifiedBy:
    /// ModifiedPurpose:
    /// </summary>
    public class DownloadService<T> : IDownloadService
    {
        private string fileName = String.Empty;
        //private List<T> listItems { get; set; }
        private List<T> listItems = new List<T>();
        public string FileName { get; set; }
        public List<T> ListItems { get; set; }


        /// <summary>
        /// Get the files
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="_items"></param>
        /// <returns></returns>
        public byte[] GetFile()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                // Handle the case when fileName is empty or null
                return null;
            }

            FileTypes inputExt =  (FileTypes) Enum.Parse(typeof(FileTypes), Path.GetExtension(FileName).ToLower().Substring(1));
            switch (inputExt)
            {
                case FileTypes.pdf:
                    return GetPdfFile();
                case FileTypes.xlsx:
                    return GetExcelFile();
                default:
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_items"></param>
        /// <returns></returns>
        public  byte[] GetExcelFile()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(LineItemsToDataTable(listItems));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);                    
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_items"></param>
        /// <returns></returns>
        public  byte[] GetPdfFile()
        {            
            return listItems.ToPdf();
        }

        /// <summary>
        /// Returning the ContentType
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public  string ContentType()
        {                        
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(FileName, out contentType);
            return contentType;      
        }

        /// <summary>
        /// Convert Grid Line Items to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public  DataTable LineItemsToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }           
            return dataTable;         
        }
    }
}
   
