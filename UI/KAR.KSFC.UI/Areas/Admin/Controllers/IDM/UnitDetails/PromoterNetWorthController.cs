using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
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

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class PromoterNetWorthController : Controller
    {
        private readonly ILogger _logger;
        

        public PromoterNetWorthController(ILogger logger)
        {
            _logger = logger;
        }

        // Author Sandeep M on 09/09/2022
        public IActionResult ViewRecord(long id = 0)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);

                List<AssetLiabalityDTO> AllPromoterLiabityList = new();

                var assetDetailsList = JsonConvert.DeserializeObject<List<IdmPromAssetDetDTO>>(HttpContext.Session.GetString(Constants.sessionAllAssets));
                if(assetDetailsList != null)
                {
                    List<IdmPromAssetDetDTO> activePromoterAssetList = assetDetailsList.Where(x => x.PromoterCode == id && x.IdmPromassetId != 0).ToList();
                    if(activePromoterAssetList.Count != 0)
                    {
                        foreach (var item in activePromoterAssetList)
                        {
                            AssetLiabalityDTO obj = new AssetLiabalityDTO();
                            obj.IdmAssetValue = item.IdmAssetValue;
                            obj.AssettypeDets = item.AssettypeDets;
                            AllPromoterLiabityList.Add(obj);
                        }
                        ViewBag.PromoterName = activePromoterAssetList.First().PromoterName;
                    }
                }

                var promoterLiabilityList = JsonConvert.DeserializeObject<List<TblPromoterLiabDetDTO>>(HttpContext.Session.GetString(Constants.sessionPromLiability));
                if (promoterLiabilityList != null)
                {
                    List<TblPromoterLiabDetDTO> activePromoterLiabList = promoterLiabilityList.Where(x => x.PromoterCode == id && x.PromLiabId != 0).ToList();
                    if(activePromoterLiabList.Count != 0)
                    {
                        foreach (var item in activePromoterLiabList)
                        {
                            AssetLiabalityDTO obj = new AssetLiabalityDTO();
                            obj.LiabVal = item.LiabVal;
                            obj.LiabDesc = item.LiabDesc;
                            AllPromoterLiabityList.Add(obj);
                        }
                        ViewBag.PromoterName = activePromoterLiabList.First().PromoterName;
                    }
                }
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.promNWresultViewPath + Constants.ViewRecord, AllPromoterLiabityList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
