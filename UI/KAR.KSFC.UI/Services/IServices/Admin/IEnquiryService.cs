using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin
{
    public interface IEnquiryService
    {
        Task<List<EnquirySummary>> GetAllEnquiriesForAdmin();
    }
}
