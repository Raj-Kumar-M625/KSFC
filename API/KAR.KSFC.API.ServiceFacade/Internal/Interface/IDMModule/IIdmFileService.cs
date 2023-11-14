using KAR.KSFC.Components.Common.Dto.IDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule
{
    public interface IIdmFileService
    {
        Task<ldDocumentDto> UploadFile(byte[] bytes, int subModuleId, string subModuleType, string mainModule, IConfiguration configure, CancellationToken token);
        Task<IdmFileUploadDto> GetEncryptedFileById(string mainModule, string documentId, CancellationToken token);
        Task<List<ldDocumentDto>> GetFileListAsync(string mainModule,CancellationToken token);
        Task<bool> DeleteFileAsync(string mainModule,string documentId, IConfiguration configure, CancellationToken token);
    }
}
