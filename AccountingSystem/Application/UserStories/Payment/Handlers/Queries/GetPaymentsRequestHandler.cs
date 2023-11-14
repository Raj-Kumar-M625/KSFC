using Application.Interface.Persistence.Payment;
using Application.UserStories.Payment.Requests.Queries;
using AutoMapper;
using Domain.Payment;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Handlers.Queries
{
    /// <summary>
    /// Author: Date:26/05/2022
    /// Purpose:Get Payments Request Handler
    /// </summary>
    /// <returns></returns>
    public class GetPaymentsRequestHandler : IRequestHandler<GetPaymentsRequest, IQueryable<Payments>>
    {
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Author: Date:26/05/2022
        /// Purpose:Consturctor
        /// </summary>
        /// <returns></returns>
        public GetPaymentsRequestHandler(IPaymentRepository vendorPaymentRepository, IMapper mapper)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Author: Date:26/05/2022
        /// Purpose:Handler Request method
        /// </summary>
        /// <returns></returns>
        public async Task<IQueryable<Payments>> Handle(GetPaymentsRequest request, CancellationToken cancellationToken)
        {
            var payments =  _vendorPaymentRepository.GetVendorPayment();
            return payments;
        }
    }
}
