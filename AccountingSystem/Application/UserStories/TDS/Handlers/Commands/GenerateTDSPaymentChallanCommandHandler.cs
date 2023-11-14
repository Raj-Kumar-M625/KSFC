using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.TDS.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Bill;
using Domain.Master;
using Domain.Payment;
using Domain.TDS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.TDS.Handlers.Commands
{
    public class GenerateTdsPaymentChallanCommandHandler : IRequestHandler<GenerateTdsPaymentChallanCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IBankMasterRepository _bankMasterrepo;

        private readonly ITdsPaymentChallanRepository _tdsPaymentChallanRepository;

        public GenerateTdsPaymentChallanCommandHandler(ITdsPaymentChallanRepository tdsPaymentChallanRepository, IMapper mapper, ICommonMasterRepository commonMaster,
            IBankMasterRepository bankmasterRepo)
        {
            _tdsPaymentChallanRepository = tdsPaymentChallanRepository;
            _mapper = mapper;
            _commonMaster = commonMaster;
            _bankMasterrepo = bankmasterRepo;
        }

        public async Task<int> Handle(GenerateTdsPaymentChallanCommand request, CancellationToken cancellationToken)
        {
     
            var tdsPaymentChallan = _mapper.Map<TdsPaymentChallan>(request.tdsPaymentChallan);

            tdsPaymentChallan.CreatedOn = DateTime.UtcNow;
            tdsPaymentChallan.ModifiedOn = DateTime.UtcNow;
            tdsPaymentChallan.CreatedBy = request.user;
            tdsPaymentChallan.ModifiedBy = request.user;
            //List<TDSStatus> TDSStatus = new  List<TDSStatus>();

            //foreach(var item in tdsPaymentChallan.BillsId)
            //{
            //    TDSStatus obj = new TDSStatus();
            //    obj.BillID = item;
            //    obj.StatusCMID = TDSStatusList.FirstOrDefault(p => p.CodeValue == ValueMapping.ChallanCreated).Id;
            //    tdsPaymentChallan.ModifiedOn = DateTime.UtcNow;
            //    tdsPaymentChallan.CreatedBy = request.user;
            //}
            tdsPaymentChallan = await _tdsPaymentChallanRepository.AddTDSPaymentChallanAsync(tdsPaymentChallan);
            return tdsPaymentChallan.Id;
        }
    }
}
