﻿using Application.DTOs.TDS;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Requests.Queries
{
    public class GetTdsPaymentChallanListRequest: IRequest<IQueryable<TdssPaymentChallanDto>>
    {
    }
}
