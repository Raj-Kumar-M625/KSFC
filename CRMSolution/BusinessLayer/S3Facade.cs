using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class S3Facade
    {
        public static bool EnsureBucketExist(string bucketName, IAmazonS3 s3Client)
        {
            try
            {
                if (s3Client.DoesS3BucketExist(bucketName) == false)
                {
                    s3Client.EnsureBucketExists(bucketName);
                }
                return true;
            }
            catch (Exception ex)
            {
                Business.LogError($"{nameof(EnsureBucketExist)}", ex);
                return false;
            }
        }

        public static ICollection<string> BucketEntries(string bucketName, IAmazonS3 s3Client)
        {
            try
            {
                //ListObjectsResponse response = s3Client.ListObjects(bucketName);
                var currentObjectList = s3Client.GetAllObjectKeys(bucketName, "", null);
                return currentObjectList.Select(x => x).ToList();
            }
            catch (Exception ex)
            {
                Business.LogError($"{nameof(BucketEntries)}", ex);
                return null;
            }
        }

        public static bool SaveFileInS3(string filePathAndName, string bucketName, 
            IAmazonS3 s3Client, Func<string, Stream> funcToGetStream)
        {
            S3Facade.EnsureBucketExist(bucketName, s3Client);

            // take out just the file name
            string[] filePaths = filePathAndName.Split(new char[] { '/', '\\' });
            string fileName = filePaths[filePaths.Length - 1];

            // if file already exist in S3 - return
            var currentObjectList = s3Client.GetAllObjectKeys(bucketName, fileName, null);
            if (currentObjectList != null && currentObjectList.Count > 0)
            {
                Business.LogError($"{nameof(SaveFileInS3)}", $"file {filePathAndName} already exist in S3");
                return true;
            }

            //MemoryStream ms = LocalDiskFacade.GetImageMemoryStream(filePathAndName);
            Stream ms = funcToGetStream(filePathAndName);

            if (ms == null)
            {
                Business.LogError($"{nameof(SaveFileInS3)}", $"Not able to read {filePathAndName} ");
                return false;
            }
            
            // Upload file inbucket
            var response = s3Client.PutObject(new PutObjectRequest()
            {
                BucketName = bucketName,
                CannedACL = S3CannedACL.Private,
                Key = fileName,
                InputStream = ms
            });

            ms.Dispose();

            return (response.HttpStatusCode == HttpStatusCode.OK);
        }

        public static byte[] GetS3ImageBytes(string imageFileName, string bucketName, IAmazonS3 s3Client)
        {
            // first check that the key exist
            try
            {
                var currentObjectList = s3Client.GetAllObjectKeys(bucketName, imageFileName, null);
                if (currentObjectList != null && currentObjectList.Count == 0)
                {
                    Business.LogError($"{nameof(GetS3ImageBytes)}", $"Image {imageFileName} does not exist in S3");
                    return null;
                }

                var response = s3Client.GetObject(new GetObjectRequest()
                {
                    BucketName = bucketName,
                    Key = imageFileName
                });

                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (var rs = response.ResponseStream)
                        {
                            rs.CopyTo(ms);
                        }
                        return ms.ToArray();
                    }
                }

                Business.LogError($"{nameof(GetS3ImageBytes)}", $"Could not retrieve image {imageFileName} from S3");

                return null;
            }
            catch(Exception ex)
            {
                Business.LogError($"{nameof(GetS3ImageBytes)}", ex);
                return null;
            }
        }

        public static Tuple<bool, string, string> GetPresignedUrl(string filePathAndName, string bucketName, IAmazonS3 s3Client, TimeSpan validity)
        {
            S3Facade.EnsureBucketExist(bucketName, s3Client);

            // take out just the file name
            string[] filePaths = filePathAndName.Split(new char[] { '/', '\\' });
            string fileName = filePaths[filePaths.Length - 1];

            // file should already exist in S3 - return
            var currentObjectList = s3Client.GetAllObjectKeys(bucketName, fileName, null);
            if ((currentObjectList?.Count ?? 0) == 0)
            {
                Business.LogError($"{nameof(GetPresignedUrl)}", $"file {filePathAndName} does not exist in S3");
                return new Tuple<bool, string, string>(false, "File does not exist in S3 bucket.", "");
            }

            if (currentObjectList.Count > 1)
            {
                Business.LogError($"{nameof(GetPresignedUrl)}", $"file {filePathAndName} has more than one keys in s3");
                return new Tuple<bool, string, string>(false, "There seems to be more than one file in S3 with this key", "");
            }

            string preSignedUrl = s3Client.GetPreSignedURL(new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Expires = DateTime.UtcNow.Add(validity),
                Protocol = Protocol.HTTPS,
                Verb = HttpVerb.GET,
                Key = currentObjectList.First()
            });

            Business.LogError($"{nameof(GetPresignedUrl)}", $"file {filePathAndName} => {preSignedUrl}");

            return new Tuple<bool, string, string>(true, "", preSignedUrl);
        }

        public static ICollection<string> GetAllBucketNames(IAmazonS3 s3Client)
        {
            ListBucketsResponse bucketResponse = s3Client.ListBuckets();
            return bucketResponse.Buckets.Select(x => x.BucketName).OrderBy(x => x).ToList();
        }
    }
}
