using BusinessLayer;
using CRMUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpicCrmWebApi
{
    [AuthorizeDevAttribute]
    public class TestBedController : Controller
    {
        // GET: TestBed
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DataSync()
        {
            return View();
        }

        public ActionResult Download()
        {
            return View();
        }

        public ActionResult ImageDownload()
        {
            return View();
        }

        public ActionResult DownloadMini()
        {
            return View();
        }


        public ActionResult EORPData()
        {
            return View();
        }

        public ActionResult EORPMonthWiseData()
        {
            return View();
        }

        public ActionResult TOPItems()
        {
            return View();
        }

        public ActionResult LogData()
        {
            return View();
        }

        public ActionResult EmployeeStats()
        {
            return View();
        }

        public ActionResult PreSignedLinkFromS3(string fileName = "")
        {
            ViewBag.FileName = fileName;
            return View();
        }

        public ActionResult DistanceCalculator()
        {
            return View();
        }

        public ActionResult ListS3Buckets()
        {
            ICollection<string> s3Buckets = Business.S3BucketNames();
            // for each bucket count the number of entries
            Dictionary<string, int> resultList = new Dictionary<string, int>();
            s3Buckets.Select(x =>
            {
                resultList.Add(x, 0);
                //resultList.Add(x, Business.S3BucketEntries(x).Count);
                return 1;

            }).Count();

            return View(resultList);
        }

        [AjaxOnly]
        [HttpGet]
        public JsonResult GetDistanceCalculation(decimal BeginLatitude, decimal BeginLongitude, decimal EndLatitude,decimal EndLongitude)
        {
            decimal distance = Business.GetLinearDistance(BeginLatitude, BeginLongitude, EndLatitude, EndLongitude);

            return Json(distance, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public EmptyResult GetImageData(string fileName)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.GetImageBytes(fileName);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult S3Link(string fileName)
        {
            ViewBag.FileName = fileName;
            ViewBag.Link = "";
            ViewBag.Status = false;

            // ensure file exists
            if (string.IsNullOrEmpty(fileName))
            {
                ViewBag.Message = $"Invalid file name.";
                return PartialView("_S3Link");
            }

            // ensure file is readable
            FileInfo fi = new FileInfo(fileName);

            // ensure file exist
            if (fi.Exists == false)
            {
                ViewBag.Message = $"File {fileName} does not exist.";
                return PartialView("_S3Link");
            }

            try
            {
                Tuple<bool, string, string> response = Business.GetPreSignedLink(
                    fileName, TimeSpan.FromMinutes(Utils.DownloadLinkTimeoutInMinutes));

                ViewBag.Status = response.Item1;

                if (response.Item1 == false)
                {
                    ViewBag.Message = response.Item2;
                }
                else
                {
                    ViewBag.Link = response.Item3;
                    ViewBag.Message = "";
                }

                return PartialView("_S3Link");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();
                return PartialView("_S3Link");
            }
        }
    }
}