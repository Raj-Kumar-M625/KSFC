using Application.DTOs.Vendor;
using Application.UserStories.Vendor.Requests.Commands;
using AutoMapper;
using Domain.Vendor;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Repositories.Generic;
using Presentation.Helpers;
using Presentation.Models.Vendor;
using Presentation.Services.Infra.Cache;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Presentation.Controllers.Vendor
{
    /// <summary>
    /// Author:Swetha M Date:20/06/2022
    /// Purpose:Controller for VendorDefaults
    /// </summary>
    /// <returns></returns>
    /// 
    [SessionTimeoutHandlerAttribute]

    public class VendorDefaultsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        private GenericRepository<VendorDefaults> repository;

        protected string CurrentUser => this.HttpContext.User.Identity.Name;

        /// <summary>
        /// Author:Swetha M Date:20/06/2022
        /// Purpose:Constructor for VendorDefaults
        /// </summary>
        /// <returns></returns>
        public VendorDefaultsController(IMediator mediator, IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            repository = new GenericRepository<VendorDefaults>(unitOfWork);
        }
        /// <summary>
        /// Author:Swetha M Date:20/06/2022
        /// Purpose:Add VendorDefault Details to Database
        /// </summary>
        /// <returns>Dropdowns and Add Vendor View</returns>
        [HttpPost]
        public async Task<IActionResult> AddVendorDefautls(VendorDefaultsModel vendorDefaults)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    vendorDefaults.CreatedBy = CurrentUser;
                    _unitOfWork.CreateTransaction();
                    var vendorDefaultsDto = _mapper.Map<VendorDefaultsDto>(vendorDefaults);


                    var command = new AddVendorDefaultsCommand { vendorDefaultsDto = vendorDefaultsDto };

                    await _mediator.Send(command);
                    _unitOfWork.Save();
                    _unitOfWork.Commit();
                    return RedirectToAction("AddVenddor");
                }
              

            }
            catch (Exception)
            {
                Log.Error("Add vendor defaults failed");
                _unitOfWork.Rollback();
            }
            return View();

        }
        /// <summary>
        /// Author:Swetha M Date:22/06/2022
        /// Purpose:Edit VendorDefault Details to Database
        /// </summary>
        /// <returns>Dropdowns and Add Vendor View</returns>

        [HttpPost]
        public async Task<IActionResult> EditVendorDefaults(VendorDefaultsModel vendorDefaults)
        {
            try
            {
                
                    _unitOfWork.CreateTransaction();

                    vendorDefaults.ModifiedBy = CurrentUser;
                    var vendorID = vendorDefaults.VendorId;
                    var vendorDefaultsDto = _mapper.Map<VendorDefaultsDto>(vendorDefaults);
                    var command = new EditVendorDefaultsCommand { vendorDefaultsDto = vendorDefaultsDto };
                   await _mediator.Send(command);

                    return RedirectToAction("EditVendor", "Vendor", new { Id = vendorID });
                
            }
            catch (Exception)
            {
                Log.Error("Edit vendor defaults failed");
                _unitOfWork.Rollback();
            }
            return View();
        }
    }
}
