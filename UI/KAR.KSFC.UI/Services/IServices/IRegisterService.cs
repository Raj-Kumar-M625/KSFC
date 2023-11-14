using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.UI.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices
{
    public interface IRegisterService
    {
        Task<RegisterViewModel> GetDDLConstitutionTypes();

        Task<List<CnstCdtab>> GetAllConstitutionTypes();
        Task<bool> RegisterUser(RegisterViewModel registerViewModel);
    }
}
