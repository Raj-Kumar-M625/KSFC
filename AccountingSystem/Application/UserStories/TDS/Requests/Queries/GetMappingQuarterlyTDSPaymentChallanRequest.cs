﻿using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.TDS.Requests.Queries
{
    public class GetMappingQuarterlyTdsPaymentChallanRequest:IRequest<IQueryable<MappingTdsQuarterChallan>>
    {
        public List<int> Ids { get; set; }
    }
}
