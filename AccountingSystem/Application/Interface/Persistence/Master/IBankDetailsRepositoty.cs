using Application.DTOs.Master;
using Application.Interface.Persistence.Generic;
using Domain.Master;
using Domain.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Master
{
    public  interface IBankDetailsRepositoty :IGenericRepository<BankDetails>
    {
        Task<ICollection<BranchMaster>> GetBankDetailsList();
        Task<List<BranchMaster>> GetBranchDetails(int Id);
        
    }
}
