using System.Collections.Generic;

using AutoMapper;

using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.Components.Common.Dto.Enquiry;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.Components.Data.Repository.UoW;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KAR.KSFC.API.Areas.Customer.Controllers
{
    //[KSFCAuthorization(role:"Admin")]
    public class EnquirySubmissionController : BaseApiController
    {
        private readonly IEnquirySubmissionService _enquiryService;
        private readonly IUnitOfWork _unitOfWorkService;
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        private readonly ILogger<EnquirySubmissionController> _logger;

        /// <summary>
        /// constructor Dependency injection
        /// </summary>
        /// <param name="enquiryService"></param>
        /// <param name="mapper"></param>
        /// <param name="unitOfWorkService"></param>
        /// <param name="userInfo"></param>
        /// <param name="logger"></param>
        public EnquirySubmissionController(IEnquirySubmissionService enquiryService, IMapper mapper, IUnitOfWork unitOfWorkService, UserInfo userInfo, ILogger<EnquirySubmissionController> logger)
        {
            _enquiryService = enquiryService;
            _mapper = mapper;
            _unitOfWorkService = unitOfWorkService;
            _userInfo = userInfo;
            _logger = logger;
        }

        /// <summary>
        /// Get all enquiry list based on unit pan number.
        /// </summary>
        /// <param name="pan"></param>
        /// <returns></returns>
        [HttpGet, Route("GetEnquiryList")]
        public IActionResult GetAllEnquiryList(string pan)
        {
            //based on PAN Fetch the list of enquiry added before.
            return Ok();
        }

        /// <summary>
        /// Delete the enquiry record by requested enquiry id(Soft delete)
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <returns></returns>
        [HttpDelete, Route("DeleteEnquiry")]
        public IActionResult DeleteEnquiry(int enquiryId)
        {
            //update enquiry Status false
            return Ok();
        }

        /// <summary>
        /// Get Basic details tab data including dropdown list.
        /// </summary>
        /// <param name="PanNum"></param>
        /// <returns></returns>
        [HttpGet, Route("GetBasicDetails")]
        public IActionResult GetBasicDetails(string PanNum)
        {

            if (PanNum == string.Empty)
            {
                return BadRequest("please enter PAN number.");
            }

            //Below are hard-coded for development purpose
            DDLListDTO ddlList = new();
            ddlList.ListBranch = new List<Branch>()
                {
                    new Branch{BranchCode=1,BranchName="Jayanagar"},
                    new Branch{BranchCode=2,BranchName="Yalahanka"},
                    new Branch{BranchCode=3,BranchName="Tumkur"},
                    new Branch{BranchCode=4,BranchName="Bhanashankari"},
                    new Branch{BranchCode=5,BranchName="Koppal"}
                };
            ddlList.ListLoanPurpose = new List<LoanPurpose>()
                {
                    new LoanPurpose{PurposeCode=1,PurposeDesc="Bussiness"},
                    new LoanPurpose{PurposeCode=2,PurposeDesc="Land"}
                };
            ddlList.ListFirmSize = new List<FirmSize>()
                {
                    new FirmSize{SizeCode=1,SizeDesc="Small"},
                    new FirmSize{SizeCode=2,SizeDesc="Medium"},
                    new FirmSize{SizeCode=3,SizeDesc="Large"},
                };
            ddlList.ListProduct = new List<ProductType>()
                {
                    new ProductType{ProductCode=1,ProductDesc="Bikes"},
                    new ProductType{ProductCode=2,ProductDesc="Cars"},
                    new ProductType{ProductCode=3,ProductDesc="Trucks"},
                };
            ddlList.ListDistrict = _mapper.Map<List<DistrictMast>>(_enquiryService.GetAllDistricts());
            ddlList.ListAccountType = new List<BankAccountType>()
                {
                    new BankAccountType{AccountTypeCode=1,AccountTypeDesc="Salary"},
                    new BankAccountType{AccountTypeCode=2,AccountTypeDesc="Current"},
                    new BankAccountType{AccountTypeCode=3,AccountTypeDesc="Saving"},
                };
            ddlList.ListRegnType = new List<FirmRegistration>()
                {
                    new FirmRegistration{ RegnTypeCode=1,RegnTypeDesc="Solo"},
                    new FirmRegistration{ RegnTypeCode=2,RegnTypeDesc="Propriater"},
                };
            ddlList.ListPremises = new List<PremisesType>()
                {
                    new PremisesType{PremisesCode=1,Premisesdesc="ABC"},
                    new PremisesType{PremisesCode=2,Premisesdesc="XYZ"},
                };

            //Based on PAN Take Enquiry Temp Id and give the basic details.
            EnquiryDTO enquiryInfo = new()
            {
                DDLDTO = ddlList
            };
            return Ok(new ApiResultResponse(enquiryInfo, "Success"));

        }

        /// <summary>
        /// Save Basic Details tab data.
        /// </summary>
        /// <param name="enquiryInfo"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveBasicDetails")]
        public IActionResult SaveBasicdetails(EnquiryDTO enquiryInfo)
        {
            _enquiryService.SaveBasicDetails(enquiryInfo);
            _unitOfWorkService.CommitAsync();

            if (enquiryInfo.EnquiryId == 0)
                return Ok(new ApiResultResponse(200,enquiryInfo.EnquiryId,"Basic details created successfully"));

            else
                return Ok(new ApiResultResponse("Basic details updation successful.", "Success"));

        }

        /// <summary>
        /// Get the details of Promotor and guarentor details along with dropdown list.
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <returns></returns>
        [HttpPost, Route("GetPGDetails")]
        public ActionResult GetPGDetails(int enquiryId)
        {

            DDLListDTO dDLListDTO = new();
            dDLListDTO.ListPromDesgnType = new List<PromDesnType>()
                {
                    new PromDesnType{DesnCode=1,DesgnDesc="Admin"},
                    new PromDesnType{DesnCode=2,DesgnDesc="manager"},
                    new PromDesnType{DesnCode=3,DesgnDesc="Genaral manager"},
                };
            dDLListDTO.ListDomicileStatus = new List<DomicileStatus>()
                {
                    new DomicileStatus{StatusCode=1,StatusDesc="Status 1" },
                    new DomicileStatus{StatusCode=2,StatusDesc="Status 2" }
                };
            dDLListDTO.ListAccountType = new List<BankAccountType>()
                {
                    new BankAccountType{AccountTypeCode=1,AccountTypeDesc="Savings"},
                    new BankAccountType{AccountTypeCode=1,AccountTypeDesc="Current"}
                };
            dDLListDTO.ListAssetCategory = new List<AssetCategory>()
                {
                    new AssetCategory{CategoryCode=1,AssetDesc="Internal"},
                    new AssetCategory{CategoryCode=2,AssetDesc="External"},
                };
            dDLListDTO.ListAssetType = new List<AssetType>()
                {
                    new AssetType{AssetTypeCode=1,AssetTypeDesc="ABC"},
                    new AssetType{AssetTypeCode=2,AssetTypeDesc="DEF"},
                };
            dDLListDTO.ListAcquireMode = new List<AssetAcquire>()
                {
                    new AssetAcquire{AcquireCode=1,AcquireDesc="Lease"},
                    new AssetAcquire{AcquireCode=2,AcquireDesc="Self"},
                };

            EnquiryDTO enquiry = new()
            {
                DDLDTO = dDLListDTO
            };

            //Fetch the PG details using enquiryId if they entered tab data before.
            return Ok(new ApiResultResponse(enquiry, "Success"));

        }

        /// <summary>
        /// Save Promotor annd guarantor details.
        /// </summary>
        /// <param name="enquiryInfo"></param>
        /// <returns></returns>
        [HttpPost, Route("SavePGDetails")]
        public IActionResult SavePGDetails(EnquiryDTO enquiryInfo)
        {
            _enquiryService.SavePGDetails(enquiryInfo);
            _unitOfWorkService.CommitAsync();

            if (enquiryInfo.EnquiryId == 0)
                return Ok(new ApiResultResponse(201,enquiryInfo.EnquiryId, "Basic details created successfully"));

            else
                return Ok(new ApiResultResponse("Basic details updation successful.", "Success"));

        }

        /// <summary>
        /// Get Associate/Sister concern details along with dropdown list.
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetSisterConcerns")]
        public IActionResult GetSisterConcerns(int enquiryId)
        {
            DDLListDTO dDLListDTO = new();
            dDLListDTO.ListFacility = new List<BankFacilityType>()
                {
                    new BankFacilityType{FacilityTypeCode=1,FacilityTypeDesc="ABC"},
                    new BankFacilityType{FacilityTypeCode=2,FacilityTypeDesc="XYZ"}
                };
            dDLListDTO.ListAssociate = new List<SisterConcernMast>()
                {
                    new SisterConcernMast{Code=1,Desc="ABC"},
                    new SisterConcernMast{Code=2,Desc="XYZ"}
                };
            dDLListDTO.ListFY = new List<FinancialYear>()
                {
                    new FinancialYear{YearCode=1,YearDesc="2020"},
                    new FinancialYear{YearCode=2,YearDesc="2021"},
                    new FinancialYear{YearCode=3,YearDesc="2022"}
                };

            dDLListDTO.ListFinancialComponent = new List<FinancialComponent>()
                {
                    new FinancialComponent{ComponentCode=1,ComponentDesc="ABC"},
                    new FinancialComponent{ComponentCode=2,ComponentDesc="XYZ"},
                };

            EnquiryDTO enquiry = new()
            {
                DDLDTO = dDLListDTO
            };

            //Fetch the Sister Concern details using enquiryId if they entered tab data before.
            return Ok(new ApiResultResponse(enquiry, "Success"));

        }

        /// <summary>
        /// Save Associate/Sister concern details.
        /// </summary>
        /// <param name="enquiryInfo"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveSisterConcerns")]
        public IActionResult SaveSisterConcerns(EnquiryDTO enquiryInfo)
        {
            _enquiryService.SaveSisterConcern(enquiryInfo);
            _unitOfWorkService.CommitAsync();

            if (enquiryInfo.EnquiryId == 0)
                return Ok(new ApiResultResponse(201,enquiryInfo.EnquiryId, "Sister concern Details created successfully" ));
            else
                return Ok(new ApiResultResponse("Sister concern Details updation successful.", "Success"));

        }

        /// <summary>
        /// Get Project deatils tab information along with dropdown list.
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetProjectDetails")]
        public IActionResult GetProjectDetails(int enquiryId)
        {

            DDLListDTO dDLListDTO = new();
            dDLListDTO.ListMeansOfFinanceCategory = new List<MeansOfFinanceCategory>()
                {
                    new MeansOfFinanceCategory{MFCategoryCode=1,MFCategoryDesc="ABC"},
                    new MeansOfFinanceCategory{MFCategoryCode=2,MFCategoryDesc="XYZ"}
                };
            dDLListDTO.ListMeansOfFinanceType = new List<MeansOfFinanceType>()
                {
                    new MeansOfFinanceType{TypeCode=1,TypeDesc="ABC"},
                    new MeansOfFinanceType{TypeCode=2,TypeDesc="XYZ"},
                };
            dDLListDTO.ListFY = new List<FinancialYear>()
                {
                    new FinancialYear{YearCode=1,YearDesc="2020"},
                    new FinancialYear{YearCode=2,YearDesc="2021"},
                    new FinancialYear{YearCode=3,YearDesc="2022"}
                };
            dDLListDTO.ListFinancialComponent = new List<FinancialComponent>()
                {
                    new FinancialComponent{ComponentCode=1,ComponentDesc="ABC"},
                    new FinancialComponent{ComponentCode=2,ComponentDesc="XYZ"}
                };

            EnquiryDTO enquiry = new()
            {
                DDLDTO = dDLListDTO
            };
            //Fetch the Project details using enquiryId if they entered tab data before.
            return Ok(new ApiResultResponse(enquiry, "Success"));

        }

        /// <summary>
        /// Save project Details.
        /// </summary>
        /// <param name="enquiryInfo"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveProjectDetails")]
        public IActionResult SaveProjectDetails(EnquiryDTO enquiryInfo)
        {
            _enquiryService.SaveProjectDetails(enquiryInfo);
            _unitOfWorkService.CommitAsync();

            if (enquiryInfo.EnquiryId == 0)
                return Ok(new ApiResultResponse(201,enquiryInfo.EnquiryId, "Project Details created successfully"));

            else
                return Ok(new ApiResultResponse("Project Details updation successful.", "Success"));

        }

        /// <summary>
        /// Get Documents details along with dropdown list.
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetDocumentDetails")]
        public IActionResult GetDocumentDetails(int enquiryId)
        {
            DDLListDTO dDLListDTO = new();
            dDLListDTO.ListSecurityType = new List<SecurityTyp>()
                {
                    new SecurityTyp{TypeCode=1,Typedesc="ABC"},
                    new SecurityTyp{TypeCode=2,Typedesc="XYZ"}
                };
            dDLListDTO.ListSecurityDet = new List<SecurityDetail> {
                new SecurityDetail{DetailsCode=1,DetailsDesc="ABC"},
                new SecurityDetail{DetailsCode=2,DetailsDesc="XYZ"}
                };
            dDLListDTO.ListSecurityRelation = new List<PromoterRelation>()
                {
                    new PromoterRelation{RelationCode=1,RelationDesc="ABC" },
                    new PromoterRelation{RelationCode=2,RelationDesc="XYZ" }
                };

            EnquiryDTO enquiry = new()
            {
                DDLDTO = dDLListDTO
            };
            //Fetch the Documents details using enquiryId if they entered tab data before.
            return Ok(new ApiResultResponse(enquiry, "Success"));
        }

        /// <summary>
        /// Save documents related to unit.
        /// </summary>
        /// <param name="enquiryInfo"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveDocumentDetails")]
        public IActionResult SaveSecurityDetails(EnquiryDTO enquiryInfo)
        {

            _enquiryService.SaveSecurityDocuments(enquiryInfo);
            _unitOfWorkService.CommitAsync();

            if (enquiryInfo.EnquiryId == 0)
                return Ok(new ApiResultResponse(201,enquiryInfo.EnquiryId, "Document Details created successfully"));
            else
                return Ok(new ApiResultResponse("Document Details updation successful.","Success"));

        }

        /// <summary>
        /// Genarate reference number and update once the required field has been filled.
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <returns></returns>
        [HttpPost, Route("EnquirySubmission")]
        public IActionResult FinalSubmission(int enquiryId)
        {
            //Update status,Enquiry reference number.
            return Ok("Enquiry succsfully Submitted.");
        }

        /// <summary>
        /// fetch enquiry Details.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("ViewEnquiry")]
        public IActionResult ViewEnquiryDetails()
        {
            EnquiryDTO enquiry = new();
            return Ok(enquiry);
        }

        /// <summary>
        /// To display full enquiry tab details to verify and print. 
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("ViewFullEnquiry")]
        public IActionResult ViewFullEnquiryDetails()
        {
            return Ok();
        }

        /// <summary>
        /// Cascading dropdown list. Binding taluku by distict id followed by taluk and hobli Id.
        /// </summary>
        /// <param name="districtCode"></param>
        /// <param name="talukCode"></param>
        /// <param name="hobliCode"></param>
        /// <returns></returns>
        [HttpPost, Route("GetLocationListById")]
        public IActionResult GetLocationById(int? districtCode, int? talukCode, int? hobliCode)
        {
            if (districtCode == null && talukCode == null && hobliCode == null)
            {
                return new NotFoundObjectResult(new ApiException(404, "Please provide the Id to find location."));

            }

            if (districtCode != null)
            {
                var lstTaluk = _enquiryService.GetTalukByDistCode(districtCode);
                return Ok(new ApiResultResponse(_mapper.Map<List<TalukMast>>(lstTaluk), "Success"));
            }

            if (talukCode != null)
            {
                var lstHobli = _enquiryService.GetHobliByTlqCode(talukCode);
                return Ok(new ApiResultResponse(_mapper.Map<List<HobliMast>>(lstHobli), "Success"));
            }

            var lstVillage = _enquiryService.GetVillageByTHobliCode(hobliCode);
            return Ok(new ApiResultResponse(_mapper.Map<List<VillageMast>>(lstVillage), "Success"));
        }
    }
}
