using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using KAR.KSFC.Components.Common.Dto.IDM;
using System.Collections.Generic;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Security.Principal;
using static AutoMapper.Internal.ExpressionFactory;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfDisbursmentProposalService;
using System.Security.Permissions;
using System.Xml;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.CreationOfDisbursmentProposal.DisbursmentProposalDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    
    public class DisbursmentProposalDetailsController : Controller
    {
        private readonly ICreationOfDisbursmentProposalService _createOfDisbursementProposal;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        private const string resultViewPath = "~/Areas/Admin/Views/CreationOfDisbursmentProposal/ProposalDetails/";
        private const string viewPath = "../../Areas/Admin/Views/CreationOfDisbursmentProposal/ProposalDetails/";
        private const string ReleasePath = "../../Areas/Admin/Views/CreationOfDisbursmentProposal/ReleaseAdjustmentAmount/";

        public DisbursmentProposalDetailsController(ILogger logger, SessionManager sessionManager, ICreationOfDisbursmentProposalService createOfDisbursementProposal)
        {
            _logger = logger;
            _sessionManager = sessionManager;
            _createOfDisbursementProposal = createOfDisbursementProposal;
        }
        public IActionResult Index()
        {   
            return View();
        }

        [HttpGet]
        public IActionResult CreateChargeList(long AccountNumber, int LoanSub, byte OffCd, string uniqid,int total)
        {
            try
            {

                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);

                var DeptMaster = _sessionManager.GetDDListDeptMaster();
                var otherdebitmast = _sessionManager.GetAllOtherDebitCode();
                var dsbChargeMap = _sessionManager.GetAllDsbChargeMap();
                var AllProposalDetailsList = _sessionManager.GetAllProposalDetailsList();
                if (uniqid != null)
                {

                    //         TblIdmReleDetlsDTO ProposalDetailsList = AllProposalDetailsList.First(x => x.UniqueId == uniqid);
                    //    ViewBag.otherdebitMast = otherdebitmast;
                    //    ViewBag.dsbChargeMap = dsbChargeMap;
                    //    ViewBag.DeptMaster = DeptMaster;
                    //    ViewBag.AccountNumber = AccountNumber;
                    //    ViewBag.LoanSub = LoanSub;
                    //    ViewBag.OffCd = OffCd;
                       ViewBag.Uniqid = uniqid;
                    //    _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                    //    //return View(resultViewPath + "_CreateListOfCharge.cshtml", AllProposalDetailsList);
                    //    return Json(new { html = Helper.RenderRazorViewToString(this, viewPath + "_CreateListOfCharge", ProposalDetailsList), status = Constants.success });
            }
                ViewBag.otherdebitMast = otherdebitmast;
                ViewBag.dsbChargeMap = dsbChargeMap;
                ViewBag.DeptMaster = DeptMaster;
                ViewBag.AccountNumber = AccountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffCd = OffCd;
                ViewBag.TotalAmount = total;
                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
              //return View(resultViewPath + "_CreateListOfCharge.cshtml", AllProposalDetailsList);
               return Json(new { html = Helper.RenderRazorViewToString(this, viewPath + "_CreateListOfCharge", AllProposalDetailsList), status = Constants.success });
                //return Json(AllProposalDetailsList,JsonRequestBehavior.AlloGet)
                //return Json(new { html = Helper.RenderRazorViewToString(this, Constants.documentviewPath + Constants.UploadDisbursmentDoc, AllProposalDetailsList), status = Constants.success });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]

        public ActionResult CreateChargeList(long LoanAcc, int LoanSub, byte OffCd , string DsbOthdebitDesc, decimal ReleChargeAmount,string uniqid,int BenfAmt,int ReleOthGlcd,int ReleFdGlcd) // passuniquei
        {
            try
            {
                     
                List<TblIdmReleDetlsDTO> proposalDetails = new();
                
                List<TblIdmReleDetlsDTO> activeproposalDetails = new();
                if (_sessionManager.GetAllProposalDetailsList() != null)
                    proposalDetails = _sessionManager.GetAllProposalDetailsList();
                TblIdmReleDetlsDTO newpropdetails = new();
                if (uniqid != null)
                {
                     newpropdetails = proposalDetails.Find(x => x.UniqueId == uniqid);
                    newpropdetails.BenfAmt = BenfAmt - ReleChargeAmount;
                    ViewBag.Uniqid = uniqid;
                }
                //else
                //{
                //     newpropdetails = proposalDetails.Find(x => x.Action == 0);
                //    if (BenfAmt > 0)
                //    {
                //        newpropdetails.BenfAmt = BenfAmt - ReleChargeAmount;
                //    }
                //    else
                //    {
                //        newpropdetails.BenfAmt -=  ReleChargeAmount;

                //    }

                //}




                //List<TblIdmReleDetlsDTO> dumpnewpropdetails = proposalDetails.Where(x => x.Action == null).ToList();
                //if (dumpnewpropdetails.Any())
                //{
                //    foreach (var i in dumpnewpropdetails)
                //    {
                //        proposalDetails.Remove(i);
                //    }
                //}

                if (newpropdetails != null)
                {
                    proposalDetails.Remove(newpropdetails);

                    var list = newpropdetails;
                    list.LoanAcc = LoanAcc;
                    list.OffcCd = OffCd;
                    list.LoanSub = LoanSub;
                    list.ReleChargeAmount = ReleChargeAmount;
                    list.Action = (int)Constant.Update;
                    list.ReleOthGlcd = ReleOthGlcd;
                    list.ReleFdGlcd = ReleFdGlcd;
                    var otherdebitmast = _sessionManager.GetAllOtherDebitCode();
                    list.DsbOthdebitDesc = DsbOthdebitDesc;
                    var mastid = otherdebitmast.Where(x => x.Text == list.DsbOthdebitDesc);
                    list.DsbOthdebitId = Convert.ToInt32(mastid.First().Value);

                    var dsbChargeMap = _sessionManager.GetAllDsbChargeMap();
                    var dsbCharge = dsbChargeMap.Where(x => Convert.ToInt32(x.Value) == list.DsbOthdebitId).ToList();
                    switch (dsbCharge.First().Text)
                    {
                        case "rele_bnk_chg":
                            list.ReleBnkChg = ReleChargeAmount;
                            list.BnkChrg = DsbOthdebitDesc;
                            break;
                        case "rele_doc_chg":
                            list.ReleDocChg = ReleChargeAmount;
                            list.DocCharge = DsbOthdebitDesc;
                            break;
                        case "addl_amt1":
                            list.AddlAmt1 = (int?)ReleChargeAmount;
                            list.Insurencecharge = DsbOthdebitDesc;
                            break;
                        case "addl_amt2":
                            list.AddlAmt2 = (int?)ReleChargeAmount;
                            list.LegalCharge = DsbOthdebitDesc;
                            break;
                        case "addl_amt3":
                            list.AddlAmt3 = (int?)ReleChargeAmount;
                            list.Penalty = DsbOthdebitDesc;
                            break;
                        case "rele_at_par_amt":
                            list.ReleAtParAmount = (int?)ReleChargeAmount;
                            list.AtParAmt = DsbOthdebitDesc;
                            break;
                        case "rele_up_frt_amt":
                            list.ReleUpFrtAmount = ReleChargeAmount;
                            list.UpCharge = DsbOthdebitDesc;
                            break;

                        case "rele_com_chg":
                            list.ReleComChg = ReleChargeAmount;
                            list.CommCharge = DsbOthdebitDesc;
                            break;
                        case "rele_fd_amt":
                            list.ReleFdAmount = (int?)ReleChargeAmount;
                            list.FDAmt = DsbOthdebitDesc;
                            break;
                        case "rele_oth_amt":
                            list.ReleOthAmount = (int?)ReleChargeAmount;
                            list.OthAmt = DsbOthdebitDesc;
                            break;
                        case "rele_adj_amt":
                            list.ReleAdjAmount = ReleChargeAmount;
                            list.AdjustAmt = DsbOthdebitDesc;
                            break;
                        case "rele_add_up_frt_amt":
                            list.ReleAddUpFrtAmount = (int?)ReleChargeAmount;
                            list.AddUpAmt= DsbOthdebitDesc;
                            break;
                        case "rele_addlafd_amt":
                            list.ReleAddlafdAmount = (int?)ReleChargeAmount;
                            list.AddFDAmt = DsbOthdebitDesc;
                            break;
                        case "rele_sertax_amt":
                            list.ReleSertaxAmount = (int?)ReleChargeAmount;
                            list.SerTaxAmt = DsbOthdebitDesc;
                            break;
                        case "rele_cersai":
                            list.ReleCersai = (int?)ReleChargeAmount;
                            list.CersaiAmt = DsbOthdebitDesc;
                            break;
                        case "rele_swachcess":
                            list.ReleSwachcess = (int?)ReleChargeAmount;
                            list.SwachCess = DsbOthdebitDesc;
                            break;
                        case "rele_krishikalyancess":
                            list.Relekrishikalyancess = (int?)ReleChargeAmount;
                            list.KrishiCess = DsbOthdebitDesc;
                            break;
                        case "rele_coll_guarantee_fee":
                            list.ReleCollGuaranteeFee =(int?)ReleChargeAmount;
                            list.CollGuarFee = DsbOthdebitDesc;
                            break;
                        case "addl_amt4":
                            list.AddlAmt4 = (int?)ReleChargeAmount;
                            list.add_amt1 = DsbOthdebitDesc;
                            break;
                        case "addl_amt5":
                            list.AddlAmt5 = (int?)ReleChargeAmount;
                            list.add_amt2 = DsbOthdebitDesc;
                            break;

                    }
                    proposalDetails.Add(list);
                    _sessionManager.SetProposalDetailsList(proposalDetails);
                    var AllProposalDetailsList = _sessionManager.GetAllProposalDetailsList();
                    ViewBag.ProposalDetails = proposalDetails.Where(x=> x.LoanAcc == LoanAcc && x.UniqueId == uniqid).ToList();
                    ViewBag.dsbChargeMap = dsbChargeMap;
                    ViewBag.otherdebitMast = otherdebitmast; 
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.OffCd = list.OffcCd;
                    ViewBag.LoanSub = list.LoanSub;
                   
                }
                //return RedirectToAction("CreateAddChargeList", "DisbursmentProposalDetails", new { @uniqid = uniqid });
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, ReleasePath + "_ViewListOfCharges") });



            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
                                                                                                                                            


        public async Task<IActionResult> CreateAddChargeList(long AccountNumber, int LoanSub, byte OffCd , string uniqid)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);

                var DeptMaster = _sessionManager.GetDDListDeptMaster();
                var otherdebitmast = _sessionManager.GetAllOtherDebitCode();
                var dsbChargeMap = _sessionManager.GetAllDsbChargeMap();
                var AllProposalDetailsList = _sessionManager.GetAllProposalDetailsList();
              
             
               
                ViewBag.otherdebitMast = otherdebitmast;
                ViewBag.dsbChargeMap = dsbChargeMap;
                ViewBag.DeptMaster = DeptMaster;
                if (AccountNumber != 0)
                {
                    ViewBag.AccountNumber = AccountNumber;
                    ViewBag.LoanSub = LoanSub;
                    ViewBag.OffCd = OffCd;
                }
                else
                {
                    ViewBag.AccountNumber = AllProposalDetailsList.FirstOrDefault().LoanAcc;
                    ViewBag.LoanSub = AllProposalDetailsList.FirstOrDefault().LoanSub;
                    ViewBag.OffCd = AllProposalDetailsList.FirstOrDefault().OffcCd;

                }

                ViewBag.TotalAmount = AllProposalDetailsList.LastOrDefault().ReleAmount;//New Added


                if (uniqid != null)
                {
                    var propeditlist = AllProposalDetailsList.First(x => x.UniqueId == uniqid);
                    if (propeditlist.Action != 0)
                    {
                        var allbeneficiaryDetails = await _createOfDisbursementProposal.GetAllBeneficiaryDetails((long)propeditlist.LoanAcc);

                        var deptlist = allbeneficiaryDetails.Where(x => x.BenfNumber == propeditlist.TblIdmDisbProp.PropNumber).ToList();
                        if (deptlist.Count > 0)
                        {
                            var deptcode = deptlist.FirstOrDefault().BenfDept;
                            propeditlist.DeptCode = (int)deptcode;
                        }
                        propeditlist.Action = (int)Constant.Update;
                        ViewBag.ProposalDetails = propeditlist;
                        return View(resultViewPath + "Edit.cshtml", propeditlist);
                    }
                   
                }


                ViewBag.ProposalDetails = AllProposalDetailsList;
                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                var proplist = AllProposalDetailsList.First(x => x.Action == 0);

                return View(resultViewPath + "Create.cshtml", proplist);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


            
        [HttpGet]
        public IActionResult Create(long AccountNumber, int LoanSub, byte OffCd)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);

                var DeptMaster = _sessionManager.GetDDListDeptMaster();
               var otherdebitmast =  _sessionManager.GetAllOtherDebitCode();
                var dsbChargeMap = _sessionManager.GetAllDsbChargeMap();
                var AllProposalDetailsList = _sessionManager.GetAllProposalDetailsList();
                var recomanded = _sessionManager.GetAllRecommDisbursementDetail();
                ViewBag.Recommended = recomanded.Sum(x => x.DsbAmt);

                ViewBag.ProposalDetails = AllProposalDetailsList;
                ViewBag.otherdebitMast = otherdebitmast;
                ViewBag.dsbChargeMap = dsbChargeMap;
                ViewBag.DeptMaster = DeptMaster;
                if(AccountNumber != 0)
                {
                    ViewBag.AccountNumber = AccountNumber;
                    ViewBag.LoanSub = LoanSub;
                    ViewBag.OffCd = OffCd;
                }
                else
                {
                    ViewBag.AccountNumber = AllProposalDetailsList.FirstOrDefault().LoanAcc;
                    ViewBag.LoanSub = AllProposalDetailsList.FirstOrDefault().LoanSub;
                    ViewBag.OffCd = AllProposalDetailsList.FirstOrDefault().OffcCd;
                    ViewBag.TotalAmount = AllProposalDetailsList.LastOrDefault().ReleAmount;
                }

                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(resultViewPath + "Create.cshtml");

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TblIdmReleDetlsDTO proposal)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.CreateStartedPost + LogAttribute.ProposalDetailsDto,
                     proposal.ReleAmount, proposal.LoanAcc, proposal.OffcCd));

                Random rand = new Random(); // <-- Make this static somewhere
                const int maxValue = 9999;
                var number =Convert.ToInt64( rand.Next(maxValue + 1).ToString("D4"));

                
               

                if (ModelState.IsValid)
                {

                    List<TblIdmReleDetlsDTO> proposalDetails = new();
                    List<TblIdmReleDetlsDTO> activeproposalDetails = new();
                    if (_sessionManager.GetAllProposalDetailsList() != null)
                        proposalDetails = _sessionManager.GetAllProposalDetailsList();

                    TblIdmReleDetlsDTO list = new TblIdmReleDetlsDTO();
                    TblIdmDisbPropDTO proplist = new TblIdmDisbPropDTO();
                    list.LoanAcc = proposal.LoanAcc;
                    list.OffcCd = proposal.OffcCd;
                    list.LoanSub = proposal.LoanSub;                   
                    list.TblIdmDisbProp = proplist;
                    list.TblIdmDisbProp.PropNumber = number;
                    list.DeptCode = proposal.DeptCode;
                    list.ReleAtParCharges = proposal.ReleAtParCharges;
                    list.ReleAmount = proposal.ReleAmount;
                    list.ReleDueAmount = proposal.ReleDueAmount;
                    list.ReleDocChg = proposal.ReleDocChg;
                    list.ReleAdjAmount = proposal.ReleAdjAmount;
                    list.ReleFdAmount = proposal.ReleFdAmount;
                    list.ReleComChg = proposal.ReleComChg;
                    list.ReleAdjRecSeq = proposal.ReleAdjRecSeq;
                    list.ReleAddUpFrtAmount = proposal.ReleAddUpFrtAmount;
                    list.ReleBnkChg = proposal.ReleBnkChg;
                    list.BenfAmt = proposal.BenfAmt;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    proposalDetails.Add(list);
                    _sessionManager.SetProposalDetailsList(proposalDetails);
                    if (proposalDetails.Count != 0)
                    {
                        activeproposalDetails = (proposalDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }
                    _logger.Information(string.Format(CommonLogHelpers.CreateCompletedPost + LogAttribute.ProposalDetailsDto,
                       proposal.ReleId, proposal.LoanAcc, proposal.OffcCd));
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.OffCd = list.OffcCd;
                    ViewBag.LoanSub = list.LoanSub;
                    return Json(new { isValid = true, data = proposal.LoanAcc, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", activeproposalDetails) });
                    //return RedirectToAction("Create", "DisbursmentProposalDetails");
                }
                ViewBag.AccountNumber = proposal.LoanAcc;
                ViewBag.OffCd = proposal.OffcCd;
                ViewBag.LoanSub = proposal.LoanSub;
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.AuditClearanceDto,
                    proposal.ReleId, proposal.LoanAcc, proposal.OffcCd));
                 return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "Create", proposal) });
               // return RedirectToAction("Create", "DisbursmentProposalDetails");
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public async Task<IActionResult> Edit(string unqid = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + unqid);
                var AllProposalDetailsList = _sessionManager.GetAllProposalDetailsList();
                var DeptMaster = _sessionManager.GetDDListDeptMaster();
                ViewBag.DeptMaster = DeptMaster;
                TblIdmReleDetlsDTO ProposalDetailsList = AllProposalDetailsList.First(x => x.UniqueId == unqid);
                var allbeneficiaryDetails = await _createOfDisbursementProposal.GetAllBeneficiaryDetails((long)ProposalDetailsList.LoanAcc);

                var deptlist = allbeneficiaryDetails.Where(x => x.BenfNumber == ProposalDetailsList.TblIdmDisbProp.PropNumber).ToList();
                if (deptlist.Count() > 0)
                {
                    var deptcode = deptlist.FirstOrDefault().BenfDept;
                    ProposalDetailsList.DeptCode = (int)deptcode;
                }

                ViewBag.ProposalDetails = ProposalDetailsList;
                ViewBag.AccountNumber = ProposalDetailsList.LoanAcc;
                ViewBag.OffCd = ProposalDetailsList.OffcCd;
                ViewBag.LoanSub = ProposalDetailsList.LoanSub;
                ViewBag.TotalAmount = ProposalDetailsList.ReleAmount;
                _logger.Information(CommonLogHelpers.UpdateCompleted + unqid);
                return View(resultViewPath + "Edit.cshtml", ProposalDetailsList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TblIdmReleDetlsDTO proposal)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.UpdateStartedPost + LogAttribute.ProposalDetailsDto,
                  proposal.ReleAmount, proposal.LoanAcc, proposal.OffcCd));

                List<TblIdmReleDetlsDTO> proposalDetails = new();
                List<TblIdmReleDetlsDTO> activeproposalDetails = new();
                if (_sessionManager.GetAllProposalDetailsList() != null)
                    proposalDetails = _sessionManager.GetAllProposalDetailsList();

                TblIdmReleDetlsDTO proposalDetailsExist = proposalDetails.Find(x => x.UniqueId == proposal.UniqueId);
                long? accountNumber = 0;
                if (proposalDetailsExist != null)
                {
                    accountNumber = proposalDetailsExist.LoanAcc;
                    proposalDetails.Remove(proposalDetailsExist);
                    var list = proposalDetailsExist;
                    list.LoanAcc = proposal.LoanAcc;
                    list.OffcCd = proposal.OffcCd;
                    list.LoanSub = proposal.LoanSub;
                    list.ReleAtParCharges = proposal.ReleAtParCharges;
                    list.ReleAmount = proposal.ReleAmount;
                    list.ReleDueAmount = proposal.ReleDueAmount;
                    list.ReleDocChg = proposalDetailsExist.ReleDocChg;
                    list.ReleAdjAmount = proposal.ReleAdjAmount;
                    list.ReleFdAmount = proposal.ReleFdAmount;
                    list.ReleComChg = proposal.ReleComChg;
                    list.ReleAdjRecSeq = proposal.ReleAdjRecSeq;
                    list.ReleAddUpFrtAmount = proposal.ReleAddUpFrtAmount;
                    list.ReleBnkChg = proposalDetailsExist.ReleBnkChg;

                    list.IsActive = true;
                    list.IsDeleted = false;

                    if (proposalDetailsExist.ReleId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;
                    }

                    proposalDetails.Add(list);
                    _sessionManager.SetProposalDetailsList(proposalDetails);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.OffCd = list.OffcCd;
                    ViewBag.LoanSub = list.LoanSub;

                    if (proposalDetails.Count != 0)
                    {
                        activeproposalDetails = (proposalDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }

                    _logger.Information(string.Format(CommonLogHelpers.UpdateCompletedPost + LogAttribute.ProposalDetailsDto,
                  proposal.ReleAmount, proposal.LoanAcc, proposal.OffcCd));
                    // return Json(new { isValid = true, data = accountNumber, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", activeproposalDetails) });
                    return RedirectToAction("Edit", "DisbursmentProposalDetails", new { @unqid = proposal.UniqueId });
                }
                ViewBag.AccountNumber = proposal.LoanAcc;
                ViewBag.OffCd = proposal.OffcCd;
                ViewBag.LoanSub = proposal.LoanSub;
                ViewBag.TotalAmount = proposal.ReleAmount;


                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.ProposalDetailsDto,
                proposal.ReleAmount, proposal.LoanAcc, proposal.OffcCd));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "Edit", proposal) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public async Task<IActionResult> ViewRecord(string unqid = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + unqid);
              
                var otherdebitmast = _sessionManager.GetAllOtherDebitCode();
                var dsbChargeMap = _sessionManager.GetAllDsbChargeMap();
              
                ViewBag.otherdebitMast = otherdebitmast;
                ViewBag.dsbChargeMap = dsbChargeMap;

               
                var DeptMaster = _sessionManager.GetDDListDeptMaster();
                ViewBag.DeptMaster = DeptMaster;
                var AllproposalDetails = _sessionManager.GetAllProposalDetailsList();
                TblIdmReleDetlsDTO proposalDetails = AllproposalDetails.FirstOrDefault(x => x.UniqueId == unqid);
                var allbeneficiaryDetails = await _createOfDisbursementProposal.GetAllBeneficiaryDetails((long)proposalDetails.LoanAcc);

                var deptlist = allbeneficiaryDetails.Where(x => x.BenfNumber == proposalDetails.TblIdmDisbProp.PropNumber).ToList();
                if (deptlist.Count() > 0)
                {
                    var deptcode = deptlist.FirstOrDefault().BenfDept;
                    proposalDetails.DeptCode = (int)deptcode;
                }
                ViewBag.ProposalDetails = proposalDetails;
                ViewBag.TotalAmount = proposalDetails.ReleAmount;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + unqid);
                return View(resultViewPath + "ViewRecord.cshtml", proposalDetails);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public IActionResult Delete(string unqid = "")
        {
            try
            {
                IEnumerable<TblIdmReleDetlsDTO> activeProposalList = new List<TblIdmReleDetlsDTO>();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, unqid));
                var proposalList = JsonConvert.DeserializeObject<List<TblIdmReleDetlsDTO>>(HttpContext.Session.GetString("SessionAllProposalDetailsList"));
                var itemToRemove = proposalList.Find(r => r.UniqueId == unqid);
                itemToRemove.IsActive = false;
                itemToRemove.IsDeleted = true;
                itemToRemove.Action = (int)Constant.Delete;
                proposalList.Add(itemToRemove);
                _sessionManager.SetProposalDetailsList(proposalList);
                if (proposalList.Count > 0)
                {
                    activeProposalList = proposalList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                }
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, unqid));
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.OffCd = itemToRemove.OffcCd;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", activeProposalList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        public PartialViewResult DownloadToPdf(string id)
        {
            _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
            var AllproposalDetails = _sessionManager.GetAllProposalDetailsList();
            TblIdmReleDetlsDTO proposalDetails = AllproposalDetails.FirstOrDefault(x => x.UniqueId == id);
            _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
            return PartialView(resultViewPath + "DownloadView.cshtml", proposalDetails);

        }

        [HttpPost]
        public IActionResult CreateNewProp()
        {
            try
            {
                var AllProposalDetailsList = _sessionManager.GetAllProposalDetailsList();

                //return RedirectToAction("Create", "DisbursmentProposalDetails");

                ViewBag.AccountNumber = AllProposalDetailsList.First().LoanAcc;
                ViewBag.OffCd = AllProposalDetailsList.First().OffcCd;
                ViewBag.LoanSub = AllProposalDetailsList.First().LoanSub;
                return Json(new { isValid = true, data = AllProposalDetailsList.First().LoanAcc, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", AllProposalDetailsList) });

                //  return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "Create") });
                // return RedirectToAction("Create", "DisbursmentProposalDetails");
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        [HttpGet]
        public IActionResult ViewChargeList(string bankdisc, string unqid = "")
        {
            try
            {
                var AllproposalDetails = _sessionManager.GetAllProposalDetailsList();
                TblIdmReleDetlsDTO proposalDetails = AllproposalDetails.LastOrDefault(x => x.UniqueId == unqid);
                switch (bankdisc)
                {
                    case "bankcharge":
                        ViewBag.charge = "Bank Chargers";
                        ViewBag.chargeamount = proposalDetails.ReleBnkChg;
                        break;
                    case "documentationcharge":
                        ViewBag.charge = "Documentation Chargers";
                        ViewBag.chargeamount = proposalDetails.ReleDocChg;
                        break;
                    case "insurancecharge":
                        ViewBag.charge = "Insurance Chargers";
                        ViewBag.chargeamount = proposalDetails.AddlAmt1;
                        break;
                    case "legalcharge":
                        ViewBag.charge = "Legal Chargers";
                        ViewBag.chargeamount = proposalDetails.AddlAmt2;
                        break;
                    case "penalty":
                        ViewBag.charge = "Penalty";
                        ViewBag.chargeamount = proposalDetails.AddlAmt3;
                        break;
                    case "atparamt":
                        ViewBag.charge = "At Par Amount";
                        ViewBag.chargeamount = proposalDetails.ReleAtParAmount;
                        break;
                    case "upcharge":
                        ViewBag.charge = "Upfront Charges";
                        ViewBag.chargeamount = proposalDetails.ReleUpFrtAmount;
                        break;
                    case "commissioncharge":
                        ViewBag.charge = "Commission Charges";
                        ViewBag.chargeamount = proposalDetails.ReleComChg;
                        break;
                    case "fdamt":
                        ViewBag.charge = "FD Amount";
                        ViewBag.chargeamount = proposalDetails.ReleFdAmount;
                        break;
                    case "othamt":
                        ViewBag.charge = "Other Amount";
                        ViewBag.chargeamount = proposalDetails.ReleOthAmount;
                        break;
                    case "adjamt":
                        ViewBag.charge = "Adjustment Amount";
                        ViewBag.chargeamount = proposalDetails.ReleAdjAmount;
                        break;
                    case "addupamt":
                        ViewBag.charge = "Additional Upfront Amount";
                        ViewBag.chargeamount = proposalDetails.ReleAddUpFrtAmount;
                        break;
                    case "addfdamt":
                        ViewBag.charge = "Additional FD Amount";
                        ViewBag.chargeamount = proposalDetails.ReleAddlafdAmount;
                        break;
                    case "servicetaxamt":
                        ViewBag.charge = "Service Tax Amount";
                        ViewBag.chargeamount = proposalDetails.ReleSertaxAmount;
                        break;
                    case "cersaiamt":
                        ViewBag.charge = "Cersai Amount";
                        ViewBag.chargeamount = proposalDetails.ReleCersai;
                        break;
                    case "swachcess":
                        ViewBag.charge = "Swach Cess";
                        ViewBag.chargeamount = proposalDetails.ReleSwachcess;
                        break;
                    case "krishicess":
                        ViewBag.charge = "Krishikalyan Cess";
                        ViewBag.chargeamount = proposalDetails.Relekrishikalyancess;
                        break;
                    case "collguarfee":
                        ViewBag.charge = "Collateral Guarantee Fee";
                        ViewBag.chargeamount = proposalDetails.ReleCollGuaranteeFee;
                        break;
                    case "addamt1":
                        ViewBag.charge = "additional_amount1";
                        ViewBag.chargeamount = proposalDetails.AddlAmt4;
                        break;
                    case "addamt2":
                        ViewBag.charge = "additional_amount2";
                        ViewBag.chargeamount = proposalDetails.AddlAmt5;
                        break;
                }
                var DeptMaster = _sessionManager.GetDDListDeptMaster();
                var dsbChargeMap = _sessionManager.GetAllDsbChargeMap();
                var otherdebitmast = _sessionManager.GetAllOtherDebitCode();
                ViewBag.otherdebitMast = otherdebitmast;
                ViewBag.ReleOthGlcd = proposalDetails.ReleOthGlcd;
                ViewBag.ReleFdGlcd = proposalDetails.ReleFdGlcd;
                ViewBag.dsbChargeMap = dsbChargeMap;
                ViewBag.DeptMaster = DeptMaster;
               // ViewBag.AccountNumber = AccountNumber;
               // ViewBag.LoanSub = LoanSub;
                //ViewBag.OffCd = OffCd;

               // _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                //return View(resultViewPath + "_CreateListOfCharge.cshtml", AllProposalDetailsList);
                return Json(new { html = Helper.RenderRazorViewToString(this, viewPath + "_ViewChargeList"), status = Constants.success });
                //return Json(AllProposalDetailsList,JsonRequestBehavior.AlloGet)
                //return Json(new { html = Helper.RenderRazorViewToString(this, Constants.documentviewPath + Constants.UploadDisbursmentDoc, AllProposalDetailsList), status = Constants.success });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        [HttpPost]
        public IActionResult DeleteCharges( string bankdisc, byte OffCd,string unqid = "")
        {
            try
            {
                IEnumerable<TblIdmReleDetlsDTO> activeProposalList = new List<TblIdmReleDetlsDTO>();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, unqid));
                var proposalList = JsonConvert.DeserializeObject<List<TblIdmReleDetlsDTO>>(HttpContext.Session.GetString("SessionAllProposalDetailsList"));
                var list = proposalList.Find(r => r.UniqueId == unqid);
                proposalList.Remove(list);
                var itemToRemove = list;
                var DeptMaster = _sessionManager.GetDDListDeptMaster();
                var otherdebitmast = _sessionManager.GetAllOtherDebitCode();
                var dsbChargeMap = _sessionManager.GetAllDsbChargeMap();

                ViewBag.otherdebitMast = otherdebitmast;
                ViewBag.dsbChargeMap = dsbChargeMap;
                ViewBag.DeptMaster = DeptMaster;
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffCd = OffCd;
                ViewBag.TotalAmount = 0;
                switch (bankdisc)
                {
                    case "bankcharge":
                        itemToRemove.BnkChrg = null;
                        itemToRemove.ReleBnkChg = null;
                        break;
                    case "documentationcharge":
                   
                        itemToRemove.DocCharge = null;
                        itemToRemove.ReleDocChg = null;
                        break;
                    case "insurancecharge":
                        itemToRemove.Insurencecharge = null;
                        itemToRemove.AddlAmt1 = null;
                        break;

                    case "legalcharge":
                        itemToRemove.LegalCharge = null;
                        itemToRemove.AddlAmt2 = null;
                        break;
                    case "penalty":
                        itemToRemove.Penalty = null;
                        itemToRemove.AddlAmt3 = null;
                        break;
                    case "atparamt":
                        itemToRemove.AtParAmt = null;
                        itemToRemove.ReleAtParAmount = null;
                        break;
                    case "upcharge":
                        itemToRemove.UpCharge = null;
                        itemToRemove.ReleUpFrtAmount = null;
                        break;
                    case "commissioncharge":
                        itemToRemove.CommCharge = null;
                        itemToRemove.ReleComChg = null;
                        break;
                    case "fdamt":
                        itemToRemove.FDAmt = null;
                        itemToRemove.ReleFdAmount = null;
                        break;
                    case "othamt":
                        itemToRemove.OthAmt = null;
                        itemToRemove.ReleOthAmount = null;
                        break;
                    case "adjamt":
                        itemToRemove.AdjustAmt = null;
                        itemToRemove.ReleAdjAmount = null;
                        break;
                    case "addupamt":
                        itemToRemove.AddUpAmt = null;
                        itemToRemove.ReleAddUpFrtAmount = null;
                        break;
                    case "addfdamt":
                        itemToRemove.AddFDAmt = null;
                        itemToRemove.ReleAddlafdAmount = null;
                        break;
                    case "servicetaxamt":
                        itemToRemove.SerTaxAmt = null;
                        itemToRemove.ReleSertaxAmount = null;
                        break;
                    case "cersaiamt":
                        itemToRemove.CersaiAmt = null;
                        itemToRemove.ReleCersai = null;
                        break;
                    case "swachcess":
                        itemToRemove.SwachCess = null;
                        itemToRemove.ReleSwachcess = null;
                        break;
                    case "krishicess":
                        itemToRemove.KrishiCess = null;
                        itemToRemove.Relekrishikalyancess = null;
                        break;
                    case "collguarfee":
                        itemToRemove.CollGuarFee = null;
                        itemToRemove.ReleCollGuaranteeFee = null;
                        break;
                    case "addamt1":
                        itemToRemove.add_amt1 = null;
                        itemToRemove.AddlAmt4 = null;
                        break;
                    case "addamt2":
                        itemToRemove.add_amt2 = null;
                        itemToRemove.AddlAmt5 = null;
                        break;
                  
                }
                itemToRemove.Action = (int)Constant.Update;
                itemToRemove.ReleOthGlcd = null;
                itemToRemove.ReleFdGlcd = null;
                proposalList.Add(itemToRemove);
                _sessionManager.SetProposalDetailsList(proposalList);
                var AllProposalDetailsList = _sessionManager.GetAllProposalDetailsList();
                ViewBag.ProposalDetails = AllProposalDetailsList.Where(x => x.UniqueId == unqid).ToList();
                ViewBag.Uniqid = itemToRemove.UniqueId;
                //if (proposalList.Count > 0)
                //{
                //    activeProposalList = proposalList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                //}
                //return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "Create", itemToRemove)});
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, ReleasePath + "_ViewListOfCharges") });
                //return RedirectToAction("CreateAddChargeList", "DisbursmentProposalDetails", new { @uniqid = unqid });
                
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
