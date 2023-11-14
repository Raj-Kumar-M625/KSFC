using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface
{
    public interface IFileService
    {
        /// <summary>
        /// Get Encryptrd file
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<FileDetailsDTO> GetEncryptedFileById(string documentId, CancellationToken token);

        /// <summary>
        /// Upload File
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileTypes"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public Task<EnqDocumentDTO> UploadFile(byte[] bytes, int enqId, DocumentTypeEnum documentType, FileTypesEnum fileTypes, IConfiguration configure, CancellationToken token);

        /// <summary>
        /// Delete document
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public  Task<bool> DeleteFileAsync(int documentId, IConfiguration configure, CancellationToken token);

        /// <summary>
        /// Get File List Async
        /// </summary>
        /// <param name="enqId"></param>
        /// <param name="documentType"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<EnqDocumentDTO>> GetFileListAsync(int enqId,string process, DocumentTypeEnum documentType,CancellationToken token);
    }
}
