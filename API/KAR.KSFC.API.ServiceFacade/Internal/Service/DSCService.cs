using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service
{
    public class DscService : IDscService
    {
        private readonly IEntityRepository<TblEmpdscTab> _dscRepository;
        public DscService(IEntityRepository<TblEmpdscTab> dscRepository)
        {
            this._dscRepository = dscRepository;
        }
        public async Task<TblEmpdscTab> AuthenticateDSC(string pubKey, string empId,CancellationToken token)
        {
            var user =await _dscRepository.FirstOrDefaultByExpressionAsync(x => x.DscPubkey == pubKey && x.EmpId == empId && x.DscExpdate >= DateTime.Now,token).ConfigureAwait(false);
            return user;
        }
    }
}
