using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Security;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Exceptions;
using KAR.KSFC.Components.Common.Utilities.Extensions;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service
{
    public class FileService : IFileService
    {
        private readonly IEntityRepository<TblEnqDocument> _documentRepository;
        private readonly IEnquiryHomeService _enquiryService;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        private readonly IConfiguration _configure;
        public FileService(IEntityRepository<TblEnqDocument> documentRepository, UserInfo userInfo, IMapper mapper, IUnitOfWork work, IEnquiryHomeService enquiryService, IConfiguration configure)
        {
            _configure = configure;
            _documentRepository = documentRepository;
            _userInfo = userInfo;
            _mapper = mapper;
            _work = work;
            _enquiryService = enquiryService;
        }

        public async Task<bool> DeleteFileAsync(int documentId, IConfiguration configure, CancellationToken token)
        {
            var data = await _documentRepository.FirstOrDefaultByExpressionAsync(x => x.Id == documentId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (data != null)
            {
                Enum.TryParse(data.DocSection, out DocumentTypeEnum typeEnum);
                string folderpath = $"{ FileTypeExtension.GetArchiveFilePath(typeEnum, configure)}";
                string archivedPath = $"{ folderpath}\\{data.Name}";
                try
                {
                    if (!Directory.Exists(folderpath))
                        Directory.CreateDirectory(folderpath);

                    File.Move(data.FilePath, archivedPath);
                }
                finally
                {
                    data.IsDeleted = true;
                    data.IsActive = false;
                    data.ModifiedDate = DateTime.UtcNow;
                    data.ModifiedBy = _userInfo.UserId;
                    await _documentRepository.UpdateAsync(data, token).ConfigureAwait(false);
                    await _work.CommitAsync(token).ConfigureAwait(false);
                }
                return true;
            }
            return false;
        }

        public async Task<FileDetailsDTO> GetEncryptedFileById(string documentId, CancellationToken token)
        {
            var data = await _documentRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == documentId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (data != null)
            {
                var byteArray = await FileSecurity.EncryptedBytesFromFilePath(data.FilePath, "password");
                if (byteArray == null)
                {
                    throw new FileSecurityExceptions("File encryption failed.");
                }

                return new FileDetailsDTO
                {
                    Bytes = byteArray,
                    DocumentType = data.DocSection,
                    FileName = data.Name
                };
            }
            throw new FileSecurityExceptions("File encryption failed.");

        }

        public async Task<EnqDocumentDTO> UploadFile(byte[] bytes, int enqId, DocumentTypeEnum documentType, FileTypesEnum fileTypes, IConfiguration configure, CancellationToken token)
        {
            if (!FileTypeExtension.ValidateFileSize(bytes.Length, fileTypes, configure))
            {
                var size = FileTypeExtension.GetFileSize(fileTypes, configure);
                throw new FileSecurityExceptions(_configure["FileSize:Message"]);
            }
            if (enqId == 0)
            {
                enqId = await _enquiryService.AddNewEnqiry(_userInfo.Pan, token).ConfigureAwait(false);
            }

            var isAlreadyExist = await _documentRepository.FirstOrDefaultByExpressionAsync(x => x.EnquiryId == enqId && x.IsActive == true
                                   && x.IsDeleted == false && x.Description == documentType.ToString()
                                   && x.DocSection == documentType.ToString() && x.Process == "EnquirySubmission", token).ConfigureAwait(false);

            if (isAlreadyExist != null)
            {
                string folderpath = FileTypeExtension.GetArchiveFilePath(documentType, configure);
                string archivedPath = $"{ folderpath }\\{isAlreadyExist.Name}";
                try
                {
                    if (!Directory.Exists(folderpath))
                        Directory.CreateDirectory(folderpath);
                    File.Move(isAlreadyExist.FilePath, archivedPath);
                }
                finally
                {
                    isAlreadyExist.IsActive = false;
                    isAlreadyExist.IsDeleted = true;
                    isAlreadyExist.ModifiedBy = _userInfo.UserId;
                    isAlreadyExist.ModifiedDate = DateTime.UtcNow;

                    await _documentRepository.UpdateAsync(isAlreadyExist, token).ConfigureAwait(false);
                }
            }



            string filePath = FileTypeExtension.GetFilePath(documentType, configure);

            string fileName = FileTypeExtension.GetFileName(documentType, enqId);

            string filepath = $"{filePath}\\{fileName}.{fileTypes.ToString()}";

            var documentData = new TblEnqDocument()
            {

                EnquiryId = enqId,
                Description = documentType.ToString(),
                Name = $"{fileName}.{fileTypes.ToString()}",
                FilePath = filepath,
                DocSection = documentType.ToString(),
                Process = "EnquirySubmission",
                FileType = fileTypes.ToString(),
                UniqueId = Guid.NewGuid().ToString(),
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _userInfo.UserId
            };

            var data = await _documentRepository.AddAsync(documentData, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            //Decrypting the byte array
            var byteArray = FileSecurity.DecryptByteArray(bytes, "password");
            //Save byte array
            var result = await FileSecurity.SaveBytesToFile(filepath, byteArray);

            if (!result)
            {
                throw new FileSecurityExceptions("File decryption failed.");
            }
            return _mapper.Map<EnqDocumentDTO>(data);

        }

        public async Task<IEnumerable<EnqDocumentDTO>> GetFileListAsync(int enqId, string process, DocumentTypeEnum documentType, CancellationToken token)
        {
            var data = await _documentRepository.FindByExpressionAsync(x => x.EnquiryId == enqId
                                                                    && x.Process == process
                                                                    && x.DocSection == documentType.ToString()
                                                                    && x.IsActive == true
                                                                    && x.IsDeleted == false, token).ConfigureAwait(false);

            return _mapper.Map<IEnumerable<EnqDocumentDTO>>(data);

        }
    }
}
