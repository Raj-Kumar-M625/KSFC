using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.LoanAccounting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin.IDM
{
    public interface ICommonService
    {
        Task<(bool, string, int, string)> UploadDocument(IdmFileUploadDto item);
        Task<List<ldDocumentDto>> FileList(string MainModule);
        Task<byte[]> ViewFile(string fileId, string mainModule);
        Task<bool> DeleteFile(string fileId, string mainModule);
        Task<IdmDDLListDTO> GetAllIdmDropDownList();
        Task<LA_DDLListDTO> GetAllLADropDownList();
    }
}
