using Application.DTOs.Document;
using Application.DTOs.Vendor;
using Domain;
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
    public class EditDocumentCommand : IRequest<int>
    {
        public DocumentsDto vendordocuments { get; set; }
        public string entityPath { get; set; }
        public int id { get; set; }
        public string description { get; set; }
        public string user { get; set; }
        public string entityType { get; set; }



    }
}
