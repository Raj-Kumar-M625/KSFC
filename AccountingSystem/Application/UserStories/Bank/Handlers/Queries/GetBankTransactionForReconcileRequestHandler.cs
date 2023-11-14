using Application.DTOs.Bank;
using Application.Interface.Persistence.Bank;
using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Reconcile;
using Application.UserStories.Bank.Requests.Queries;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Domain.Bank;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bank.Handlers.Queries
{
    public class GetBankTransactionForReconcileRequestHandler : IRequestHandler<GetBankTransactionForReconcileRequest, List<ReconciliationDto>>
    {
        private readonly IReconciliationRepository _reconcileRepo;
        private readonly IMapper _mapper;
        public GetBankTransactionForReconcileRequestHandler(IReconciliationRepository reconcileRepo, IMapper mapper)
        {
            _reconcileRepo = reconcileRepo;
            _mapper = mapper;
        }

        public async Task<List<ReconciliationDto>> Handle(GetBankTransactionForReconcileRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var reconcile = await _reconcileRepo.GetReconciliationsByBankTransactionId(request.bankTransactionId);
                return _mapper.Map<List<ReconciliationDto>>(reconcile);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
