using KAR.KSFC.Components.Common.Dto.AdminModule;
using KAR.KSFC.Components.Common.Dto.Employee;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.Components.Data.Models.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices
{
    public interface IEmployeeService
    {
        // create employee
        public Task<bool> IsEmployeeNumberUnique(string empNo);
        Task<IList<EmployeeDTO>> GetEmployeeDetail(string employeeNumber);
        Task<IList<EmployeeDTO>> GetAllEmployeeDetail();
        Task<(bool,string)> CreateOrUpdate(EmployeeDTO employeeDTO);
        Task<bool> Delete(string employeeNumber);
        Task<List<TblTrgEmpGrade>> GetEmployeeDesignation();


        // Assign office
        Task<IList<ChairMasterDto>> GetChairDetails(string officeId);
        Task<IList<OfficeMasterDto>> GetOfficeDetails();
        Task<bool> SubmitAssignment(AssignOfficeDto requestObj);
        Task<List<AssignOfficeMasterHistoryDto>> GetAllAssignDataUsingEmployeeId(string empId);
        Task<bool> CheckOut(CheckOutDto dto);


        // Assign role 
        Task<IList<ModuleMasterDto>> GetModules();
        Task<IList<RoleMasterDto>> GetRolesUsingModuleId(string moduleID);
        Task<IList<AssignRoleDto>> GetEmployeeRoleDetails(string empId);
        Task<ApiResultResponse> AssignRole(AssignRoleDto obj);
        Task<bool> DeleteRole(AssignRoleDto dto);


    }
}
