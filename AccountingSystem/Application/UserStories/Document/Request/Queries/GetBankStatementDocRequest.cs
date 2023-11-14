using Domain.Document;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Document.Request.Queries
{
    public class GetBankStatementDocRequest : IRequest<Documents>
    {
        public int DocumentRefId { get; set; }
    }
}
