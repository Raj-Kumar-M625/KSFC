using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SalesPersonsAssociationDataModel
    {
        [Display(Name = "Scope")]
        public string CodeType { get; set; }

        [Display(Name = "Name")]
        public string CodeName { get; set; }

        [Display(Name = "Since (UTC)")]
        public DateTime AssignedDate { get; set; }

        public string Code { get; set; }
    }
}