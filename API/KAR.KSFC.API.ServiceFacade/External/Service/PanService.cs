using KAR.KSFC.API.ServiceFacade.External.Interface;
using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.External.Service
{
    public class PanService : IPanService
    {
        private readonly IEntityRepository<RegduserTab> _regUserRepository;
        private readonly IEntityRepository<Promoter> _promoterRepository;
        private readonly IEntityRepository<UnitInfo1> _unitrRepository;
        private readonly IEntityRepository<OffcCdtab> _Officerepository;

        public PanService(IEntityRepository<RegduserTab> regUserRepository, IEntityRepository<OffcCdtab> Officerepository, IEntityRepository<Promoter> promoterRepository, IEntityRepository<UnitInfo1> unitrRepository)
        {
            _regUserRepository = regUserRepository;
            _promoterRepository = promoterRepository;
            _unitrRepository = unitrRepository;
            _Officerepository = Officerepository;
        }

        public async Task<RegdUserDTO> IsPanAlreadyExist(string panNo, CancellationToken token)
        {
            string branch = String.Empty;
            var user = await _regUserRepository.FirstOrDefaultByExpressionAsync(x => x.UserPan == panNo, token).ConfigureAwait(false);
            var promoter = await _promoterRepository.FirstOrDefaultByExpressionAsync(x => x.PanNo == panNo, token).ConfigureAwait(false);
            var unit = await _unitrRepository.FirstOrDefaultByExpressionAsync(x => x.UtPan == panNo, token).ConfigureAwait(false);

            if (unit != null)
            {
                var branchData = await _Officerepository.FirstOrDefaultByExpressionAsync(a => a.OffcCd == unit.UtOffc, token).ConfigureAwait(false);
                branch = branchData.OffcNam;
            }

            if (user != null)
            {
                return new RegdUserDTO() { Branch = branch, mobile = user.UserMobileno, Pan = user.UserPan };
            }

            if (promoter != null)
            {
                return new RegdUserDTO() { Branch = branch, mobile = promoter.PromMobile, Pan = promoter.PanNo, Name = promoter.PromName };
            }

            if (unit != null)
            {
                return new RegdUserDTO { Pan = unit.UtPan, Branch = branch ?? string.Empty, mobile = "" };
            }
            return null;
        }
    }
}
