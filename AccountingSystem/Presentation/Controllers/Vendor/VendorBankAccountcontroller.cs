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
    /// Purpose:VendorBankAccount Controller
    /// </summary>
    /// <returns></returns>
    /// 
    [SessionTimeoutHandlerAttribute]

    public class VendorBankAccountcontroller : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        private GenericRepository<VendorBankAccount> repository;

        //Get Current User
        protected string CurrentUser => this.HttpContext.User.Identity.Name;

        /// <summary>
        /// Author:Swetha M Date:20/06/2022
        /// Purpose:Constructor for VendorBankAccount
        /// </summary>
        /// <returns></returns>
        public VendorBankAccountcontroller(IMediator mediator, IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            repository = new GenericRepository<VendorBankAccount>(unitOfWork);
        }
        /// <summary>
        /// Author:Swetha M Date:20/06/2022
        /// Purpose:Add VendorBanK Details to Database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddVendorBank(VendorBankAccountModel vendorBank)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    vendorBank.CreatedBy = CurrentUser;
                    _unitOfWork.CreateTransaction();
                    var vendorDefaultsDto = _mapper.Map<VendorBankAccountDto>(vendorBank);


                    var addBankAccountCommand = new AddVendorBankAccountCommand { vendorBankAccountDto = vendorDefaultsDto };

                  await _mediator.Send(addBankAccountCommand);
                    _unitOfWork.Save();
                    _unitOfWork.Commit();


                    return RedirectToAction("AddVenddor");
                }

            }
            catch (Exception)
            {
                Log.Error("Add vendor bank account failed");
                _unitOfWork.Rollback();
            }
            return View();

        }
        /// <summary>
        /// Author:Swetha M Date:22/06/2022
        /// Purpose:Edit VendorBank Details to Database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditVendorBank (VendorBankAccountModel accountModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.CreateTransaction();
                    var vendorID = accountModel.VendorId;
                    var vendorBankDto = _mapper.Map<VendorBankAccountDto>(accountModel);
                    var editBankAccountCommand = new EditVendorBankAccountCommand { vendorBankAccountDto = vendorBankDto,user = CurrentUser };
                     await _mediator.Send(editBankAccountCommand);

                    return RedirectToAction("EditVendor", "Vendor", new { Id = vendorID });
                }
            }
            catch(Exception)
            {

                Log.Error("Edit vendor bank account failed");
                _unitOfWork.Rollback();
            }

            return View();
        }
    }
}
