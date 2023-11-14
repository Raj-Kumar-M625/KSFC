using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IUnitDetailsService;

using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails.ProductDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class ProductController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;

        public ProductController(ILogger logger, SessionManager sessionManager)
        {

            _logger = logger;
            _sessionManager = sessionManager;
           
        }

        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var AllProductList = _sessionManager.GetProductDetailsList();
               IdmUnitProductsDTO ProductList = AllProductList.FirstOrDefault(x => x.UniqueId == id);

                var allproducttypes = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionchngProductDetails));
               
                ViewBag.ProductTypes = allproducttypes;

                var allproductlist = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionProductDetailsMaster));
                ViewBag.productlist = allproductlist;

                var allindustrylist = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionIndustryDetailsMaster));
                ViewBag.industrylist = allindustrylist;

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.chngprodresultViewPath + Constants.ViewRecord, ProductList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult Create(long AccountNumber, byte OffCd, int LoanSub)
        {
            try
            {

                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);
                ViewBag.LoanAcc = AccountNumber;
                ViewBag.OffcCd = OffCd;
                ViewBag.LoanSub = LoanSub;

               var allproductlist = JsonConvert.DeserializeObject<List<TblProdCdtabDTO>>(HttpContext.Session.GetString("SessionAllProductList"));
                ViewBag.productlist = allproductlist;

                var allindustrylist = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionIndustryDetailsMaster));
                ViewBag.industrylist = allindustrylist; 

                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(Constants.chngprodresultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmUnitProductsDTO product)
        {
            try
            {
                IEnumerable<IdmUnitProductsDTO> activeProductList = new List<IdmUnitProductsDTO>();
                _logger.Information(string.Format(CommonLogHelpers.CreateStartedPost + LogAttribute.productDto,
                  product.LoanAcc, product.IndId, product.ProdId));
                
                if (ModelState.IsValid)
                {
                    List<IdmUnitProductsDTO> productDetails = new();
                    if (_sessionManager.GetProductDetailsList() != null)
                        productDetails = _sessionManager.GetProductDetailsList();                   

                    IdmUnitProductsDTO list = new IdmUnitProductsDTO();
                    list.LoanAcc = product.LoanAcc;
                    var accountNumber = product.LoanAcc;
                    list.TblProdCdtabs= product.TblProdCdtabs;
                    list.LoanAcc = product.LoanAcc;
                    list.LoanSub = product.LoanSub;
                    list.ProdCd = product.ProdCd;
                   list.ProdId = product.ProdId;
                    list.IndId = product.IndId;
                    var allindustrytypes = _sessionManager.GetallIndustrydetailsMaster();
                    var industryType = allindustrytypes.Where(x => x.Value == list.IndId.ToString());
                    list.IndDets = industryType.First().Text;

                    var allformTypes = _sessionManager.GetallProducdetailsMaster();
                    var productType = allformTypes.Where(x => x.Value == list.ProdId.ToString());
                    list.ProdDets = productType.First().Text;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    productDetails.Add(list);
                    _sessionManager.SetProductDetailsList(productDetails);
                    if (productDetails.Count != 0)
                    {
                        activeProductList = productDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    }
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.LoanSub = list.LoanSub;

                    _logger.Information(string.Format(CommonLogHelpers.CreateCompletedPost + LogAttribute.productDto,
                           product.IdmUtproductRowid, product.UtCd, product.ProdId));
                    return Json(new { isValid = true, data = accountNumber, html = Helper.RenderRazorViewToString(this, Constants.chngprodViewPath + Constants.ViewAll, activeProductList) });
                }
                ViewBag.AccountNumber = product.LoanAcc;
                ViewBag.OffcCd = product.OffcCd;
                ViewBag.LoanSub = product.LoanSub;
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.productDto,
                   product.IdmUtproductRowid, product.UtCd, product.ProdId));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.chngprodViewPath + Constants.Create, product) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult Edit(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + id);
                var AllProductList = _sessionManager.GetProductDetailsList();
                IdmUnitProductsDTO ProductList = AllProductList.FirstOrDefault(x => x.UniqueId == id);
                if (ProductList != null)
                {
                    var allproducttypes = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionchngProductDetails));

                    ViewBag.ProductTypes = allproducttypes;

                    var allproductlist = JsonConvert.DeserializeObject<List<TblProdCdtabDTO>>(HttpContext.Session.GetString("SessionAllProductList"));
                    var filteredproduct = allproductlist.Where(x => x.ProdInd == ProductList.IndId);
                    ViewBag.filteredproduct = filteredproduct.Select(x => new { x.Id, x.ProdDets }).ToList();

                    var allindustrylist = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionIndustryDetailsMaster));

                    ViewBag.industrylist = allindustrylist;

                    ViewBag.LoanAcc = ProductList.LoanAcc;
                    ViewBag.OffcCd = ProductList.OffcCd;
                    ViewBag.LoanSub = ProductList.LoanSub;
                }

                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                return View(Constants.chngprodresultViewPath + Constants.editCs, ProductList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmUnitProductsDTO product)
        {
            try
            {

                if (_sessionManager.GetProductDetailsList() != null)
                {
                    List<IdmUnitProductsDTO> productDetails = _sessionManager.GetProductDetailsList();
                    IdmUnitProductsDTO productExist = productDetails.Find(x => x.UniqueId == id);
                    if (productExist != null)
                    {
                        productDetails.Remove(productExist);
                        var list = productExist;
                        list.LoanAcc = product.LoanAcc;
                        list.LoanSub = product.LoanSub;
                        list.OffcCd = product.OffcCd;
                        list.UtCd = product.UtCd;
                        list.ProdCd = product.ProdCd;
                        list.ProdId = product.ProdId;
                        list.IndId = product.IndId;
                        list.UniqueId = product.UniqueId;

                        var allformTypes = _sessionManager.GetallProducdetailsMaster();
                        var productType = allformTypes.Where(x => x.Value == list.ProdId.ToString());
                        list.ProdDets = productType.First().Text;
                        list.IsActive = true;
                        list.IsDeleted = false;
                        list.CreatedDate = product.CreatedDate;
                        if (productExist.IdmUtproductRowid > 0)
                        {
                            list.Action = (int)Constant.Update;
                        }
                        else
                        {
                            list.Action = (int)Constant.Create;

                        }
                        ViewBag.AccountNumber = list.LoanAcc;
                        ViewBag.OffcCd = list.OffcCd;
                        ViewBag.LoanSub = list.LoanSub;
                        productDetails.Add(list);
                        _sessionManager.SetProductDetailsList(productDetails);
                        List<IdmUnitProductsDTO> activeProductList = productDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();

                        return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.chngprodViewPath + Constants.ViewAll, activeProductList) });
                    }
                }
                
                ViewBag.AccountNumber = product.LoanAcc;
                ViewBag.OffcCd = product.OffcCd;
                ViewBag.LoanSub = product.LoanSub;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.chngprodViewPath + Constants.Edit, product) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public IActionResult Delete(string Id)
        {
            try
            {
                IEnumerable<IdmUnitProductsDTO> activeProductList = new List<IdmUnitProductsDTO>();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, Id));
                var productDetailsList = JsonConvert.DeserializeObject<List<IdmUnitProductsDTO>>(HttpContext.Session.GetString(Constants.sessionchngProductDetails));
                var itemToRemove = productDetailsList.Find(r => r.UniqueId == Id);
                itemToRemove.IsActive = false;
                itemToRemove.IsDeleted = true;
                itemToRemove.Action = (int)Constant.Delete;
                productDetailsList.Add(itemToRemove);
                _sessionManager.SetProductDetailsList(productDetailsList);
                if (productDetailsList.Count != 0)
                {
                    activeProductList = productDetailsList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                }
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, Id));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.chngprodViewPath + Constants.ViewAll, activeProductList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

    }
}
