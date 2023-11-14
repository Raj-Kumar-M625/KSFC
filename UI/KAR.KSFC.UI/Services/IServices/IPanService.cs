using KAR.KSFC.Components.Data.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices
{
    public interface IPanService
    {
        bool GetPanAttempts();
        Task<JsonResult> VerifyPanWithConstTypeAndDb(string panNo, string constitutionName, string mobileNum);

        string GetPanFourthCharByConstitution(string constitutionName);
        CnstCdtab GetConstitutionByPanNumber();
        string GetPanFourthCharByUserName();
    }
}
