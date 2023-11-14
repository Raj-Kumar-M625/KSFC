using AutoMapper;
using EDCS_TG.API.Data;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Implementation;
using EDCS_TG.API.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace EDCS_TG.API.Controller
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonalDetailsController : ControllerBase
    {
        
        private readonly IPersonalDetailsServices _personalDetailsService;
        private readonly IMapper _mapper;
        private KarmaniDbContext _karmaniDbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        public PersonalDetailsController(IUnitOfWork repository, IMapper mapper,
             IPersonalDetailsServices personalDetailsServices, KarmaniDbContext karmaniDbContext)
        {
            _personalDetailsService = personalDetailsServices;
            _mapper = mapper;
       
            _karmaniDbContext = karmaniDbContext;
        } 


        //[HttpGet("GetSurveyList")]
        //public async Task<ActionResult<IEnumerable<PersonalDetails>>> GetPersonalDetailsList()
        //{
        //    try
        //    {
        //        var result = await _personalDetailsService.GetPersonalDetailsList();
        //        if (result.Count() > 0)
        //            return Ok(result);
        //        else
        //            return NoContent();
        //    }
        //    catch(Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
           
                
        //}

        //[HttpGet("GetPersonalDetailsBySurveyId")]
        //public async Task<ActionResult<PersonalDetailsDto>> getBuId(int id)
        //{
        //    try
        //    {
        //        var villa = _karmaniDbContext.PersonalDetails.FirstOrDefault(u => u.SurveyTypeId == id);
        //        var result = _mapper.Map<PersonalDetailsDto>(villa);

        //        return result;
        //    }
        //    catch(Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
            
        //}

        //[HttpPost("SavePersonalDetail")]
        //[Authorize(Roles = ("Admin"))]
        //public async Task<ActionResult<PersonalDetailsDto>> addPersonalDetails([FromBody]PersonalDetailsDto personalDetails)
        //{
        //    try
        //    {
        //        var data = _mapper.Map<PersonalDetails>(personalDetails);
        //        data.SurveyTypeId = Guid.NewGuid().GetHashCode();
        //        _karmaniDbContext.PersonalDetails.Add(data);
        //        _karmaniDbContext.SaveChanges();
        //        return Ok(data);
        //    }
        //    catch(Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
            
        //}

        //[HttpPut("UpdatePersonalDetail")]
        //[Authorize(Roles = ("Admin"))]
        //public IActionResult Update(int id,[FromBody]PersonalDetailsDto personalDetailsDto)
        //{
        //    try
        //    {
        //        PersonalDetails model = new()
        //        {
        //            PlaceofBirth = personalDetailsDto.PlaceofBirth,
        //            SurveyTypeId = personalDetailsDto.SurveyTypeId,
        //            Married = personalDetailsDto.Married
        //        };

        //        _karmaniDbContext.PersonalDetails.Update(model);
        //        _karmaniDbContext.SaveChanges();
        //        return NoContent();
        //    }
        //    catch(Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
            
        //}
    }
}
