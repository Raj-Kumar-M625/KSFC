using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IDisbursementService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.Disbursement
{
    [Area("Admin")]
    [Authorize(Roles = RolesEnum.Employee)]

    public class OtherRelaxationController : Controller
    {
        private readonly IDisbursementService _disbursementService;
        private readonly SessionManager _sessionManager;
        private readonly ICommonService _commonService;
        private readonly IIdmService _idmService;
      
       
        private const string viewPath = "../../Areas/Admin/Views/Disbursement/OtherRelaxation/";

        public OtherRelaxationController(IDisbursementService disbursementService, SessionManager sessionManager, ICommonService commonService, IIdmService idmService)
        {
            _disbursementService = disbursementService;
            _sessionManager = sessionManager;
            _commonService = commonService;
            _idmService = idmService;
   
        }

        [HttpGet]
        public async Task<IActionResult> ViewOtherRelaxationAsync(string mainModule, long accountNumber)
        {
            //List<RelaxationDTO> relxlist = _sessionManager.GetAllOtherRelaxation();
            var relxlist = await _disbursementService.GetAllOtherRelaxation(accountNumber);
            _sessionManager.SetAllOtherRelaxation(relxlist);

         
            IdmDDLListDTO idmDTO = await _commonService.GetAllIdmDropDownList();
            _sessionManager.SetConditionTypeDDL(idmDTO.AllConditionTypes);

            var allConditionTypes = await _idmService.GetAllConditionTypes();
            foreach (var i in relxlist)
            {
                if(i.ConditionType != 0)
                {
                    var condtype = allConditionTypes.Where(x => x.Value == i.ConditionType);
                    i.CondTypeDet = condtype.First().Text;
                }                
            }
            var relx = relxlist.ToList();
            
            ViewBag.CondType = idmDTO.AllConditionTypes;
            ViewBag.MainModule = mainModule;
            return Json(new { html = Helper.RenderRazorViewToString(this, viewPath + "ViewOtherRelaxation", relx)});
        }        
    }
}