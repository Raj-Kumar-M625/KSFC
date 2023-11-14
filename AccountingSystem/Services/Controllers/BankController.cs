using Application.DTOs;
//using Application.DTOs.SearchCriteria;
//using Application.Features.BankStatements.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BankController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpGet]

        //public async Task<ActionResult<IEnumerable<BankStatementsDto>>> GetBankSatements()
        //{
        //    var bankStatements = await _mediator.Send(new GetBankStatementsListRequest());
        //    return Ok(bankStatements);
        //}

        //[HttpGet("filter")]

        //public async Task<ActionResult<IEnumerable<BankStatementsDto>>> GetBankSatements([FromQuery] BankStatementsSearchCriteria searchCriteria)
        //{
        //    var request = new GetBankStatementsFilterListRequest { searchCriteria = searchCriteria };
        //    var bankStatements = await _mediator.Send(request);
        //    return Ok(bankStatements);
        //}
    }
}
