using Application.DTOs.Profile;
using Application.Interface.Persistence.Document;
using Application.Interface.Persistence.HeaderProfile;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.HeaderProfile.Request.Commands;
using AutoMapper;
using Common.FileUpload;
using Domain.Document;
using Domain.Profile;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.HeaderProfile.Handler.Commands
{
    public class HeaderProfileCommandhandler : IRequestHandler<AddHeaderProfileCommand, int>
    {

        private readonly IHeaderProfileRepositoty headerProfileRepositoty;
        private readonly IMapper _mapper;
        private IHostingEnvironment Environment;

      public HeaderProfileCommandhandler(IHeaderProfileRepositoty headerProfileRepositoty, IMapper mapper, IHostingEnvironment environment)
        {
            this.headerProfileRepositoty = headerProfileRepositoty;
            _mapper = mapper;
            Environment = environment;
        }

        public async Task<int> Handle(AddHeaderProfileCommand request, CancellationToken cancellationToken)
        {

           var  ExistingImage = await headerProfileRepositoty.GetExistingImageByType(request.HeaderProfileDetails.Type);
            //var ExistingImages = _mapper.Map<HeaderProfileDetailsDto>(ExistingImage);

            ExistingImage.IsActive = false;
           var Existing = await headerProfileRepositoty.UpdateExistingImage(ExistingImage);


            ImageUpload ImageUplaod =new  ImageUpload();
            string wwwPath = this.Environment.WebRootPath;
            var ImageModel = await ImageUplaod.UploadImage(request.HeaderProfileDetails.files, request.entityPath, wwwPath, request.HeaderProfileDetails.DocumentName);

            var Images = _mapper.Map<List<HeaderProfileDetailsDto>>(ImageModel);
           // var id = request.Id;
            foreach (var item in Images)
            {
              
                item.EntityType = request.entityType;
                item.UploadedBy = request.user;
                item.IsActive = true;
            }

            var Image = _mapper.Map<List<HeaderProfileDetails>>(Images);
            Image = await headerProfileRepositoty.AddImage(Image);
            return Image.Count;
        }
    }
}
