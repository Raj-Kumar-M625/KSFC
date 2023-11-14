using Application.DTOs.GenerateBankFile;
using MediatR;
using System.Collections.Generic;

namespace Application.UserStories.BankFile.Requests.Queries
{



    public class GetGenerateBankFileRequestList:IRequest<List<BankFileDto>>
    {
        public int Id { get; set; }
    }

}
