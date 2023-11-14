using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicUpload
{
    public static class Process
    {
        public static void ProcessFiles()
        {
            // 0. Ensure Target Folder Exist
            string backupLocation = Helper.GetBackupLocation;
            Helper.EnsureFolderExist(backupLocation);

            // 1. Get source file list
            string sourceFolder = Helper.GetSourceLocation;
            if (Helper.IsFolderExist(sourceFolder) == false)
            {
                // log a message in event viewer
                Helper.LogError($"Source folder {sourceFolder} does not exist");
                return;
            }

            string[] fileList = Helper.GetFileList(sourceFolder);

            if ((fileList?.Length ?? 0) == 0)
            {
                Helper.LogInfo($"Source folder {sourceFolder} does not have any files to process.");
                return;
            }

            // 2. Iterate over files
            foreach(var f in fileList)
            {
                string justFileName = Helper.ExtractFileNameFromFullPath(f);
                string tableName = Helper.GetTableNameForFileName(f);
                string IsCompleteRefresh = Helper.GetIsCompleteRefresh;

                if (string.IsNullOrEmpty(tableName))
                {
                    // table is not defined.
                    Helper.LogError($"File {f} name/type is not recognized.  File is not processed.");
                    continue;
                }

                FileUploadRequest fur = new FileUploadRequest()
                {
                    FileName = justFileName,
                    TableName = tableName,
                    IsCompleteRefresh = Boolean.Parse(IsCompleteRefresh),
                    FileContent = Helper.ReadFileBytes(f)
                };

                Helper.LogInfo($"Trying to upload file {fur.FileName} ({fur.FileContent.Length} bytes) on {Helper.GetAlertAPI}");

                // 2a. Make API Call
                FileUploadResponse response = Helper.UploadFile(fur).Result;

                // 2b. On Success, Move file
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Helper.LogInfo($"File {f} has been successfully uploaded. UploadId: {response.RequestId}");
                    Helper.MoveFile(f, backupLocation);
                }
                else
                {
                    Helper.LogError($"Could not upload file {f}, {response.Content}");
                }
            }
        }
    }
}
