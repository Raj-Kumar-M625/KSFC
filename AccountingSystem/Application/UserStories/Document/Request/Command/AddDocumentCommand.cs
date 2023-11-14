using Application.DTOs.Document;
using Application.DTOs.Vendor;
using Domain.Vendor;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Document.Request.Command
{
    public class AddDocumentCommand : IRequest<int>
    {
        public DocumentsDto documents { get; set; }
        public int id { get; set; }
        public string entityPath { get; set; }
        public string entityType { get; set; }
        public string user { get; set; }

    }
}
