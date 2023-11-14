using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Dto.Enquiry;
using KAR.KSFC.Components.Data.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface
{
    public interface IEnquirySubmissionService
    {
        public List<DistCdtab> GetAllDistricts();
        public List<TlqCdtab> GetTalukByDistCode(int? distCode);
        public List<HobCdtab> GetHobliByTlqCode(int? tlqCode);
        public List<VilCdtab> GetVillageByTHobliCode(int? hobliCode);
        public Task SaveBasicDetails(EnquiryDTO enquiry);
        public Task SavePGDetails(EnquiryDTO enquiry);
        public Task SaveSisterConcern(EnquiryDTO enquiry);
        public Task SaveProjectDetails(EnquiryDTO enquiry);
        public Task SaveSecurityDocuments(EnquiryDTO enquiry);

    }
}
