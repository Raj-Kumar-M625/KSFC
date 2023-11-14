using Application.Interface.Persistence.Generic;
using Domain.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Master
{
    public interface ICommonMasterRepository : IGenericRepository<CommonMaster>
    {
        Task<ICollection<CommonMaster>> GetCommoMasterValues(string codeType);
        Task<ICollection<CommonMaster>> GetCommoMasterValues();
    }
}
