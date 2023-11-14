using Common.Downloads;
using Domain.Document;
using Domain.Profile;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.FileUpload
{
    public class ImageUpload
    {
        List<HeaderProfileDetails> ImageFilemodel = new List<HeaderProfileDetails>();
        List<HeaderProfileDetails> Images = new List<HeaderProfileDetails>();
        public string? EntityPath { get; set; }

        public async Task<List<HeaderProfileDetails>> UploadImage(List<IFormFile> files, string entityPath, string wwwPath, List<string>? DocumentName)
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
                                ImageFilemodel = new List<HeaderProfileDetails>() {

                         new HeaderProfileDetails
                        {

                            CreatedOn = DateTime.UtcNow,
                            FileType = file.ContentType,
                            Extension = extension,
                            FileName = fileName,


                             ImagePath = filePath,

                        }
                    };
                            }


                        }
                    }
                    else
                    {
                        ImageFilemodel = new List<HeaderProfileDetails>() {


                            new HeaderProfileDetails
                            {

                                CreatedOn = DateTime.UtcNow,
                                FileType = file.ContentType,
                                Extension = extension,
                                FileName = fileName,
                                ImagePath = filePath,

                            }
                        };
                    }

                }


                Images.AddRange(ImageFilemodel);

            }
            return Images;
        }
        public string GetFilePath(string wwwPath)
        {
            var FolderTimeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            var filePath = Path.Combine(wwwPath + "/" + EntityPath + "/" + FolderTimeStamp + "/");
            return filePath;

        }



    }
}
