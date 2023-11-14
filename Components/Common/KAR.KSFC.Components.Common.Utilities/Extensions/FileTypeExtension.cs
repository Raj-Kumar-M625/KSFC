using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Utilities.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Utilities.Extensions
{
    public static class FileTypeExtension
    {
        public static bool ValidateFileSize(int length, FileTypesEnum fileTypes, IConfiguration _configure)
        {
            if (fileTypes == FileTypesEnum.Pdf)
            {
                int size = Convert.ToInt32(_configure["FileSize:PDF"]);
                if (length / 1024f <= size)
                    return true;
            }
            else if (fileTypes == FileTypesEnum.Jpeg || fileTypes == FileTypesEnum.Jpg)
            {
                int size = Convert.ToInt32(_configure["FileSize:JPG"]);
                if (length / 1024f <= size)
                    return true;
            }
            return false;
        }

        public static int GetFileSize(FileTypesEnum fileTypes, IConfiguration _configure)
        {
            if (fileTypes == FileTypesEnum.Pdf)
            {
                return Convert.ToInt32(_configure["FileSize:PDF"]);
                
            }
            else if (fileTypes == FileTypesEnum.Jpeg || fileTypes == FileTypesEnum.Jpg)
            {
                return Convert.ToInt32(_configure["FileSize:JPG"]);
                
            }
            return 0;
        }

        /// <summary>
        /// Get File Path
        /// </summary>
        /// <param name="fileTypes"></param>
        /// <param name="_configure"></param>
        /// <returns></returns>
        public static string GetFilePath(DocumentTypeEnum documentType, IConfiguration _configure)
        {
            if (documentType == DocumentTypeEnum.GeneralDocument1 || documentType == DocumentTypeEnum.GeneralDocument2)
            {
                string path = $"{_configure["FilePath:BasePath"]}{_configure["FilePath:GeneralDocument"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }
            if (documentType == DocumentTypeEnum.TechnicalDocument1 || documentType == DocumentTypeEnum.TechnicalDocument2)
            {
                string path = $"{_configure["FilePath:BasePath"]}{_configure["FilePath:TechnicalDocument"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }
            if (documentType == DocumentTypeEnum.FinancialDocument1 || documentType == DocumentTypeEnum.FinancialDocument2)
            {
                string path = $"{_configure["FilePath:BasePath"]}{_configure["FilePath:FinancialDocument"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }
            if (documentType == DocumentTypeEnum.LegalDocument1 || documentType == DocumentTypeEnum.LegalDocument2)
            {
                string path = $"{_configure["FilePath:BasePath"]}{_configure["FilePath:LegalDocument"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }
            if (documentType == DocumentTypeEnum.AuditClearance)
            {
                string path = $"{_configure["FilePath:BasePath"]}{_configure["FilePath:AuditClearance"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }
            if (documentType == DocumentTypeEnum.DisbursementCondition)
            {
                string path = $"{_configure["FilePath:BasePath"]}{_configure["FilePath:DisbursementCondition"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }
            if (documentType == DocumentTypeEnum.InspectionOfUnit)
            {
                string path = $"{_configure["FilePath:BasePath"]}{_configure["FilePath:InspectionOfUnit"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }
            if (documentType == DocumentTypeEnum.ChangeOfUnit)
            {
                string path = $"{_configure["FilePath:BasePath"]}{_configure["FilePath:ChangeOfUnit"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }


            throw new DocumentExceptions("Invalid Document Type");

        }


        /// <summary>
        /// Get file name from document type
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="enqId"></param>
        /// <param name="docId"></param>
        /// <returns></returns>

        public static string GetFileName(DocumentTypeEnum documentType, int enqId)
        {
            switch (documentType)
            {
                case DocumentTypeEnum.GeneralDocument1: return $"{ enqId }_GD1_{DateTime.Now.ToString("ddMMMyyyy_mm_ss")}";
                case DocumentTypeEnum.GeneralDocument2: return $"{ enqId }_GD2_{DateTime.Now.ToString("ddMMMyyyy_mm_ss")}";
                case DocumentTypeEnum.TechnicalDocument1: return $"{ enqId }_TD1_{DateTime.Now.ToString("ddMMMyyyy_mm_ss")}";
                case DocumentTypeEnum.TechnicalDocument2: return $"{ enqId }_TD2_{DateTime.Now.ToString("ddMMMyyyy_mm_ss")}";
                case DocumentTypeEnum.FinancialDocument1: return $"{ enqId }_FD1_{DateTime.Now.ToString("ddMMMyyyy_mm_ss")}";
                case DocumentTypeEnum.FinancialDocument2: return $"{ enqId }_FD2_{DateTime.Now.ToString("ddMMMyyyy_mm_ss")}";
                case DocumentTypeEnum.LegalDocument1: return $"{ enqId }_LD1_{DateTime.Now.ToString("ddMMMyyyy_mm_ss")}";
                case DocumentTypeEnum.LegalDocument2: return $"{ enqId }_LD2_{DateTime.Now.ToString("ddMMMyyyy_mm_ss")}";
                case DocumentTypeEnum.AuditClearance: return $"{ enqId }_AC_{DateTime.Now.ToString("ddMMMyyyy_mm_ss")}";
                case DocumentTypeEnum.DisbursementCondition: return $"{ enqId }_DC_{DateTime.Now.ToString("ddMMMyyyy_mm_ss")}";
                case DocumentTypeEnum.InspectionOfUnit: return $"{ enqId }_DC_{DateTime.Now.ToString("ddMMMyyyy_mm_ss")}";
                case DocumentTypeEnum.ChangeOfUnit: return $"{ enqId }_CU_{DateTime.Now.ToString("ddMMMyyyy_mm_ss")}";
            }
            return String.Empty;
        }

        /// <summary>
        /// Get Archive File Path
        /// </summary>
        /// <param name="fileTypes"></param>
        /// <param name="_configure"></param>
        /// <returns></returns>
        public static string GetArchiveFilePath(DocumentTypeEnum documentType, IConfiguration _configure)
        {
            if (documentType == DocumentTypeEnum.GeneralDocument1 || documentType == DocumentTypeEnum.GeneralDocument2)
            {
                string path = $"{_configure["FilePath:ArchivePath"]}{_configure["FilePath:GeneralDocument"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }
            if (documentType == DocumentTypeEnum.TechnicalDocument1 || documentType == DocumentTypeEnum.TechnicalDocument2)
            {
                string path = $"{_configure["FilePath:ArchivePath"]}{_configure["FilePath:TechnicalDocument"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }
            if (documentType == DocumentTypeEnum.FinancialDocument1 || documentType == DocumentTypeEnum.FinancialDocument2)
            {
                string path = $"{_configure["FilePath:ArchivePath"]}{_configure["FilePath:FinancialDocument"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            } 
            if (documentType == DocumentTypeEnum.LegalDocument1 || documentType == DocumentTypeEnum.LegalDocument2)
            {
                string path = $"{_configure["FilePath:ArchivePath"]}{_configure["FilePath:LegalDocument"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }
            if (documentType == DocumentTypeEnum.AuditClearance)
            {
                string path = $"{_configure["FilePath:BasePath"]}{_configure["FilePath:AuditClearance"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }
            if (documentType == DocumentTypeEnum.DisbursementCondition)
            {
                string path = $"{_configure["FilePath:BasePath"]}{_configure["FilePath:DisbursementCondition"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }

            if (documentType == DocumentTypeEnum.InspectionOfUnit)
            {
                string path = $"{_configure["FilePath:BasePath"]}{_configure["FilePath:InspectionOfUnit"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }

            if (documentType == DocumentTypeEnum.ChangeOfUnit)
            {
                string path = $"{_configure["FilePath:BasePath"]}{_configure["FilePath:ChangeOfUnit"]}";
                if (!string.IsNullOrEmpty(path))
                    return path;

                throw new DocumentExceptions("File Path not configured");
            }

            throw new DocumentExceptions("Invalid Document Type");

        }

        /// <summary>
        /// Convert fileto byte Array
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] FileToByteArray(this IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
