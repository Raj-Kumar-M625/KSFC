using Application.Interface.Persistence.Generic;
using Domain.Payment;
using Domain.TDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.TDS
{
    public interface ITdsStatusRepository: IGenericRepository<TdsStatus>
    {
        Task<int> UpdateStatusAsync(IEnumerable<TdsStatus> statusList);
    }
}
