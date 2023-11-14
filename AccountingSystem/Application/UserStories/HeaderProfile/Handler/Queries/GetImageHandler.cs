using Application.Interface.Persistence.HeaderProfile;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.HeaderProfile.Request.Queries;
using AutoMapper;
using Domain.Profile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.HeaderProfile.Handler.Queries
{
    public class GetImageHandler : IRequestHandler<GetImage, IQueryable<HeaderProfileDetails>>
    {
        private readonly IHeaderProfileRepositoty _headerProfileRepository;
        private readonly IMapper _mapper;


        public GetImageHandler(IHeaderProfileRepositoty headerProfileRepository, IMapper mapper)
        {
            _headerProfileRepository = headerProfileRepository;
            _mapper = mapper;
        }

        public async Task<IQueryable<HeaderProfileDetails>> Handle(GetImage request, CancellationToken cancellationToken)
        {
            var Image = _headerProfileRepository.GetImage();
            return Image;

        }
    }
}
