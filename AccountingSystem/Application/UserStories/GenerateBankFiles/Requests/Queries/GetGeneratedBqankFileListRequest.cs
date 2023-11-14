using Application.DTOs.GenerateBankFile;
using Domain.GenarteBankfile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.GenerateBankFiles.Requests.Queries
{
    public class GetGeneratedBqankFileListRequest : IRequest<IQueryable<GenerateBankFile>>
    {
    }
}
