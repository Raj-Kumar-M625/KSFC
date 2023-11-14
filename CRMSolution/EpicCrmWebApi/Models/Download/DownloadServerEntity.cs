using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadServerEntity
    {
        // apk 2.1.0296 onwards
        // this is used as primary key on phone - so must be sent as unique
        public string Id { get; set; }  
        public string EntityType { get; set; }
        public string Code { get; set; }
        public long CodeAsLong { get; set; }
        public string Name { get; set; }
        public string UIDType { get; set; } // unique id type
        public string UID { get; set; } // unique id

        // Added in March 2020, while doing PJM Farming changes
        public string EntityNumber { get; set; }
        public string FatherHusbandName { get; set; }
        public string VillageName { get; set; }
        public bool IsCustomer { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public ICollection<DownloadAgreementModel> Agreements { get; set; }
        public ICollection<DownloadSurveyModel> Surveys { get; set; }

        public ICollection<DownloadContactModel> Contacts { get; set; }
        public ICollection<DownloadEntityBankDetailModel> BankDetails { get; set; }
    }
}