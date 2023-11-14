using Application.DTOs;
using Application.DTOs.Bill;
using Application.DTOs.Document;
using Application.DTOs.Vendor;
using Application.UserStories.Bill.Requests.Commands;
using Application.UserStories.Document.Request.Command;
using Application.UserStories.Vendor.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Common.Helper;
using Domain.Vendor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Repositories;
using Presentation.Helpers;
using Presentation.Models;
using Presentation.Models.Bill;
using Presentation.Models.Vendor;
using Presentation.Services.Infra.Cache;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Controllers.Vendor
{
    [Authorize]
    [SessionTimeoutHandlerAttribute]

    public class DocumentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        //private GenericRepository<Vendors> repository;

        protected string CurrentUser => this.HttpContext.User.Identity.Name;
        public DocumentController(IMediator mediator, IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            //repository = new GenericRepository<VendorBankAccount>(unitOfWork);
        }
        /// <summary>
        /// Author:Swetha M Date:21/06/2022
        /// Purpose:Add the document Details to Database
        /// </summary>
        /// <returns>Returns the uploaded document count</returns>
        [HttpPost]
        public async Task<IActionResult> AddDocuments(DocumentsModel documents)
        {
            try
            {
               
                _unitOfWork.CreateTransaction();
               
                var document=_mapper.Map<DocumentsDto>(documents);
                var documentid=document.DocumentRefID;
                document.EntityType = ValueMapping.Vendor;
                 var command = new AddDocumentCommand { documents = document,id=documentid , entityPath = ValueMapping.vendorDetailsEntityPath, entityType = ValueMapping.Vendor , user = CurrentUser, };

                 await _mediator.Send(command);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                ModelState.Clear();
                TempData["Message"] = PopUpServices.Notify("Added Successfully!", "Vendor Details", notificationType: Alerts.success);
                return this.Ok("Form Data received!");

            }
            catch (Exception)
            {
                Log.Error("Add Vendor Documents failed");
                _unitOfWork.Rollback();
            }

            return View("AddVendor","Vendor");

        }

        /// <summary>
        /// Author:Swetha M Date:22/06/2022
        /// Purpose:Upload Doucments Uploade in Edit Vendor
        /// </summary>
        /// <returns>Uploaded document count is returned</returns>
        [HttpPost]
        public async Task<IActionResult> EditDocuments(DocumentsModel documentsModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                
                    var document = _mapper.Map<DocumentsDto>(documentsModel);

                    var documentid = document.DocumentRefID;
                   // var vendorID = documentsModel.DocumentRefID;
                    var command = new EditDocumentCommand { vendordocuments = document, description = document.Description, id = documentid, entityPath = ValueMapping.vendorDetailsEntityPath, user = CurrentUser, entityType = ValueMapping.Vendor };
                    await _mediator.Send(command);
                    return this.Ok("Saved");
                    //return RedirectToAction("EditVendor", "Vendor", new { Id = documentid });
                }
            }
            catch(Exception)
            {
                Log.Error("Update VendorDocuments Failed");
                _unitOfWork.Rollback();
            }

            return View();
        }



        /// <summary>
        /// Purpose = Add The Documents For Bill Details
        /// Author = Sandeep
        /// Date = 09 June 2022  
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> AddBillDocument(BillItemsModel fileModel)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var billDetails = _mapper.Map<BillItemsDto>(fileModel);
                var command = new CreateBillDocumentCommand { BillPaymentDetailsDto = billDetails, entityType = ValueMapping.entityType2, entityPath = ValueMapping.vendorBillsEntityPath,user =  CurrentUser };
                await _mediator.Send(command);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return this.Ok("Saved");
            }
            catch (Exception e)
            {
                Log.Information("Inside AddBillDocument Method");
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }

        }

    }
}
