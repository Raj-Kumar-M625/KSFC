using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.AdminModule
{
    /// <summary>
    /// Generic Interfae for Admin Module
    /// </summary>
    public interface IGenericAdminModuleRepository
    {
        /// <summary>
        /// Add data in repository
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<object> AddAsync(object model, CancellationToken cancellationToken);


        /// <summary>
        /// Update Data in Repository
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<object> UpdateAsync(object model, CancellationToken cancellationToken);

        /// <summary>
        /// SoftDelete in Repository
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<object> DeleteAsync(object Id, CancellationToken cancellationToken);

        /// <summary>
        /// Get All Data from Repository
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<object>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Get Data by ID from Repository
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<object> GetByIdAsync(object Id, CancellationToken cancellationToken);

    }
}
