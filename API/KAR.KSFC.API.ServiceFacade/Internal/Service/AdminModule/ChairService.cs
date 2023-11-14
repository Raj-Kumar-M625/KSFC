using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.AdminModule;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.AdminModule
{  
    public class ChairService : IGenericAdminModuleRepository
    {
        private readonly IEntityRepository<OtpTab> _entityRepository;
        public ChairService(IEntityRepository<OtpTab> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public Task<object> AddAsync(object model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<object> DeleteAsync(object model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<object>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetByIdAsync(object model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<object> UpdateAsync(object model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
