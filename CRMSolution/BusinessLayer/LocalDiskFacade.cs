using Amazon.S3;
using Amazon.S3.Model;
using CRMUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class LocalDiskFacade
    {
        public static byte[] GetImageBytes(string folderName, string imageFileName)
        {
            // first form complete image file name
            string completeFileName = System.IO.Path.Combine(folderName, imageFileName);
            return GetImageBytes(completeFileName);
        }

        public static byte[] GetImageBytes(string filePathAndName)
        {
            if (File.Exists(filePathAndName))
            {
                using (MemoryStream tmpStream = new MemoryStream())
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromFile(filePathAndName))
                    {
                        img.Save(tmpStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    return tmpStream.ToArray();
                }
            }

            // we can return a default image
            return null;
        }

        public static MemoryStream GetImageMemoryStream(string filePathAndName)
        {
            // caller is responsible to dispose memory stream;
            if (File.Exists(filePathAndName))
            {
                MemoryStream tmpStream = new MemoryStream();
                using (System.Drawing.Image img = System.Drawing.Image.FromFile(filePathAndName))
                {
                    img.Save(tmpStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                return tmpStream;
            }

            return null;
        }

        public static Stream GetFileStream(string filePathAndName)
        {
            // caller is responsible to dispose memory stream;
            if (File.Exists(filePathAndName))
            {
                FileStream fs = File.OpenRead(filePathAndName);
                return fs;
            }

            return null;
        }

        public static string SaveImageData(byte[] binaryData, string fileNamePrefix = "")
        {
            string imagesFolder = Utils.SiteConfigData.ImagesFolder;
            if (string.IsNullOrEmpty(imagesFolder))
            {
                throw new InvalidOperationException("Images folder not specified in config");
            }

            string imageFileName = $"{fileNamePrefix}{Guid.NewGuid().ToString()}.jpg";

            string completeOutputFileName = System.IO.Path.Combine(imagesFolder, imageFileName);
            System.IO.File.WriteAllBytes(completeOutputFileName, binaryData);

            return imageFileName;
        }
        
        public static bool SaveImageData(byte[] binaryData, string id, string saveFileType)
        {
            string imagesFolder = Utils.SiteConfigData.ImagesFolder;
            if (string.IsNullOrEmpty(imagesFolder))
            {
                throw new InvalidOperationException("Images folder not specified in config");
            }

            string imageFileName = $"{id}.{saveFileType}";
            string completeOutputFileName = System.IO.Path.Combine(imagesFolder, imageFileName);
            if (File.Exists(completeOutputFileName))
            {
                Business.LogError(nameof(SaveImageData), $"Image file {completeOutputFileName} already exist.");
                return false;
            }
            else
            {
                try
                {
                    System.IO.File.WriteAllBytes(completeOutputFileName, binaryData);
                    return true;
                }
                catch(Exception ex)
                {
                    Business.LogError(nameof(SaveImageData), ex);
                    return false;
                }
            }
        }

        //public static byte[] GetImageBytes(string imageFileName)
        //{
        //    // first form complete image file name
        //    string imagesFolder = Utils.SiteConfigData.ImagesFolder;
        //    string completeFileName = System.IO.Path.Combine(imagesFolder, imageFileName);

        //    if (File.Exists(completeFileName))
        //    {
        //        MemoryStream tmpStream = new MemoryStream();
        //        System.Drawing.Image img = System.Drawing.Image.FromFile(completeFileName);
        //        img.Save(tmpStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        return tmpStream.ToArray();
        //    }

        //    // we can return a default image
        //    return null;
        //}
    }
}
