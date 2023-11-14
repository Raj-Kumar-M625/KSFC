using Application.Interface.Persistence.Document;
using Common.ConstantVariables;
using Domain.Document;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Document
{
    public class DocumentRepository : GenericRepository<Documents>, IDocumentRepository
    {
        private readonly AccountingDbContext _dbContext;

        public DocumentRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Documents> AddVendorDocument(Documents documents)
        {
            await _dbContext.AddAsync(documents);
            await _dbContext.SaveChangesAsync();
            return documents;
        }



        public async Task<List<Documents>> AddDocuments(List<Documents> entity)
        {
            _dbContext.AddRange(entity);
            await _dbContext.SaveChangesAsync();
            return entity;

        }

        public async Task<List<Documents>> AddDocumentDetails(List<Documents> entity)
        {
            try
            {
                await _dbContext.AddRangeAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;

            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}", e.Message);
                return entity;
            }
        }

          public async Task<List<Documents>> GetDocumentDetails(int ID)
        {
            string entitytype = ValueMapping.Bill;
            var res = await _dbContext.Documents.Where(o => o.DocumentRefID == ID)
                                                 .Where(o => o.EntityType == entitytype && o.Status).ToListAsync();
            return res;
        }

        public async Task<Documents> DeleteDocumentDetails(Documents entity)
        {
           
            //var rec = await _dbContext.Documents.FindAsync(docID);
            entity.Status = false;
            _dbContext.Documents.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }


        public async Task<List<Documents>> GetVendorDocumentDetails(int ID)
        {
            string entitytype = ValueMapping.Vendor;
            var res = await _dbContext.Documents.Where(o => o.DocumentRefID == ID)
                                                           .Where(o => o.EntityType == entitytype && o.Status).ToListAsync();
            return res;
        }

        public async Task<Documents> DeleteVendorDocumentDetails(int ID)
        {
            var rec = await _dbContext.Documents.FindAsync(ID);
            rec.Status = false;
            _dbContext.Documents.Update(rec);
            await _dbContext.SaveChangesAsync();
            return rec;
        }

        public async Task<Documents> GetTDSDocument(int ID)
        {
            string entitytype = ValueMapping.TDS;
            var res = await _dbContext.Documents.FirstOrDefaultAsync(o => o.DocumentRefID == ID && o.EntityType == entitytype);
            return res;
        }

        public async Task<Documents> GetDocument(int ID)
        {           
            var res = await _dbContext.Documents.FirstOrDefaultAsync(o => o.Id == ID);
            return res;
        }

        public async Task<Documents> GetBankStatementDocument(int ID)
        {
            string entitytype = ValueMapping.UploadBankStatement;
            var res = await _dbContext.Documents.FirstOrDefaultAsync(o => o.DocumentRefID == ID && o.EntityType == entitytype);
            return res;
        }
    }
}
