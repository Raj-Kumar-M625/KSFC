using Application.Interface.Persistence.Generic;
using Domain.Adjustment;
using Domain.Bill;
using Domain.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Adjustment
{
    public interface IAdjustmentRepository : IGenericRepository<Adjustments>
    {
        IQueryable<Adjustments> GetAdjustmentList(int VendorId);
        Task<Adjustments> AddAdjustments(Adjustments entity);
        Task<Adjustments> GetAdjustmentsById(int Id);
        Task<AdjustmentStatus> AddAdjustmentStatus(AdjustmentStatus entity);
    }
}
