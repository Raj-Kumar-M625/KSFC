using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;
using System.Reflection;
using log4net;

namespace EDCS_TG.API.Controller
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class OfficeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOfficeService _offficeservice;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public OfficeController(IUnitOfWork unitOfWork, IMapper mapper, IOfficeService offficeservice)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _offficeservice = offficeservice;
        }

        [HttpGet("GetAllOfficeData")]
        public async Task<ActionResult<IList<Office>>> GetAllOfficeData(string stateCOde)
        {
            try
            {
                var officeData = await _offficeservice.getAllOffficeList(stateCOde);
                var OfficeList = officeData.Select(x => new {x.DistrictName,x.DistrictCode, x.DistrictName_KA}).Distinct().ToList();
                return Ok(OfficeList);
            }
            catch(Exception Ex)
            {
                _log.Info(Ex.Message);
                throw Ex;
            }
        }

        [HttpGet("GetTalukList")]
        public async Task<ActionResult<IList<Office>>> GetTalukList(string districtCode)
        {
            try
            {
                var taluk = await _offficeservice.GetTalukList(districtCode);
                var taluklist = taluk.Select(t => new { t.TalukOrTownName, t.TalukOrTownCode,t.TalukOrTownName_KA }).Distinct().ToList();
                return Ok(taluklist);
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
        }

        [HttpGet("GetHobliList")]
        public async Task<ActionResult<IList<Office>>> GetHobliList(string talukCode)
        {
            try
            {
                var Hobli = await _offficeservice.GetHobliList(talukCode);
                var HobliList = Hobli.Select(t => new { t.HobliOrZoneName, t.HobliOrZoneCode, t.HobliOrZoneName_KA }).Distinct().ToList();
                return Ok(HobliList);
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
        }

        [HttpGet("GetVillageList")]
        public async Task<ActionResult<IList<Office>>> GetVillageList(string HobliCode)
        {
            try
            {
                var Village = await _offficeservice.GetVillageList(HobliCode);
                var VillageList = Village.Select(t => new { t.VillageOrWardName, t.VillageOrWardCode, t.VillageOrWardName_KA }).Distinct().ToList();
                return Ok(VillageList);
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
        }
    }
}
