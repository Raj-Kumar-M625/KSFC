using Application.DTOs.Vendor;
using Application.Interface.Persistence.Generic;
using Domain.Vendor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Vendor
{
    public interface IVendorRepository:IGenericRepository<Vendors>
    {
        Task<Vendors> AddVendor(Vendors entity);
        Task<Vendors> AddVendorDetail(Vendors entity);
        IQueryable<Vendors> GetVendor();
        Task<Vendors> GetVendorsDetailsByPANID(string PANID);
        Task<Vendors> GetVendorsDetailsByID(int ID);
        Task<Vendors> EditVendor(Vendors entity);
        Task<Vendors> UpdateVendor(Vendors entity);
        Task<Vendors> AddVendorDetails(Vendors entity);
        Task<List<VendorListDto>> GetVendorList();
        Task<VendorDefaults> EditVendorDefaults(VendorDefaults entity);
        Task<VendorPerson> AddVendorPerson(VendorPerson entity);
        Task<VendorDefaults> AddVendorDefaults(VendorDefaults entity);
        Task<VendorBankAccount> AddVendorBankAccount(VendorBankAccount entity);
        Task<VendorPerson> EditVendorPerson(VendorPerson entity);
        Task<VendorBankAccount> EditVendorBankAccount(VendorBankAccount entity);

        Task<VendorBalance> GetVendorBalanceByID(int ID);
        Task<VendorBalance> UpdateVendorBalance(VendorBalance vendorBalace);


    }
}
