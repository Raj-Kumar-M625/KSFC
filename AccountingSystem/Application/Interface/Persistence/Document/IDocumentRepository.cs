using Application.Interface.Persistence.Generic;
using Domain.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Document
{
    public interface IDocumentRepository : IGenericRepository<Documents>
    {
        Task<List<Documents>> AddDocuments(List<Documents> entity);
      
        Task<List<Documents>> GetDocumentDetails(int ID);
        Task<List<Documents>> AddDocumentDetails(List<Documents> entity);
        Task<Documents> DeleteDocumentDetails(Documents entity);
        Task<Documents> AddVendorDocument(Documents entity);
        Task<List<Documents>> GetVendorDocumentDetails(int ID);
        Task<Documents> DeleteVendorDocumentDetails(int ID);
        Task<Documents> GetTDSDocument(int ID);
        Task<Documents> GetBankStatementDocument(int ID);

        Task<Documents> GetDocument(int ID);
       
    }
}
