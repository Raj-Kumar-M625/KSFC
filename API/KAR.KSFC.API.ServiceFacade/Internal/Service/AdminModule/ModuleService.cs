using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.AdminModule;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.AdminModule
{
    public class ModuleService : IGenericAdminModuleRepository
    {
        private readonly IEntityRepository<TblModuleCdtab> _entityRepository;
        public ModuleService(IEntityRepository<TblModuleCdtab> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<object> AddAsync(object model, CancellationToken cancellationToken)
        {
           return await _entityRepository.AddAsync((TblModuleCdtab)model, cancellationToken).ConfigureAwait(false);
        }

        public Task<object> DeleteAsync(object Id, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }

        public Task<List<object>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetByIdAsync(object Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<object> UpdateAsync(object model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
