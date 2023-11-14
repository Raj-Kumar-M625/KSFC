using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Dto.Enquiry;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class EnquirySubmissionService : IEnquirySubmissionService
    {
        private readonly IRepository<DistCdtab> _distRepository;
        private readonly IRepository<TlqCdtab> _talukRepository;
        private readonly IRepository<HobCdtab> _hobliRepository;
        private readonly IRepository<VilCdtab> _villageRepository;
        /// <summary>
        /// custome parameters
        /// </summary>
        /// <param name="talukRepository"></param>
        /// <param name="hobliRepository"></param>
        /// <param name="villageRepository"></param>
        /// <param name="distRepository"></param>
        public EnquirySubmissionService(IRepository<TlqCdtab> talukRepository, IRepository<HobCdtab> hobliRepository, IRepository<VilCdtab> villageRepository,
            IRepository<DistCdtab> distRepository)
        {
            this._distRepository = distRepository;
            this._talukRepository = talukRepository;
            this._hobliRepository = hobliRepository;
            this._villageRepository = villageRepository;
        }

        public List<DistCdtab> GetAllDistricts()
        {

            return _distRepository.Query().ToList();

        }

        public List<TlqCdtab> GetTalukByDistCode(int? distCode)
        {

            return _talukRepository.Query(x => x.DistCd == distCode).ToList();

        }

        public List<HobCdtab> GetHobliByTlqCode(int? tlqCode)
        {

            return _hobliRepository.Query(x => x.TlqCd == tlqCode).ToList();

        }

        public List<VilCdtab> GetVillageByTHobliCode(int? hobliCode)
        {

            return _villageRepository.Query(x => x.HobCd == hobliCode).ToList();

        }

        public async Task SaveBasicDetails(EnquiryDTO enquiry)
        {

            if (enquiry.EnquiryId == 0)
            {

            }
            else
            {

            }

        }

        public async Task SavePGDetails(EnquiryDTO enquiry)
        {

            if (enquiry.EnquiryId == 0)
            {

            }
            else
            {

            }

        }

        public async Task SaveSisterConcern(EnquiryDTO enquiry)
        {

            if (enquiry.EnquiryId == 0)
            {

            }
            else
            {

            }

        }

        public async Task SaveProjectDetails(EnquiryDTO enquiry)
        {


            if (enquiry.EnquiryId == 0)
            {

            }
            else
            {

            }


        }

        public async Task SaveSecurityDocuments(EnquiryDTO enquiry)
        {


            if (enquiry.EnquiryId == 0)
            {

            }
            else
            {

            }

        }

    }
}
