using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class StockInputReviewTagModel
    {
        public long Id { get; set; }

        [Display(Name = "Review Notes (100 chars)")]
        [MaxLength(100, ErrorMessage = "Review Notes can be maximum 100 characters.")]
        public string ReviewNotes { get; set; }

        public long CyclicCount { get; set; }
    }
}