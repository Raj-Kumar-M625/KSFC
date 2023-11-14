using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace Presentation.Models.GSTTDS
{
    public class UpdateGstCertificateViewModel
    {
        public List<int> Ids { get; set; }
        public DateTime? PaidDate { get; set; }

        public string UTRNo { get; set; }

        public string GSTR7ACertificate { get; set; }

        public decimal PaidAmount { get; set; }

        [DataType(DataType.Upload)]
        [FromForm(Name = "File")]
        public IFormFile File { get; set; }
    }
}
