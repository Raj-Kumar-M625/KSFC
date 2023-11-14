using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace EpicCrmWebApi
{
    [Authorize(Roles = "Admin")]
    public class PDFController : BaseDashboardController
    {

        // Rajesh V - 08/10/2021 - Generate PDF file for the selected Agreements
        [CheckRightsAuthorize(Feature = FeatureEnum.FarmerSummaryReport)]
        public ActionResult GeneratePdf(long AgreementId)
        {
            try
            {
                IEnumerable<FarmerSummaryReportData> farmerData = Business.GetFarmerSummarData(AgreementId);
                IEnumerable<FarmerSummaryReportModelData> farmerDetails = farmerData.Select(x => new FarmerSummaryReportModelData()
                {
                    AgreementId = x.AgreementId,
                    EntityName = x.EntityName,
                    FarmerId = x.FarmerId,
                    UniqueId = x.UniqueId,
                    AgreementNumber = x.AgreementNumber,
                    HQName = x.HQName,
                    TerritoryName = x.TerritoryName,
                    LandInSize = x.LandInSize,
                    RatePerKg = x.RatePerKg
                }).ToList();

                IEnumerable<FarmerSummaryReportData> farmerIssue = Business.GetFarmerIssueData(AgreementId);
                IEnumerable<FarmerSummaryReportModelData> farmerIssueData = farmerIssue.Select(x => new FarmerSummaryReportModelData()
                {
                    UniqueId = x.UniqueId,
                    Issuedate = x.Issuedate,
                    IssueSlipNumber = x.IssueSlipNumber,
                    IssueType = x.IssueType,
                    IssueInput = x.IssueInput,
                    IssueQuantity = x.IssueQuantity,
                    Uom = x.Uom,
                    PricePerUom = x.PricePerUom,
                    InputAmount = x.IssueQuantity * x.PricePerUom
                }).ToList();

                IEnumerable<FarmerSummaryReportData> farmerDws = Business.GetFarmerDwsData(AgreementId);
                IEnumerable<FarmerSummaryReportModelData> farmerDwsData = farmerDws.Select(x => new FarmerSummaryReportModelData()
                {
                    STRNumber = x.STRNumber,
                    DWSNumber = x.DWSNumber,
                    PurchaseDate = x.PurchaseDate,
                    DWSQuantity = x.DWSQuantity,
                    PurchaseAmount = x.PurchaseAmount,
                    PaymentReference = x.PaymentReference,
                    DWSDeduction = x.DWSDeduction,
                    Netpayable = x.Netpayable,
                    PayoutDate = x.PayoutDate,
                    PaymentAmount = x.PaymentAmount
                }).ToList();

                //Gagana on 18-11-21 Purpose: Advance request details and Agreement Bonus added
                IEnumerable<FarmerSummaryReportData> farmerAdvance = Business.GetFarmerAdvReqData(AgreementId);
                IEnumerable<FarmerSummaryReportModelData> farmerAdvanceData = farmerAdvance.Select(x => new FarmerSummaryReportModelData()
                {
                    AdvanceRequestDate = x.AdvanceRequestDate,
                    AmountApproved = x.AmountApproved
                }).ToList();

                IEnumerable<FarmerSummaryReportData> farmerAgreementBonus = Business.GetFarmerAgrBonusData(AgreementId);
                IEnumerable<FarmerSummaryReportModelData> farmerAgreementBonusData = farmerAgreementBonus.Select(x => new FarmerSummaryReportModelData()
                {
                    TotalNetQuantity = x.TotalNetQuantity,
                    BonusRate = x.BonusRate,
                    BonusPayableAmount = x.BonusPayableAmount,
                    BonusAmountPaid = x.BonusAmountPaid,
                    PaymentDate = x.PaymentDate,
                    BonusPaymentReference =x.BonusPaymentReference

                }).ToList();

                ViewBag.FarmerDetails = farmerDetails;
                ViewBag.FarmerIssueDetails = farmerIssueData;
                ViewBag.FarmerAdvanceDetails = farmerAdvanceData;
                ViewBag.FarmerBonusDetails = farmerAgreementBonusData;
                var HTMLViewStr = RenderViewToString(ControllerContext, "~/Views/FarmerSummaryReport.cshtml", farmerDwsData);

                using (MemoryStream stream = new MemoryStream())
                {
                    StringReader sr = new StringReader(HTMLViewStr);
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();

                    return File(stream.ToArray(), "application/pdf", farmerData.FirstOrDefault().EntityName + "_Settlement_Report.pdf");
                }
            }
            catch (Exception ex)
            {
                Business.LogError($"{nameof(GeneratePdf)}", ex);
                return PartialView("_Error");
            }
        }

        public static string RenderViewToString(ControllerContext context, string viewPath, object model = null, bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }
    }
}
