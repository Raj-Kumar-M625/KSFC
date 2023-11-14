using Application.Interface.Persistence.Generic;
using Domain.GSTTDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.GSTTDS
{
   

    public interface IGsttdsStatusRepository : IGenericRepository<GsttdsStatus>
    {
        IList<GsttdsStatus> GetGSTTDSStatus(List<int> ids);
        Task<bool> UpdateGSTTDSStatus(GsttdsStatus item);


    }
}
