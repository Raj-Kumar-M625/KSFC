using KAR.KSFC.Components.Common.Dto.AdminModule;
using KAR.KSFC.Components.Common.Dto.Employee;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.UI.Services.IServices;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IHttpClientFactory _clientFactory;

        public EmployeeService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        #region EmployeeMasterCRUD

        public async Task<(bool, string)> CreateOrUpdate(EmployeeDTO employeeDTO)
        {

            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var httpContent = new StringContent(JsonConvert.SerializeObject(employeeDTO), Encoding.UTF8, "application/json");

                var responseHttp = await client.PostAsync($"Employee/Submit", httpContent);
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    return (true, successObj.Message);
                }
                return (false, successObj.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> IsEmployeeNumberUnique(string empNo)
        {
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync($"Employee/IsEmployeeNumberUnique?empNo=" + empNo);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                return successObj.Result;
            }
            return false;
        }
        public async Task<bool> Delete(string employeeNumber)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var responseHttp = await client.GetAsync($"Employee/DeleteEmployeesById?emp_id=" + employeeNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<EmployeeDTO>> GetAllEmployeeDetail()
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var employeeDetails = new List<EmployeeDTO>();
                var responseHttp = await client.GetAsync($"Employee/GetAllEmployees");
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    employeeDetails = JsonConvert.DeserializeObject<IList<EmployeeDTO>>(successObj.Result.ToString());
                }
                return employeeDetails;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IList<EmployeeDTO>> GetEmployeeDetail(string employeeNumber)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var employeeDetails = new List<EmployeeDTO>();
                var responseHttp = await client.GetAsync($"Employee/GetAllEmployeesById?emp_id=" + employeeNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    var result = JsonConvert.DeserializeObject<EmployeeDTO>(successObj.Result.ToString());
                    employeeDetails.Add(result);
                }
                return employeeDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region AssignOffice

        public async Task<List<TblTrgEmpGrade>> GetEmployeeDesignation()
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var empDesignationDetails = new List<TblTrgEmpGrade>();
                var responseHttp = await client.GetAsync($"Employee/GetEmployeeDesignation");
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    empDesignationDetails = JsonConvert.DeserializeObject<IList<TblTrgEmpGrade>>(successObj.Result.ToString());
                }
                return empDesignationDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<ChairMasterDto>> GetChairDetails(string officeId)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var result = new List<ChairMasterDto>();
                var responseHttp = await client.GetAsync($"Employee/GetChairs?offc_id=" + officeId);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    result = JsonConvert.DeserializeObject<IList<ChairMasterDto>>(successObj.Result.ToString());
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<OfficeMasterDto>> GetOfficeDetails()
        {

            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var result = new List<OfficeMasterDto>();
                var responseHttp = await client.GetAsync($"Employee/GetAllOffices");
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    result = JsonConvert.DeserializeObject<IList<OfficeMasterDto>>(successObj.Result.ToString());
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SubmitAssignment(AssignOfficeDto requestObj)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var httpContent = new StringContent(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, "application/json");
                var responseHttp = await client.PostAsync($"Employee/Checkin", httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<AssignOfficeMasterHistoryDto>> GetAllAssignDataUsingEmployeeId(string empId)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var result = new List<AssignOfficeMasterHistoryDto>();
                var responseHttp = await client.GetAsync($"Employee/GetAllAssignDataUsingEmployeeId?employeeId=" + empId);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    result = JsonConvert.DeserializeObject<IList<AssignOfficeMasterHistoryDto>>(successObj.Result.ToString());
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CheckOut(CheckOutDto requestObj)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var httpContent = new StringContent(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, "application/json");
                var responseHttp = await client.PostAsync($"Employee/CheckOut", httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ModuleAndRoleAssignment
        public async Task<IList<ModuleMasterDto>> GetModules()
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var result = new List<ModuleMasterDto>();
                var responseHttp = await client.GetAsync($"Employee/GetModules");
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    result = JsonConvert.DeserializeObject<IList<ModuleMasterDto>>(successObj.Result.ToString());
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<RoleMasterDto>> GetRolesUsingModuleId(string moduleId)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var result = new List<RoleMasterDto>();
                var responseHttp = await client.GetAsync($"Employee/GetRoles?moduleId=" + moduleId);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    result = JsonConvert.DeserializeObject<IList<RoleMasterDto>>(successObj.Result.ToString());
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<AssignRoleDto>> GetEmployeeRoleDetails(string employeeId)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var result = new List<AssignRoleDto>();
                var responseHttp = await client.GetAsync($"Employee/GetEmployeeRoleDetails?employeeId=" + employeeId);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    result = JsonConvert.DeserializeObject<IList<AssignRoleDto>>(successObj.Result.ToString());
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResultResponse> AssignRole(AssignRoleDto obj)
        {
            var response = new ApiResultResponse();
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var httpContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                var responseHttp = await client.PostAsync($"Employee/AssignRole", httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);

                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteRole(AssignRoleDto requestObj)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var responseHttp = await client.GetAsync($"Employee/RemoveAssignedRole?empId=" + requestObj.EmployeeNumber +

                    "&moduleId=" + requestObj.ModuleId + "&roleId=" + requestObj.RoleId);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
