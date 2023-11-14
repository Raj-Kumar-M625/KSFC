using Application.Interface.Persistence.Generic;
using Domain.Document;
using Domain.Profile;
using Domain.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.HeaderProfile
{
    public interface IHeaderProfileRepositoty :IGenericRepository<HeaderProfileDetails>
    {
        Task<List<HeaderProfileDetails>> AddImage(List<HeaderProfileDetails> entity);
        IQueryable<HeaderProfileDetails> GetImage();
        Task<HeaderProfileDetails> GetExistingImageByType(string  entityType);
        Task<HeaderProfileDetails> UpdateExistingImage(HeaderProfileDetails entity);


    }
}
