using Common.Downloads;
using Domain.Document;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Reflection;

namespace Common.FileUpload
{
    public class FileUpload
    {
        List<Documents> documentFilemodel = new List<Documents>();
        List<Documents> documents = new List<Documents>();
        public string? EntityPath { get; set; }

        /// <summary>
        /// Author:Swetha M Date:16/05/2022
        /// Purpose: Get the list of files  and stores the filefrom the view
        /// </summary>
        /// <returns>List of files details  to Documents</returns>
        public async Task<List<Documents>> UploadFiles(List<IFormFile> files, string entityPath, string wwwPath, List<string>? DocumentName)
        {
            EntityPath = entityPath;
            foreach (var file in files)
            {
                var basePath = GetFilePath(wwwPath);
                bool basePathExists = Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_".AddTimeStamp();
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);

                if (!File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    if (DocumentName != null && DocumentName.Count != 0)
                    {
                        foreach (var name in DocumentName)
                        {
                            var filename = name.Split(",");


                            if (file.FileName.Contains(filename[0]))
                            {
                                documentFilemodel = new List<Documents>() {

                         new Documents
                        {

                            CreatedOn = DateTime.UtcNow,
                            FileType = file.ContentType,
                            Extension = extension,
                            Name = fileName,
                            FilePath = filePath,
                            Description = filename[1],

                        }
                    };
                            }


                        }
                    }
                    else
                    {
                        documentFilemodel = new List<Documents>() {


                            new Documents
                            {

                                CreatedOn = DateTime.UtcNow,
                                FileType = file.ContentType,
                                Extension = extension,
                                Name = fileName,
                                FilePath = filePath,

                            }
                        };
                    }

                }


                documents.AddRange(documentFilemodel);

            }
            return documents;
        }

        public string GetFilePath(string wwwPath)
        {
            var FolderTimeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            var filePath = Path.Combine(wwwPath + "/" + EntityPath + "/" + FolderTimeStamp + "/");
            return filePath;

        }


    }

}