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
    public class StockRequestReviewTagModel
    {
        public long Id { get; set; }
        public long CyclicCount { get; set; }
    }
}