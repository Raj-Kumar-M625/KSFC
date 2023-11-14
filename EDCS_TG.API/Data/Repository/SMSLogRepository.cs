using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository;

public class SMSLogRepository : Repository<SMSLog>, ISMSLogRepository
{
    public SMSLogRepository(KarmaniDbContext dbContext) : base(dbContext)
    {
    }
}