using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.AdminModule;
using KAR.KSFC.Components.Common.Dto.AdminModule;
using KAR.KSFC.Components.Common.Dto.Employee;
using KAR.KSFC.Components.Common.Security;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.AdminModule
{
    public class EmployeeService : IEmployeeService
    {

        // CRUD - Employee 
        private readonly IEntityRepository<TblEmpdesighistTab> _desgHistory;
        private readonly IEntityRepository<TblEmpdesigTab> _empDesignation;
        private readonly IEntityRepository<TblEmpdscTab> _empDSC;
        private readonly IEntityRepository<TblEmploginTab> _empLogin;
        private readonly IEntityRepository<TblTrgEmpGrade> _empGrade;

        private readonly IEntityRepository<TblTrgEmployee> _empContext;


        // Assign Office to an Employeee
        private readonly IEntityRepository<OffcCdtab> _officeDtlContext;
        private readonly IEntityRepository<TblChairCdtab> _chairDtlContext;
        private readonly IEntityRepository<TblEmpchairhistDet> _empChairHistoryContext;

        // Assign Role and Module for an Employee
        private readonly IEntityRepository<TblModuleCdtab> _moduleContext;
        private readonly IEntityRepository<TblRoleCdtab> _roleContext;
        private readonly IEntityRepository<TblEmprolehistDet> _roleDtlContext;


        // UNIT OF WORK - using for DDL operations eg.- Save ,Update ,Delete
        private readonly IUnitOfWork _unitOfWorkService;

        // USER - info contexts
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;


        public EmployeeService(
            IEntityRepository<RegduserTab> regdUserRepository,
            IEntityRepository<TblTrgEmpGrade> empGrade,
            IEntityRepository<TblTrgEmployee> employeeList,
            IEntityRepository<TblEmpdesighistTab> desgHistory,
            IEntityRepository<TblEmpdesigTab> empDesignation,
            IEntityRepository<TblEmpdscTab> empDSC,
            IEntityRepository<TblEmploginTab> empLogin,
            UserInfo UserInfo,
            IMapper Mapper, IUnitOfWork unitOfWork,
            IEntityRepository<OffcCdtab> officeDetails,
            IEntityRepository<TblChairCdtab> chairDtlContext,
            IEntityRepository<TblEmpchairhistDet> empChairHistoryDetails,
            IEntityRepository<TblRoleCdtab> roleContext,
            IEntityRepository<TblModuleCdtab> moduleContext,
            IEntityRepository<TblEmprolehistDet> roleDtlContext
            )
        {
            _empContext = employeeList;
            _desgHistory = desgHistory;
            _empDesignation = empDesignation;
            _empDSC = empDSC;
            _empLogin = empLogin;
            _userInfo = UserInfo;
            _mapper = Mapper;
            _unitOfWorkService = unitOfWork;
            _empGrade = empGrade;
            _chairDtlContext = chairDtlContext;
            _officeDtlContext = officeDetails;
            _empChairHistoryContext = empChairHistoryDetails;
            _roleContext = roleContext;
            _moduleContext = moduleContext;
            _roleDtlContext = roleDtlContext;
        }

        public async Task<bool> DeleteEmployeeById(string ticket_num, CancellationToken Token)
        {
            var Emp_details = await _empContext.FirstOrDefaultByExpressionAsync(x => x.TeyTicketNum == ticket_num, Token);

            Emp_details.IsActive = false;
            Emp_details.IsDeleted = true;
            Emp_details.ModifiedBy = _userInfo.Name;
            Emp_details.ModifiedDate = DateTime.Now;

            await _empContext.UpdateAsync(Emp_details, Token).ConfigureAwait(false);
            await _unitOfWorkService.CommitAsync(Token);
            return true;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployees(CancellationToken Token)
        {
            var Emp_details = await _empContext.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, Token);

            var Lis_Employee = _mapper.Map<List<EmployeeDTO>>(Emp_details);

            return Lis_Employee;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployeesByFilter(CancellationToken Token, string ticket_num = null, string desg_code = null, string pan_num = null, int? phone = 0)
        {
            var emp_details = await _empContext.FindByExpressionAsync(x => (x.TeyTicketNum == ticket_num || x.TeyGradeCode == desg_code || x.TeyPanNum == pan_num || x.EmpMobileNo == phone.ToString() || x.TeyPermanentPhone == phone || x.TeyPresentPhone == phone) && (x.IsActive == true), Token);

            var lis_Employee = _mapper.Map<List<EmployeeDTO>>(emp_details);

            return lis_Employee;
        }

        public async Task<bool> IsEmployeeNumberUnique(string empNo, CancellationToken token)
        {
            var data = await _empContext.FirstOrDefaultByExpressionAsync(x => x.TeyTicketNum == empNo && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (data != null)
                return false;

            return true;
        }

        public async Task<EmployeeDTO> GetAllEmployeesById(string ticket_num, CancellationToken Token)
        {
            var empObj = new EmployeeDTO();
            var emp_details = await _empContext.FirstOrDefaultByExpressionAsync(x => x.TeyTicketNum == ticket_num && x.IsActive == true && x.IsDeleted == false, Token);
            var emp_designation = await _empDesignation.FirstOrDefaultByExpressionAsync(x => x.EmpId == ticket_num, Token);
            var isCheckIn = await _empChairHistoryContext.FirstOrDefaultByExpressionAsync(x => x.EmpId == ticket_num, Token);

            if (emp_details != null)
            {
                empObj.TeyTicketNum = emp_details?.TeyTicketNum;
                empObj.TeyName = emp_details?.TeyName;
                empObj.TeyPanNum = emp_details?.TeyPanNum;
                empObj.TeyPresentAddress1 = emp_details?.TeyPresentAddress1;
                if (emp_details?.TeyBirthDate != null)
                    empObj.TeyBirthDate = (DateTime)emp_details?.TeyBirthDate;
                empObj.TeyLastdatePromotion = emp_details?.TeyLastdatePromotion;
                empObj.TeyPresentCity = emp_details?.TeyPresentCity;
                empObj.TeyPresentZip = emp_details?.TeyPresentZip;
                empObj.TeySex = emp_details?.TeySex;
                empObj.EmpMobileNo = emp_details.EmpMobileNo;
                empObj.TeyPresentEmail = emp_details?.TeyPresentEmail;
                empObj.IsCheckedIn = isCheckIn != null;
                if (emp_designation != null)
                {
                    empObj.EmpDesignation.IcDesigCode = emp_designation.IcDesigCode;
                    empObj.EmpDesignation.IcDate = emp_designation.IcDate;
                    empObj.EmpDesignation.SubstDesigCode = emp_designation.SubstDesigCode;
                    empObj.EmpDesignation.SubstDate = emp_designation.SubstDate;
                    empObj.EmpDesignation.PpDesignCode = emp_designation.PpDesignCode;
                    empObj.EmpDesignation.PpDate = emp_designation.PpDate;
                }
                else
                {
                    empObj.EmpDesignation = new EmployeeDesignationDTO();
                }


            }
            else
            {
                empObj = new EmployeeDTO();
            }

            return empObj;
        }

        public async Task<List<TblTrgEmpGrade>> GetEmployeeDesignation(CancellationToken Token)
        {
            var empDesg_details = await _empGrade.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, Token);

            var lis_desig = _mapper.Map<List<TblTrgEmpGrade>>(empDesg_details);

            return lis_desig;


        }

        public async Task<List<TblEmpdesighistTab>> GetEmployeeDesiHistory(string employeeId, CancellationToken Token)
        {
            var empDesg_details = await _empDesignation.FindByExpressionAsync(x => x.EmpId == employeeId, Token);

            var lis_desigHis = _mapper.Map<List<TblEmpdesighistTab>>(empDesg_details);
            return lis_desigHis;
        }

        public async Task<List<TblEmpdscTab>> GetEmployeeDSC(string employeeId, CancellationToken Token)
        {
            var emp_details = await _empDesignation.FindByExpressionAsync(x => x.IcDesigCode == employeeId, Token);

            var lis_dsc = _mapper.Map<List<TblEmpdscTab>>(emp_details);
            return lis_dsc;
        }

        public async Task<List<TblEmploginTab>> GetEmployeeLoginDetails(string employeeId, CancellationToken Token)
        {
            var emp_details = await _empDesignation.FindByExpressionAsync(x => x.IcDesigCode == employeeId, Token);

            var lis_empLogin = _mapper.Map<List<TblEmploginTab>>(emp_details);
            return lis_empLogin;
        }

        public async Task<TblEmploginTab> SaveEmployeeCheckIn(TblEmploginTab Checkout_data, CancellationToken Token)
        {
            Checkout_data.IsActive = true;
            Checkout_data.IsDeleted = false;
            Checkout_data.CreatedBy = _userInfo.Name;
            Checkout_data.CreatedDate = DateTime.Now;

            var result = await _empLogin.AddAsync(Checkout_data, Token);
            return _mapper.Map<TblEmploginTab>(result);
        }

        public async Task<(bool, string)> SaveEmployeeDetails(EmployeeDTO employee, CancellationToken Token)
        {
            TblEmpdscTab registrationDetails = new TblEmpdscTab();
            var checkEmployeeDtls = await _empContext.FirstOrDefaultByExpressionAsync(a => a.TeyTicketNum == employee.TeyTicketNum && a.IsActive == true && a.IsDeleted == false, Token);
            var checkEmpDesignationDtls = await _empDesignation.FirstOrDefaultByExpressionAsync(a => a.EmpId == employee.TeyTicketNum && a.IsActive == true && a.IsDeleted == false, Token);


            if (checkEmployeeDtls == null && checkEmpDesignationDtls == null)
            {
                var validation = await GetEmployeeValidation(employee, Token);
                if (validation.Length > 0)
                {
                    return (false, validation);
                }
                var employeeDescription = await _empDSC.FirstOrDefaultByExpressionAsync(a => a.EmpId == employee.TeyTicketNum && a.IsActive == true && a.IsDeleted == false, Token);

                var employeeDetails = _mapper.Map<TblTrgEmployee>(employee);

                TblEmpdesigTab designations = new TblEmpdesigTab();

                employeeDetails.IsActive = true;
                employeeDetails.IsDeleted = false;
                employeeDetails.CreatedBy = _userInfo.UserId;
                employeeDetails.CreatedDate = System.DateTime.Now;
                employeeDetails.UniqueId = System.Guid.NewGuid().ToString();
                employeeDetails.TeyUnitCode = "KSFC";
                employeeDetails.TeyCurrentUnit = "KSFC";
               
                designations.EmpId = employee.TeyTicketNum;
                designations.SubstDesigCode = employee.EmpDesignation.SubstDesigCode;
                designations.SubstDate = employee.EmpDesignation.SubstDate;
                designations.PpDesignCode = employee.EmpDesignation.PpDesignCode;
                designations.PpDate = employee.EmpDesignation.PpDate;
                designations.IcDate = employee.EmpDesignation.IcDate;
                designations.IcDesigCode = employee.EmpDesignation.IcDesigCode;
                designations.CreatedBy = _userInfo.UserId;
                designations.CreatedDate = DateTime.Now;
                designations.IsActive = true;
                designations.IsDeleted = false;

                registrationDetails.EmpId = employee.TeyTicketNum;
                registrationDetails.EmpPswd = SecurityHandler.Base64Encode("test1234");
                registrationDetails.IsActive = true;
                registrationDetails.IsDeleted = false;

                await _empContext.AddAsync(employeeDetails, Token);
                await _empDesignation.AddAsync(designations, Token);
                if (employeeDescription == null)
                {
                    registrationDetails.IsPswdChng = false;
                    await _empDSC.AddAsync(registrationDetails, Token);
                }
            }
            else
            {
                var getUserLoginDetails = await _empDSC
                    .FirstOrDefaultByExpressionAsync(x => x.EmpId == checkEmployeeDtls.TeyTicketNum, Token);

                checkEmployeeDtls.TeyName = employee.TeyName.ToUpper();
                checkEmployeeDtls.TeySex = employee.TeySex;
                checkEmployeeDtls.TeyBirthDate = employee.TeyBirthDate;
                checkEmployeeDtls.TeyPresentAddress1 = employee.TeyPresentAddress1.ToUpper();
                checkEmployeeDtls.TeyPresentCity = employee.TeyPresentCity.ToUpper();
                checkEmployeeDtls.TeyPresentZip = employee.TeyPresentZip.ToUpper();
                checkEmployeeDtls.TeyPanNum = employee.TeyPanNum.ToUpper();
                checkEmployeeDtls.EmpMobileNo = employee.EmpMobileNo.ToString();
                checkEmployeeDtls.TeyPresentEmail = employee.TeyPresentEmail.ToLower();
                checkEmployeeDtls.TeyLastdatePromotion = employee.TeyLastdatePromotion;
                checkEmployeeDtls.ModifiedBy = _userInfo.UserId;
                checkEmployeeDtls.ModifiedDate = System.DateTime.UtcNow;
                checkEmployeeDtls.TeyUnitCode = "KSFC";
                checkEmployeeDtls.TeyCurrentUnit = "KSFC";
                checkEmployeeDtls.IsActive = true;
                checkEmployeeDtls.IsDeleted = false;

                if (getUserLoginDetails != null)
                {
                    getUserLoginDetails.ModifiedBy = _userInfo.UserId;
                    getUserLoginDetails.ModifiedDate = DateTime.UtcNow;
                    await _empDSC.UpdateAsync(getUserLoginDetails, Token);
                }

                checkEmpDesignationDtls.ModifiedBy = _userInfo.UserId;
                checkEmpDesignationDtls.ModifiedDate = DateTime.UtcNow;
                checkEmpDesignationDtls.SubstDesigCode = employee.EmpDesignation.SubstDesigCode;
                checkEmpDesignationDtls.SubstDate = employee.EmpDesignation.SubstDate;
                checkEmpDesignationDtls.PpDesignCode = employee.EmpDesignation.PpDesignCode;
                checkEmpDesignationDtls.PpDate = employee.EmpDesignation.PpDate;
                checkEmpDesignationDtls.IcDate = employee.EmpDesignation.IcDate;
                checkEmpDesignationDtls.IcDesigCode = employee.EmpDesignation.IcDesigCode;

                await _empContext.UpdateAsync(checkEmployeeDtls, Token);
                await _empDesignation.UpdateAsync(checkEmpDesignationDtls, Token);
            }

            await _unitOfWorkService.CommitAsync(Token);

            return (true, "New Employee addedd sucessfully");
        }

        #region AssignOffice

        public async Task<List<OfficeMasterDto>> GetAllOffices(CancellationToken Token)
        {
            var result = await _officeDtlContext.FindByExpressionAsync(x => x.OffcCd == x.OffcCd, Token);
            var resultObj = new List<OfficeMasterDto>();

            foreach (var item in result)
            {
                resultObj.Add(new OfficeMasterDto()
                {
                    OffcCd = item.OffcCd,
                    OffcNam = item.OffcNam,
                });

            }
            return resultObj;
        }

        public async Task<List<ChairMasterDto>> GetAllChairs(string officeId, CancellationToken Token)
        {
            var result = await _chairDtlContext.FindByExpressionAsync(x => x.OffcCd == Convert.ToByte(officeId), Token);
            var resultObj = new List<ChairMasterDto>();
            foreach (var item in result)
            {
                resultObj.Add(new ChairMasterDto()
                {
                    OfficeCode = item.OffcCd,
                    Id = item.ChairId,
                    Description = item.ChairDesc,
                    Code = item.ChairCode
                });

            }
            return resultObj;
        }

        public async Task<bool> Checkin(AssignOfficeDto requestObj, CancellationToken Token)
        {

            var checkIfEmployeeIDisAlreadyAvailable = await _empChairHistoryContext.FindByExpressionAsync(x => x.EmpId == requestObj.TeyTicketNum
            && x.OffcCd == Convert.ToByte(requestObj.OfficeId)
            && x.ChairCode == Convert.ToInt32(requestObj.ChairId)
            && x.TgesCode == requestObj.OpsDesignationId
            && x.IsActive == true
            && x.IsDeleted == false,
            Token);


            if (checkIfEmployeeIDisAlreadyAvailable.Count > 0)
            {
                foreach (var item in checkIfEmployeeIDisAlreadyAvailable)
                {

                    item.IsActive = false;
                    item.IsDeleted = true;
                    item.ModifiedBy = _userInfo.UserId;
                    item.ModifiedDate = DateTime.UtcNow;

                }

                var saveObj = new TblEmpchairhistDet()
                {
                    EmpId = requestObj.TeyTicketNum,
                    OffcCd = Convert.ToByte(requestObj.OfficeId),
                    ChairCode = Convert.ToInt32(requestObj.ChairId),
                    FromDate = DateTime.Parse(requestObj.CommencementDate),
                    TgesCode = requestObj.OpsDesignationId,
                    IsActive = true,
                    CreatedBy = _userInfo.UserId,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _empChairHistoryContext.UpdateAsync(checkIfEmployeeIDisAlreadyAvailable, Token);
                await _empChairHistoryContext.AddAsync(saveObj, Token);

            }
            else
            {
                var saveObj = new TblEmpchairhistDet()
                {
                    EmpId = requestObj.TeyTicketNum,
                    OffcCd = Convert.ToByte(requestObj.OfficeId),
                    ChairCode = Convert.ToInt32(requestObj.ChairId),
                    FromDate = DateTime.Parse(requestObj.CommencementDate),
                    TgesCode = requestObj.OpsDesignationId,
                    IsActive = true,
                    CreatedBy = _userInfo.UserId,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _empChairHistoryContext.AddAsync(saveObj, Token);

            }

            var saveResult = await _unitOfWorkService.CommitAsync(Token);

            if (saveResult > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<List<AssignOfficeMasterHistoryDto>> GetAllAssignDataUsingEmployeeId(string employeeId, CancellationToken Token)
        {
            // employee assign master 
            var empOfficeDetails = await _empChairHistoryContext.FindByExpressionAsync(x => x.EmpId == employeeId && x.IsActive == true
            && x.IsDeleted == false, Token);

            // this is all offices 
            var AllOffices = await _officeDtlContext.FindByExpressionAsync(x => x.OffcCd == x.OffcCd, Token);
            var AllChair = await _chairDtlContext.FindByExpressionAsync(x => x.ChairCode == x.ChairCode, Token);
            var AllDesignations = await _empGrade.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, Token);

            var testObjOne = empOfficeDetails.Join(AllOffices,
                            emp => emp.OffcCd,
                            off => off.OffcCd,
                            (emp, off) => new
                            {
                                EmployeeId = emp.EmpId,
                                OfficeCode = off.OffcCd,
                                OfficeName = off.OffcNam,
                                ChairCode = emp.ChairCode,
                                OpDesigCode = emp.TgesCode,
                                FromDate = emp.FromDate

                            }).Join(AllChair, m => m.ChairCode, n => n.ChairCode, (m, n) => new
                            {
                                EmployeeId = m.EmployeeId,
                                OfficeCode = m.OfficeCode,
                                OfficeName = m.OfficeName,
                                ChairCode = m.ChairCode,
                                ChairName = n.ChairDesc,
                                OpDesigCode = m.OpDesigCode,
                                FromDate = m.FromDate
                            }).Join(AllDesignations, a => a.OpDesigCode, b => b.TgesCode, (a, b) => new
                            {
                                EmployeeId = a.EmployeeId,
                                OfficeCode = a.OfficeCode,
                                OfficeName = a.OfficeName,
                                ChairCode = a.ChairCode,
                                ChairName = a.ChairName,
                                OpDesigCode = b.TgesCode,
                                OpDesigName = b.TgesDesc,
                                FromDate = a.FromDate
                            }).ToList();

            var responseObj = new List<AssignOfficeMasterHistoryDto>();

            if (testObjOne.Count > 0)
            {
                foreach (var item in testObjOne)
                {
                    responseObj.Add(new AssignOfficeMasterHistoryDto()
                    {
                        EmployeeId = item.EmployeeId,
                        OfficeCode = item.OfficeCode.ToString(),
                        OfficeName = item.OfficeName,
                        ChairCode = item.ChairCode.ToString(),
                        ChairName = item.ChairName,
                        OpDesigCode = item.OpDesigCode,
                        OpDesigName = item.OpDesigName,
                        FromDate = item.FromDate.ToString(),
                    });
                }
            }

            return responseObj;

        }

        public async Task<bool> CheckOut(CheckOutDto dto, CancellationToken Token)
        {
            var getResult = await _empChairHistoryContext.FindByExpressionAsync(x => x.EmpId == dto.employeeId && x.ChairCode == Convert.ToInt32(dto.chairCode) && x.OffcCd == Convert.ToInt32(dto.officeId)

                && x.TgesCode == dto.opsDesignationId
            , Token);

            if (getResult.Count > 0)
            {
                foreach (var item in getResult)
                {
                    item.IsActive = false;
                    item.IsDeleted = true;
                    item.ModifiedBy = _userInfo.UserId;
                    item.ModifiedDate = DateTime.Now;
                }

                await _empChairHistoryContext.UpdateAsync(getResult, Token);

                await _unitOfWorkService.CommitAsync(Token);

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region AssignModuleAndRole
        public async Task<List<ModuleMasterDto>> GetModules(CancellationToken Token)
        {
            var response = new List<ModuleMasterDto>();
            var modules = await _moduleContext.FindByExpressionAsync(x => x.ModuleId == x.ModuleId && x.IsDeleted == false && x.IsActive == true, Token);
            if (modules.Count > 0)
            {
                foreach (var module in modules)
                {
                    response.Add(new ModuleMasterDto()
                    {
                        Id = module.ModuleId,
                        Description = module.ModuleDesc
                    });

                }
            }
            return response;
        }

        public async Task<List<RoleMasterDto>> GetRoles(string moduleId, CancellationToken Token)
        {
            var response = new List<RoleMasterDto>();
            var roles = await _roleContext.FindByExpressionAsync(x => x.ModuleId == Convert.ToInt32(moduleId) && x.IsActive == true && x.IsDeleted == false, Token);

            if (roles.Count > 0)
            {
                foreach (var role in roles)
                {
                    response.Add(new RoleMasterDto()
                    {
                        Id = role.RoleId,
                        Description = role.RoleDesc,
                    });
                }
            }
            return response;
        }

        public async Task<int> AssignRoleAndModule(AssignRoleDto requestObj, CancellationToken Token)
        {
            var data = await _roleDtlContext.FindByExpressionAsync(x => x.EmpId == requestObj.EmployeeNumber
            && x.RoleId == Convert.ToInt32(requestObj.RoleId) && x.IsActive == true && x.IsDeleted == false, Token);
            if (data.Count > 0)
            {
                return 1;
            }
            else
            {
                var saveObj = new TblEmprolehistDet();
                saveObj.IsActive = true;
                saveObj.IsDeleted = false;
                saveObj.CreatedBy = _userInfo.UserId;
                saveObj.CreatedDate = DateTime.Now;
                saveObj.EmpId = requestObj.EmployeeNumber;
                saveObj.RoleId = Convert.ToInt32(requestObj.RoleId);

                await _roleDtlContext.AddAsync(saveObj, Token);
                await _unitOfWorkService.CommitAsync(Token);
            }

            return 0;
        }

        public async Task<bool> RemoveAssignedRole(string empId, string moduleId, string roleId, CancellationToken Token)
        {
            var data = await _roleDtlContext.FindByExpressionAsync(x => x.EmpId == empId
            && x.RoleId == Convert.ToInt32(roleId) && x.IsActive == true && x.IsDeleted == false, Token);
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    item.IsActive = false;
                    item.IsDeleted = true;
                    item.ModifiedBy = _userInfo.UserId;
                    item.ModifiedDate = DateTime.Now;

                }

                await _roleDtlContext.UpdateAsync(data, Token);
                await _unitOfWorkService.CommitAsync(Token);
                return true;

            }
            return false;
        }

        public async Task<List<AssignRoleDto>> GetEmployeeRoleDetails(string employeeId, CancellationToken Token)
        {
            var empData = await _empContext.FindByExpressionAsync(x => x.TeyTicketNum == employeeId && x.IsActive == true && x.IsDeleted == false, Token);

            var roleData = await _roleDtlContext.FindByExpressionAsync(x => x.EmpId == employeeId && x.IsActive == true && x.IsDeleted == false, Token);

            var modules = await _moduleContext.FindByExpressionAsync(x => x.ModuleId == x.ModuleId && x.IsActive == true && x.IsDeleted == false, Token);

            var roles = await _roleContext.FindByExpressionAsync(x => x.RoleId == x.RoleId && x.IsActive == true && x.IsDeleted == false, Token);

            var data = empData.Join(roleData,
                        emp => emp.TeyTicketNum,
                        roleDtl => roleDtl.EmpId,
                        (emp, roleDtl) => new
                        {
                            EmployeeId = emp.TeyTicketNum.ToString(),
                            EmployeeName = emp.TeyName,
                            Mobile = emp.EmpMobileNo,
                            Email = emp.TeyPresentEmail,
                            RoleId = roleDtl.RoleId,
                        }).Join(roles,
                            r => r.RoleId,
                            d => d.RoleId,
                            (r, d) => new
                            {
                                EmployeeId = r.EmployeeId,
                                EmployeeName = r.EmployeeName,
                                Mobile = r.Mobile,
                                Email = r.Email,
                                RoleId = r.RoleId,
                                RoleDsc = d.RoleDesc,
                                ModuleId = d.ModuleId
                            }).Join(modules,
                                r => r.ModuleId,
                                m => m.ModuleId, (r, m) => new
                                {
                                    EmployeeId = r.EmployeeId,
                                    EmployeeName = r.EmployeeName,
                                    Mobile = r.Mobile,
                                    Email = r.Email,
                                    RoleId = r.RoleId,
                                    RoleDsc = r.RoleDsc,
                                    ModuleDesc = m.ModuleDesc,
                                    ModuleId = m.ModuleId
                                }).ToList();
            var result = new List<AssignRoleDto>();

            if (data.Count > 0)
            {
                foreach (var i in data)
                {
                    result.Add(new AssignRoleDto()
                    {
                        EmployeeNumber = i.EmployeeId,
                        EmployeeName = i.EmployeeName,
                        ModuleId = i.ModuleId.ToString(),
                        ModuleName = i.ModuleDesc,
                        RoleId = i.RoleId.ToString(),
                        RoleName = i.RoleDsc
                    });

                }

            }

            return result;
        }

        private async Task<string> GetEmployeeValidation(EmployeeDTO employee, CancellationToken Token)
        {
            StringBuilder returnaDta = new StringBuilder();
            var data = await _empContext.FindByExpressionAsync(a => true == true , Token);
            foreach (var item in data)
            {
                if (item.EmpMobileNo == employee.EmpMobileNo && (!returnaDta.ToString().Contains("Mobile")))
                {
                    returnaDta.Append($"Mobile,");
                }
                if (item.TeyPanNum == employee.TeyPanNum && (!returnaDta.ToString().Contains("Pan")))
                {
                    returnaDta.Append($"Pan,");
                }
                if (item.TeyPresentEmail == employee.TeyPresentEmail && (!returnaDta.ToString().Contains("Email")))
                {
                    returnaDta.Append($"Email,");
                }
            }
            return returnaDta.ToString();
        }

        #endregion


    }


}
