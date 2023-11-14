using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class DocumentsDTO
    {
        public GenDocsDTO GenDocs { get; set; }
        public TechnicalDocsDTO TechnicalDocs { get; set; }
        public FinancialDocsDTO FinancialDocs { get; set; }
        public LegalDocsDTO LegalDocs { get; set; }
    }

    public class GenDocsDTO
    {
        public IFormFile ProjectReport { get; set; }
        public IFormFile ResidenceProof { get; set; }
    }

    public class TechnicalDocsDTO
    {
        public IFormFile ApprovedBuildingPlans { get; set; }
        public IFormFile ExistingMachinAndBuildings { get; set; }
    }

    public class FinancialDocsDTO
    {
        public IFormFile AuditedBalenceSheet { get; set; }
        public IFormFile Incometax { get; set; }
    }

    public class LegalDocsDTO
    {
        public IFormFile LandBuildingDocs { get; set; }
        public IFormFile LeaseAgreement { get; set; }
    }
}
