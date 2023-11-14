using Application.Interface.Persistence.HeaderProfile;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Bill;
using Domain.Document;
using Domain.Profile;
using Domain.Vendor;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.HeaderProfile
{
    public class HeaderProfileRepository : GenericRepository<HeaderProfileDetails>, IHeaderProfileRepositoty
    {
        private readonly AccountingDbContext _dbContext;

        public HeaderProfileRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HeaderProfileDetails>> AddImage(List<HeaderProfileDetails> entity)
        {
            _dbContext.AddRange(entity);
            await _dbContext.SaveChangesAsync();
            return entity;

        }

        public IQueryable<HeaderProfileDetails> GetImage()
        {
            IQueryable<HeaderProfileDetails> res = _dbContext.Set<HeaderProfileDetails>()
                                                .Where(o => o.IsActive == true);

            return res;
        }

        public async Task<HeaderProfileDetails> GetExistingImageByType(string  EntityType)
        {
            
            var res = await _dbContext.HeaderProfileDetails.Where(o => o.EntityType == EntityType).Where(o=>o.IsActive==true).FirstOrDefaultAsync();
            return res;
        }

        public async Task<HeaderProfileDetails> UpdateExistingImage(HeaderProfileDetails entity)
        {
            try
            {
                _dbContext.Update(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                throw e;
            }

        }


    }
}
