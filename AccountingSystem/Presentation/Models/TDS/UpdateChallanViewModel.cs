using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Presentation.Models.TDS
{
    public class UpdateChallanViewModel
    {
        public int Id { get; set; }

        public string PaymentDateStr { get; set; }
        public DateTime? PaymentDate
        {
            get;set;
            //get
            //{
            //    return DateTime.TryParseExact(PaymentDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : null;
            //}
        }

        public string UTRNo { get; set; }
        public bool IsBulkTDS { get; set; }
        public string BSRCode { get; set; }

        public string ChallanNo { get; set; }

        public int AssessmentYear { get; set; }

        public decimal? TotalTDSAmount { get; set; }

        [DataType(DataType.Upload)]
        [FromForm(Name = "File")]
        public IFormFile File { get; set; }

        public string TenderDateStr { get; set; }

        public DateTime? TenderDate
        {
            get;set;
            //get
            //{
            //    return DateTime.TryParseExact(TenderDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var tDate) ? tDate : null;
            //}
        }
    }
}
