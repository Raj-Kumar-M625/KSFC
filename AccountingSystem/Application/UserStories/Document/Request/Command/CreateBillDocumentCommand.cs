using Application.DTOs.Bill;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Document.Request.Command
{
    public class CreateBillDocumentCommand : IRequest<long>
    {
        public BillItemsDto BillPaymentDetailsDto { get; set; }
        public string entityPath { get; set; }
        public string entityType { get; set; }
        public string user { get; set; }
    }

}
