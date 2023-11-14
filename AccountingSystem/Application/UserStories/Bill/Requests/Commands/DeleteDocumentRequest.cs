using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Requests.Commands
{
    public class DeleteDocumentRequest: IRequest<string[]>
    {
        public int[] document;
        public string[] filepath;
    }
}
