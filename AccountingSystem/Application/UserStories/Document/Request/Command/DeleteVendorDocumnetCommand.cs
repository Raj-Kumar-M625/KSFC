using Application.DTOs.Document;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Document.Request.Command
{
    public class DeleteVendorDocumnetCommand : IRequest<DocumentsDto>
    {
        public int ID { get; set; }
    }
}
