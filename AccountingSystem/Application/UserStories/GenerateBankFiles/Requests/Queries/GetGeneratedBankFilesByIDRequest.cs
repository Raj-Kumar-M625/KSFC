using Application.DTOs.GenerateBankFile;
using MediatR;
using System.Collections.Generic;

namespace Application.UserStories.GenerateBankFiles.Requests.Queries
{
    public class GetGeneratedBankFilesByIDRequest:IRequest<List<PaymentGenerateBankFileDto>>
    {
        public int Id { get; set; }
    }
}
