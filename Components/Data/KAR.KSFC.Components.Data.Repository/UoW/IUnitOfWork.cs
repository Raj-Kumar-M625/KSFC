using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Repository.UoW
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken token);
    }
}
