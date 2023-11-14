using KAR.KSFC.Components.Common.Dto.AdminModule;
using KAR.KSFC.Components.Common.Dto.Employee;
using KAR.KSFC.Components.Data.Models.DbModels;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.AdminModule
{
    public interface IEmployeeService
    {
        public Task<bool> IsEmployeeNumberUnique(string empNo, CancellationToken token);
        public Task<List<EmployeeDTO>> GetAllEmployees(CancellationToken Token);
        public Task<EmployeeDTO> GetAllEmployeesById(string ticket_num, CancellationToken Token);
        public Task<bool> DeleteEmployeeById(string ticket_num, CancellationToken Token);
        public Task<List<EmployeeDTO>> GetAllEmployeesByFilter(CancellationToken Token, string ticket_num = null, string desg_code = null, string pan_num = null, int? phone = 0);
        public Task<List<TblTrgEmpGrade>> GetEmployeeDesignation(CancellationToken Token);
        public Task<List<TblEmpdesighistTab>> GetEmployeeDesiHistory(string employeeId, CancellationToken Token);
        public Task<List<TblEmpdscTab>> GetEmployeeDSC(string employeeId, CancellationToken Token);
        public Task<List<TblEmploginTab>> GetEmployeeLoginDetails(string employeeId, CancellationToken Token);
        public Task<TblEmploginTab> SaveEmployeeCheckIn(TblEmploginTab Checkout_data, CancellationToken Token);
        public Task<(bool, string)> SaveEmployeeDetails(EmployeeDTO employee, CancellationToken Token);
        public Task<List<OfficeMasterDto>> GetAllOffices(CancellationToken Token);
        public Task<List<ChairMasterDto>> GetAllChairs(string officeId, CancellationToken Token);
        public Task<bool> CheckOut(CheckOutDto dto, CancellationToken Token);
        public Task<bool> Checkin(AssignOfficeDto requestObj, CancellationToken Token);
        public Task<List<AssignOfficeMasterHistoryDto>> GetAllAssignDataUsingEmployeeId(string employeeId, CancellationToken Token);
        public Task<List<ModuleMasterDto>> GetModules(CancellationToken Token);
        public Task<List<RoleMasterDto>> GetRoles(string moduleId, CancellationToken Token);
        public Task<int> AssignRoleAndModule(AssignRoleDto requestObj, CancellationToken Token);
        public Task<bool> RemoveAssignedRole(string empId, string moduleId, string roleId, CancellationToken Token);
        public Task<List<AssignRoleDto>> GetEmployeeRoleDetails(string employeeId, CancellationToken Token);

    }
}
