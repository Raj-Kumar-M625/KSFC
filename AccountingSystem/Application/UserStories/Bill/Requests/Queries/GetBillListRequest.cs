﻿using Application.DTOs.Bill;
using Domain.Bill;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Requests.Queries
{
    public class GetBillListRequest: IRequest<IQueryable<Bills>>
    {
    }
}
