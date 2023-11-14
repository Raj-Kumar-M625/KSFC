using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class AssociationViewModel
    {
        // this is the data that user has entered on association screen
        public IEnumerable<SalesPersonsAssociationDataModel> AssociationModel { get; set; }

        // this is what we derived out of this association
        public IEnumerable<SelectableOfficeHierarchyModel> OfficeHierarchyModel { get; set; }

        // visible Staff
        public IEnumerable<SalesPersonViewModel> VisibleStaffData { get; set; }

        // Available Area Codes
        public IEnumerable<CodeTableEx> AreaCodes { get; set; }

        public string StaffCode { get; set; }
    }
}