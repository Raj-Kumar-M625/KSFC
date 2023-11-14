using Application.DTOs.Vendor;
using Application.Interface.Persistence.Vendor;
using Domain.Vendor;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.Vendor
{
    public class VendorRepository:GenericRepository<Vendors>, IVendorRepository
    {
        private readonly AccountingDbContext _dbContext;

        public VendorRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Vendors> AddVendor(Vendors vendors)
        {
            try
            {
                await _dbContext.AddAsync(vendors);
                await _dbContext.AddAsync(vendors.VendorPerson.Addresses);
                await _dbContext.AddAsync(vendors.VendorPerson.Contacts);
                await _dbContext.AddAsync(vendors.VendorPerson);
                await _dbContext.AddAsync(vendors.VendorBalance);
                await _dbContext.AddAsync(vendors.VendorBankAccounts);
                await _dbContext.AddAsync(vendors.VendorDefaults);
                await _dbContext.SaveChangesAsync();
                return vendors;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Vendors> AddVendorDetail(Vendors vendors)
        {
            try
            {
                vendors.VendorBalance = null;
                if (vendors.VendorDefaults.Id == 0)
                {
                    await _dbContext.AddAsync(vendors.VendorDefaults);

                }
               
                if (vendors.VendorPerson.Id == 0)
                {
                    await _dbContext.AddAsync(vendors.VendorPerson.Addresses);
                    await _dbContext.AddAsync(vendors.VendorPerson.Contacts);
                    await _dbContext.AddAsync(vendors.VendorPerson);

                }
                else
                {
                    if (vendors.VendorPerson.Addresses.Id == 0)
                    {
                        await _dbContext.AddAsync(vendors.VendorPerson.Addresses);
                    }
                    else
                    {
                        _dbContext.Update(vendors.VendorPerson.Addresses).State = EntityState.Modified;

                    }
                    if (vendors.VendorPerson.Contacts.Id == 0)
                    {
                        await _dbContext.AddAsync(vendors.VendorPerson.Contacts);
                    }
                    else
                    {
                        _dbContext.Update(vendors.VendorPerson.Contacts).State = EntityState.Modified;
                    }

                       
                   
                }
                if (vendors.VendorBankAccounts.Id == 0)
                {
                    await _dbContext.AddAsync(vendors.VendorBankAccounts);

                }
                

                //vendors.ModifiedOn = DateTime.UtcNow;
                //_dbContext.Update(vendors).State = EntityState.Modified;


                await _dbContext.SaveChangesAsync();
                return vendors;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Vendors> AddVendorDetails(Vendors entity)
        {

            _dbContext.Add(entity);
            _dbContext.Add(entity.VendorBalance);

            await _dbContext.SaveChangesAsync();
            return entity;
        }


        public async Task<Vendors> EditVendor(Vendors entity)
        {
            var list = _dbContext.Vendor.ToList();
            entity.ModifiedOn = DateTime.UtcNow;
            var entry = list.FirstOrDefault(x => x.Id == entity.Id);
            _dbContext.Entry(entry).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public IQueryable<Vendors> GetVendor()
        {
            IQueryable<Vendors> res = _dbContext.Set<Vendors>()
                                                .Include(o => o.VendorDefaults)
                                                .Include(o => o.VendorBalance)
                                                .Include(o => o.VendorBankAccounts);
                                               
                                                
            return res;
        }

        async Task<List<VendorListDto>> IVendorRepository.GetVendorList()
        {
            var res = await _dbContext.Set<Vendors>().Include(o => o.VendorDefaults).Include(o => o.VendorBalance).Distinct().ToListAsync();
            List<VendorListDto> vendorDetails = res
         .Select(b => new VendorListDto
         {
             Name = b.Name,
             GSTIN_Number = b.GSTIN_Number == null ? "N/A" : b.GSTIN_Number,
             PAN_Number = b.PAN_Number,
             Category = b.VendorDefaults == null ? "N/A" : b.VendorDefaults.Category,
             OpeningBalance = b.VendorBalance == null ? 0 : b.VendorBalance.OpeningBalance,
             TotalBillAmount = b.VendorBalance == null ? 0 : b.VendorBalance.TotalBillAmount,
             TotalNetPayable = b.VendorBalance == null ? 0 : b.VendorBalance.TotalNetPayable,
             AmountPaid = b.VendorBalance == null ? 0 : b.VendorBalance.AmountPaid,
             BalanceAmount = b.VendorBalance == null ? 0 : b.VendorBalance.BalanceAmount,
             TDSPercentage = b.VendorDefaults == null ? 0 : b.VendorDefaults.TDSPercentage,
             GST_TDSPercentage = b.VendorDefaults == null ? 0 : b.VendorDefaults.GST_TDSPercentage,
             Status = b.Status ? "Active" : "InActive"
         }).ToList();

            return vendorDetails;
        }

        public async Task<Vendors> GetVendorsDetailsByID(int ID)
        {
            var res = await _dbContext.Vendor.Include(o => o.VendorDefaults)
                                                .Include(o => o.VendorBalance)
                                                .Include(o => o.VendorPerson.Addresses)
                                                .Include(o => o.VendorPerson.Contacts)
                                                .Include(o => o.VendorBankAccounts)
                                                .Include(o => o.VendorBankAccounts.BranchMaster)
                                                .Include(o => o.VendorBankAccounts.BranchMaster.BankDetails)
                                                .Include(o => o.VendorPerson)
                                                .Where(o => o.Id == ID).FirstOrDefaultAsync(); 
            return res;
        }
        public async Task<Vendors> GetVendorsDetailsByPANID(string PANID)
        {
            var res = await _dbContext.Vendor.Include(o => o.VendorDefaults)
                                                .Include(o => o.VendorBalance)
                                                .Include(o => o.VendorPerson.Addresses)
                                                .Include(o => o.VendorPerson.Contacts)
                                                .Include(o => o.VendorBankAccounts)
                                                .Include(o => o.VendorPerson)
                                                .Include(o => o.VendorBankAccounts.BranchMaster)
                                                .Include(o => o.VendorBankAccounts.BranchMaster.BankDetails)
                                               .Where(o => o.PAN_Number == PANID).FirstOrDefaultAsync();
                                              // .Where(o => o.VendorBankAccounts.BankMasterId == o.VendorBankAccounts.BranchMaster.Id)
            return res;
        }


        public async Task<Vendors> UpdateVendor(Vendors vendors)
        {
            vendors.VendorBalance = null;
            if (vendors.VendorDefaults == null)
            {
                await _dbContext.AddAsync(vendors.VendorDefaults);

            }
            else
            {
                _dbContext.Update(vendors.VendorDefaults).State = EntityState.Modified;
            }
            if (vendors.VendorPerson == null)
            {
                await _dbContext.AddAsync(vendors.VendorPerson.Addresses);
                await _dbContext.AddAsync(vendors.VendorPerson.Contacts);
                await _dbContext.AddAsync(vendors.VendorPerson);

            }
            else
            {
                _dbContext.Update(vendors.VendorPerson.Addresses).State = EntityState.Modified;
                _dbContext.Update(vendors.VendorPerson.Contacts).State = EntityState.Modified;
                _dbContext.Update(vendors.VendorPerson).State = EntityState.Modified;
            }
            if (vendors.VendorBankAccounts == null)
            {
                await _dbContext.AddAsync(vendors.VendorBankAccounts);

            }
            else
            {
                _dbContext.Update(vendors.VendorBankAccounts).State = EntityState.Modified;
            }

            vendors.ModifiedOn = DateTime.UtcNow;
            _dbContext.Update(vendors).State = EntityState.Modified;


            await _dbContext.SaveChangesAsync();
            return vendors;

        }
        public async Task<VendorPerson> AddVendorPerson(VendorPerson entity)
        {
            _dbContext.Add(entity.Addresses);
            _dbContext.Add(entity.Contacts);

            _dbContext.Add(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<VendorPerson> EditVendorPerson(VendorPerson entity)
        {
            if (entity.Id == 0)
            {
                _dbContext.Add(entity.Addresses);
                _dbContext.Add(entity.Contacts);
                _dbContext.Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                entity.ModifiedOn = DateTime.UtcNow;
                _dbContext.Update(entity).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
            return entity;

        }

        public async Task<VendorDefaults> EditVendorDefaults(VendorDefaults entity)
        {
            if (entity.Id == 0)
            {
                _dbContext.Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                entity.ModifiedOn = DateTime.UtcNow;
                _dbContext.Update(entity).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<VendorBankAccount> EditVendorBankAccount(VendorBankAccount entity)
        {
            if (entity.Id == 0)
            {
                _dbContext.Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                entity.ModifiedOn = DateTime.UtcNow;
                _dbContext.Update(entity).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<VendorDefaults> AddVendorDefaults(VendorDefaults entity)
        {
            entity.ModifiedOn = DateTime.UtcNow;
            await _dbContext.AddAsync(entity);

            await _dbContext.SaveChangesAsync();
            return entity;

        }
        public async Task<VendorBankAccount> AddVendorBankAccount(VendorBankAccount entity)
        {
            entity.ModifiedOn = DateTime.UtcNow;
            await _dbContext.AddAsync(entity);

            await _dbContext.SaveChangesAsync();
            return entity;

        }

        public async Task<VendorBalance> GetVendorBalanceByID(int ID)
        {
            var res = await _dbContext.VendorBalance.Where(o => o.VendorId == ID).FirstOrDefaultAsync();
            return res;
        }

        public async Task<VendorBalance> UpdateVendorBalance(VendorBalance vendorBalace)
        {
            vendorBalace.ModifiedOn = DateTime.UtcNow;
            _dbContext.Update(vendorBalace).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return vendorBalace;
        }

    }
}

