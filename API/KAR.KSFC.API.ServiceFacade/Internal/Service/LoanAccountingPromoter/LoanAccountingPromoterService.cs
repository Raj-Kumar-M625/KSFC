using KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccountingPromoter;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.LoanAccountingPromoter
{
   
    public class LoanAccountingPromoterService : ILoanAccountingPromoterService
    {
        private readonly IEntityRepository<TblAppLoanMast> _loanRepository;
        private readonly IEntityRepository<TblEmpchairDet> _empChairDet;
        private readonly IEntityRepository<TblIdmLegalWorkflow> _legalWokFlow;

        public LoanAccountingPromoterService(IEntityRepository<TblAppLoanMast> loanRepository, IEntityRepository<TblEmpchairDet> empChairDet, IEntityRepository<TblIdmLegalWorkflow> legalWokFlow)
        {
            _loanRepository = loanRepository;
            _empChairDet = empChairDet;
            _legalWokFlow = legalWokFlow;
        }

        public async Task<IEnumerable<LoanAccountNumberDTO>> GetAccountNumber(CancellationToken token, string EmpId)
        {
            var OffcUnit = await _loanRepository.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false,
                incOff => incOff.OffcCdtab,
                incUnit => incUnit.TblUnitMast,
                workflow => workflow.TblIdmLegalWorkflow
                 ).ConfigureAwait(false);

            var EmpChair = await _empChairDet.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false && x.EmpId == EmpId,
              incEmp => incEmp.ChairCodeNavigation
              ).ConfigureAwait(false);

            var workFlow = await _legalWokFlow.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false
               ).ConfigureAwait(false);

            var LoanList = OffcUnit.Join(EmpChair,
            loanmast => loanmast.InOffc,
            chair => chair.OffcCd,
            (loanmast, chair) => new
            {
                LoanMastID = loanmast.InMastId,
                LoanOffice = loanmast.OffcCdtab.OffcNam,
                LoanUnit = loanmast.TblUnitMast.UtName,
                LoanAcc = loanmast.InNo,
                LoanSub = loanmast.InSub,
                InOffc = loanmast.InOffc,
                InUnit = loanmast.InUnit
            }).ToList();
            var LoanAccounts = LoanList.Where(x => workFlow.Any(y => y.LoanAcc == x.LoanAcc)).ToList();


            var result = new List<LoanAccountNumberDTO>();
            if (LoanList.Count > 0)
            {
                foreach (var i in LoanAccounts)
                {
                    result.Add(new LoanAccountNumberDTO()
                    {
                        LoanOffice = i.LoanOffice,
                        LoanUnit = i.LoanUnit,
                        LoanAcc = i.LoanAcc,
                        LoanSub = i.LoanSub,
                        InOffc = i.InOffc,
                        InUnit = i.InUnit
                    });
                }
            }
            return result;
        }

    }
    
}
