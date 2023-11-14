using Application.DTOs.Document;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Requests.Queries
{

    public class GetBillDocumentDetailsRequest : IRequest<List<DocumentsDto>>
    {
        public int ID { get; set; }
    }
}
