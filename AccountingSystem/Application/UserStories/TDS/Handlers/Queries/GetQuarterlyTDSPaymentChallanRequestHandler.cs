﻿using Application.Interface.Persistence.Payment;
using Application.UserStories.TDS.Requests.Queries;
using AutoMapper;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.TDS.Handlers.Queries
{
    public class GetQuarterlyTdsPaymentChallanRequestHandler : IRequestHandler<GetQuarterlyTdsPaymentChallanRequest, IQueryable<QuarterlyTdsPaymentChallan>>
    {
        private readonly IQuarterlyTdsPaymentChallanRepository _quarterlyTDSPaymentChallanRepository;
        private readonly IMapper _mapper;
        public GetQuarterlyTdsPaymentChallanRequestHandler(IQuarterlyTdsPaymentChallanRepository quarterlyTDSPaymentChallanRepository, IMapper mapper)
        {
            _quarterlyTDSPaymentChallanRepository = quarterlyTDSPaymentChallanRepository;
            _mapper = mapper;
        }

        public async Task<IQueryable<QuarterlyTdsPaymentChallan>> Handle(GetQuarterlyTdsPaymentChallanRequest request, CancellationToken cancellationToken)
        {
            return await _quarterlyTDSPaymentChallanRepository.GetQuarterlyTDSPaymentChallanAsync();
        }
    }
}
