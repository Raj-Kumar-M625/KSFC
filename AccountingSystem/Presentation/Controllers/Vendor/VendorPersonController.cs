using Application.DTOs.Vendor;
using Application.UserStories.Vendor.Requests.Commands;
using AutoMapper;
using Domain.Vendor;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Repositories.Generic;
using Presentation.Helpers;
using Presentation.Models;
using Presentation.Models.Vendor;
using Presentation.Services.Infra.Cache;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Presentation.Controllers.Vendor
{
    /// <summary>
    /// Author:Swetha M Date:20/06/2022
    /// Purpose:Controller for VendorPerson
    /// </summary>
    /// <returns></returns>
    /// 
    [SessionTimeoutHandlerAttribute]

    public class VendorPersonController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        private GenericRepository<VendorPerson> _repository;

        protected string CurrentUser => this.HttpContext.User.Identity.Name;
        /// <summary>
        /// Author:Swetha M Date:20/06/2022
        /// Purpose:Constructor for VendorPerson
        /// </summary>
        /// <returns></returns>
        public VendorPersonController(IMediator mediator, IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = new GenericRepository<VendorPerson>(unitOfWork);
        }
        /// <summary>
        /// Author:Swetha M Date:22/06/2022
        /// Purpose:Edit VendorPerson Details to Database
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> AddVendorInformation(VendorPersonModel vendorperson)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    vendorperson.CreatedBy = CurrentUser;
                    _unitOfWork.CreateTransaction();
                    var vendorpersondetails = _mapper.Map<VendorPersonDto>(vendorperson);

                    var command = new AddVendorPersonCommand { vendorPerson = vendorpersondetails };
                    await _mediator.Send(command);
                    _unitOfWork.Save();
                    _unitOfWork.Commit();
                    ModelState.Clear();
                    return RedirectToAction("Vendor.AddVendor");
                }
               
            }
            catch (Exception)
            {
                Log.Error("Add vendorperson details failed");
                _unitOfWork.Rollback();

            }

            return View();

        }
        /// <summary>
        /// Author:Swetha M Date:22/06/2022
        /// Purpose:Edit VendorPerson Details to Database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditVendorPerson(VendorPersonModel vendorPerson)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.CreateTransaction();
                    vendorPerson.CreatedBy = CurrentUser;
                    vendorPerson.ModifiedBy = CurrentUser;
                    var VendorID = vendorPerson.VendorId;
                    var vendorPersondto = _mapper.Map<VendorPersonDto>(vendorPerson);
                    var command = new EditVendorPersonCommand { vendorPerson = vendorPersondto };
                   await _mediator.Send(command);
                    ModelState.Clear();
                    return RedirectToAction("EditVendor", "Vendor", new { Id = VendorID });
                }
            }
            catch (Exception)
            {
                Log.Error("Edit vendorperson details failed");
                _unitOfWork.Rollback();
            }
            return View();
        }
    }
}
