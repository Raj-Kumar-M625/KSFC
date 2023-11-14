using KAR.KSFC.Components.Common.Dto.Employee;
using KAR.KSFC.Components.Data.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.AdminModule
{
    public interface IAssignOffice
    {
        public Task<List<EmployeeDesignationHistoryDTO>> GetAllEmployeeBranch(CancellationToken Token);
        public Task<List<EmployeeDesignationHistoryDTO>> GetAllEmployeeBranchByFilter(CancellationToken Token, string ticket_num = null);
        public Task<int> SaveEmployeeCheckIn(TblEmpdesigTab CheckIn_dets, CancellationToken Token);
        public Task<int> SaveEmployeeCheckOut(TblEmpdesigTab Checkout_dets, CancellationToken Token);
        public Task<EmployeeDesignationHistoryDTO> DeleteEmployeeBranchById(string ticket_num, CancellationToken Token);
    }
}
