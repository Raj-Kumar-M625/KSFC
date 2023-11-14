using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.AdminModule;
using KAR.KSFC.Components.Common.Dto.Employee;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.AdminModule
{
    public class AssignOfficeService : IAssignOffice
    {
        private readonly IEntityRepository<TblEmploginTab> _assignofficeService;
        private readonly IEntityRepository<TblEmpdesigTab> _desgService;
        private readonly IUnitOfWork _unitOfWorkService;
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        private readonly ITokenService _tokenService;


        public AssignOfficeService(IEntityRepository<TblEmploginTab> assignofficeService, IEntityRepository<TblEmpdesigTab> desgService, IUnitOfWork unitOfWorkService, IMapper smsService, ITokenService tokenService)
        {
            _assignofficeService = assignofficeService;
            _desgService = desgService;
            _unitOfWorkService = unitOfWorkService;
            _tokenService = tokenService;
        }

        public async Task<List<EmployeeDesignationHistoryDTO>> GetAllEmployeeBranch(CancellationToken Token)
        {
            List<EmployeeDesignationHistoryDTO> lis_dsc = new();
            var emp_details = await _desgService.FindByExpressionAsync(x => x.EmpId == x.EmpId, Token);

            lis_dsc = _mapper.Map<List<EmployeeDesignationHistoryDTO>>(emp_details);
            return lis_dsc;
        }

        public async Task<List<EmployeeDesignationHistoryDTO>> GetAllEmployeeBranchByFilter(CancellationToken Token, string ticket_num = null)
        {
            List<EmployeeDesignationHistoryDTO> lis_dsc = new();
            var emp_details = await _desgService.FindByExpressionAsync(x => x.EmpId == ticket_num, Token);

            lis_dsc = _mapper.Map<List<EmployeeDesignationHistoryDTO>>(emp_details);
            return lis_dsc;
        }

        public async Task<int> SaveEmployeeCheckIn(TblEmpdesigTab CheckIn_dets, CancellationToken Token)
        {

            CheckIn_dets.IsActive = true;
            CheckIn_dets.IsDeleted = false;
            CheckIn_dets.CreatedBy = _userInfo.Name;
            CheckIn_dets.CreatedDate = DateTime.Now;

           await _desgService.AddAsync(CheckIn_dets, Token);
            return 1;
        }
         public async Task<int> SaveEmployeeCheckOut (TblEmpdesigTab Checkout_dets, CancellationToken Token)
        {
            TblEmpdesigTab exis_Data = await _desgService.FirstOrDefaultByExpressionAsync(x => x.EmpId == Checkout_dets.EmpId, Token);
            if (exis_Data == null)
            {
                Checkout_dets.IsActive = true;
                Checkout_dets.IsDeleted = false;
                Checkout_dets.CreatedBy = _userInfo.Name;
                Checkout_dets.ModifiedDate = DateTime.Now;

                await _desgService.AddAsync(Checkout_dets, Token);
                return 1;
            }
            else
            {
                return 0;
            }
            
        }
        public async Task<EmployeeDesignationHistoryDTO> DeleteEmployeeBranchById(string ticket_num, CancellationToken Token)
        {
            TblEmpdesigTab emp_details = await _desgService.FirstOrDefaultByExpressionAsync(x => x.IsActive == true, Token);
            emp_details.IsActive = false;
            emp_details.IsDeleted = true;
            emp_details.ModifiedDate = DateTime.Now;

            await _desgService.UpdateAsync(emp_details, Token);

            return _mapper.Map<EmployeeDesignationHistoryDTO>(emp_details);

        }

    }
}
