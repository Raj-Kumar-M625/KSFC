using Domain.Vendor;
using MediatR;
using System.Linq;

namespace Application.UserStories.Vendor.Requests.Queries
{
    public class GetVenorQuerableValue:IRequest<IQueryable<Vendors>>
    {
    }
}
