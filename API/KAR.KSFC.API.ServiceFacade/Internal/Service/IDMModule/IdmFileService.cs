using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Utilities.Extensions;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;
using KAR.KSFC.Components.Common.Dto.Enums;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using KAR.KSFC.Components.Common.Utilities.Exceptions;
using System.IO;
using KAR.KSFC.Components.Common.Security;
using KAR.KSFC.Components.Common.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Utilities.Helpers;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule
{
    public class IdmFileService : IIdmFileService
    {
        private readonly IEntityRepository<TblLdDocument> _ldDocumentRepository;
        private readonly IEntityRepository<TblUCDocument> _acDocumentRepository;
        private readonly IEntityRepository<TblDCDocument> _dcDocumentRepository;
        private readonly IEntityRepository<TblINSPDocument> _inspDocumentRepository; 
        private readonly IEntityRepository<TblCUDocument> _cuDocumentRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        private readonly IConfiguration _configure;

        public IdmFileService(IEntityRepository<TblDCDocument> dcDocumentRepository,
                               IEntityRepository<TblLdDocument> ldDocumentRepository,
                               IEntityRepository<TblUCDocument> ucDocumentRepository, IMapper mapper, IUnitOfWork work,
                               IConfiguration configure,UserInfo UserInfo, IEntityRepository<TblINSPDocument> inspDocumentRepository, IEntityRepository<TblCUDocument> cuDocumentRepository)
        {
            _ldDocumentRepository = ldDocumentRepository;
            _acDocumentRepository = ucDocumentRepository;
            _dcDocumentRepository = dcDocumentRepository;
            _mapper = mapper;
            _work = work;
            _configure = configure;
            _userInfo = UserInfo;
            _inspDocumentRepository = inspDocumentRepository;
            _cuDocumentRepository = cuDocumentRepository;
        }


        public async Task<bool> DeleteFileAsync(string mainModule, string documentUniqueId, IConfiguration configure, CancellationToken token)
        {
            bool result;
            switch (mainModule)
            {
                case Document.LegalDocumentation:
                    var ldDoc = await _ldDocumentRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == documentUniqueId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                    if (ldDoc != null)
                    {
                        string folderpath = $"{FileTypeExtension.GetArchiveFilePath(DocumentTypeEnum.LegalDocument1, configure)}";
                        string archivedPath = $"{folderpath}\\{ldDoc.FileName}";
                        try
                        {
                            if (!Directory.Exists(folderpath))
                                Directory.CreateDirectory(folderpath);

                            File.Move(ldDoc.FilePath, archivedPath);
                        }
                        finally
                        {
                            ldDoc.IsDeleted = true;
                            ldDoc.IsActive = false;
                            ldDoc.ModifiedDate = DateTime.UtcNow;
                            ldDoc.ModifiedBy = _userInfo.Name;
                            await _ldDocumentRepository.UpdateAsync(ldDoc, token).ConfigureAwait(false);
                            await _work.CommitAsync(token).ConfigureAwait(false);
                        }
                        result = true;
                        return result;
                    }
                    break;
                case Document.AuditClearance:
                    var acDoc = await _acDocumentRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == documentUniqueId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                    if (acDoc != null)
                    {
                        string folderpath = $"{FileTypeExtension.GetArchiveFilePath(DocumentTypeEnum.AuditClearance, configure)}";
                        string archivedPath = $"{folderpath}\\{acDoc.FileName}";
                        try
                        {
                            if (!Directory.Exists(folderpath))
                                Directory.CreateDirectory(folderpath);

                            File.Move(acDoc.FilePath, archivedPath);
                        }
                        finally
                        {
                            acDoc.IsDeleted = true;
                            acDoc.IsActive = false;
                            acDoc.ModifiedDate = DateTime.UtcNow;
                            acDoc.ModifiedBy = _userInfo.Name;
                            await _acDocumentRepository.UpdateAsync(acDoc, token).ConfigureAwait(false);
                            await _work.CommitAsync(token).ConfigureAwait(false);
                        }
                        result = true;
                        return result;
                    }
                    break;
                case Document.DisbursementCondition:
                    var dcDoc = await _dcDocumentRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == documentUniqueId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                    if (dcDoc != null)
                    {
                        string folderpath = $"{FileTypeExtension.GetArchiveFilePath(DocumentTypeEnum.DisbursementCondition, configure)}";
                        string archivedPath = $"{folderpath}\\{dcDoc.FileName}";
                        try
                        {
                            if (!Directory.Exists(folderpath))
                                Directory.CreateDirectory(folderpath);

                            File.Move(dcDoc.FilePath, archivedPath);
                        }
                        finally
                        {
                            dcDoc.IsDeleted = true;
                            dcDoc.IsActive = false;
                            dcDoc.ModifiedDate = DateTime.UtcNow;
                            dcDoc.ModifiedBy = _userInfo.Name;
                            await _dcDocumentRepository.UpdateAsync(dcDoc, token).ConfigureAwait(false);
                            await _work.CommitAsync(token).ConfigureAwait(false);
                        }
                        result = true;
                        return result;
                    }
                    break;
                case Document.InspectionOfUnit:
                    var inspDoc = await _inspDocumentRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == documentUniqueId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                    if (inspDoc != null)
                    {
                        string folderpath = $"{FileTypeExtension.GetArchiveFilePath(DocumentTypeEnum.InspectionOfUnit, configure)}";
                        string archivedPath = $"{folderpath}\\{inspDoc.FileName}";
                        try
                        {
                            if (!Directory.Exists(folderpath))
                                Directory.CreateDirectory(folderpath);

                            File.Move(inspDoc.FilePath, archivedPath);
                        }
                        finally
                        {
                            inspDoc.IsDeleted = true;
                            inspDoc.IsActive = false;
                            inspDoc.ModifiedDate = DateTime.UtcNow;
                            inspDoc.ModifiedBy = _userInfo.Name;
                            await _inspDocumentRepository.UpdateAsync(inspDoc, token).ConfigureAwait(false);
                            await _work.CommitAsync(token).ConfigureAwait(false);
                        }
                        result = true;
                        return result;
                    }
                    break;
                case Document.ChangeOfUnit:
                    var cuDoc = await _cuDocumentRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == documentUniqueId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                    if (cuDoc != null)
                    {
                        string folderpath = $"{FileTypeExtension.GetArchiveFilePath(DocumentTypeEnum.ChangeOfUnit, configure)}";
                        string archivedPath = $"{folderpath}\\{cuDoc.FileName}";
                        try
                        {
                            if (!Directory.Exists(folderpath))
                                Directory.CreateDirectory(folderpath);

                            File.Move(cuDoc.FilePath, archivedPath);
                        }
                        finally
                        {
                            cuDoc.IsDeleted = true;
                            cuDoc.IsActive = false;
                            cuDoc.ModifiedDate = DateTime.UtcNow;
                            cuDoc.ModifiedBy = _userInfo.Name;
                            await _cuDocumentRepository.UpdateAsync(cuDoc, token).ConfigureAwait(false);
                            await _work.CommitAsync(token).ConfigureAwait(false);
                        }
                        result = true;
                        return result;
                    }
                    break;
            }
            return result = false;
        }

        public async Task<IdmFileUploadDto> GetEncryptedFileById(string mainModule, string documentId, CancellationToken token)
        {
            switch(mainModule)
            {
                case Document.LegalDocumentation:
                    var ldDoc = await _ldDocumentRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == documentId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                    if (ldDoc != null)
                    {
                        var byteArray = await FileSecurity.EncryptedBytesFromFilePath(ldDoc.FilePath, "password");
                        if (byteArray == null)
                        {
                            throw new FileSecurityExceptions(DocumentError.Encryptionfailed);
                        }
                        return new IdmFileUploadDto
                        {
                            Bytes = byteArray,
                            SubModuleId = ldDoc.SubModuleId,
                            SubModuleType = ldDoc.SubModuleType,
                            MainModule = ldDoc.MainModule
                        };
                    }
                    break;
                case Document.AuditClearance:
                    var acDoc = await _acDocumentRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == documentId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                    if (acDoc != null)
                    {
                        var byteArray = await FileSecurity.EncryptedBytesFromFilePath(acDoc.FilePath, "password");
                        if (byteArray == null)
                        {
                            throw new FileSecurityExceptions(DocumentError.Encryptionfailed);
                        }
                        return new IdmFileUploadDto
                        {
                            Bytes = byteArray,
                            SubModuleId = acDoc.SubModuleId,
                            SubModuleType = acDoc.SubModuleType,
                            MainModule = acDoc.MainModule
                        };
                    }
                    break;
                case Document.DisbursementCondition:
                    var dcDoc = await _dcDocumentRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == documentId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                    if (dcDoc != null)
                    {
                        var byteArray = await FileSecurity.EncryptedBytesFromFilePath(dcDoc.FilePath, "password");
                        if (byteArray == null)
                        {
                            throw new FileSecurityExceptions(DocumentError.Encryptionfailed);
                        }
                        return new IdmFileUploadDto
                        {
                            Bytes = byteArray,
                            SubModuleId = dcDoc.SubModuleId,
                            SubModuleType = dcDoc.SubModuleType,
                            MainModule = dcDoc.MainModule
                        };
                    }
                    break;
            case Document.InspectionOfUnit:
                    var inspDoc = await _inspDocumentRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == documentId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                    if (inspDoc != null)
                    {
                        var byteArray = await FileSecurity.EncryptedBytesFromFilePath(inspDoc.FilePath, "password");
                        if (byteArray == null)
                        {
                            throw new FileSecurityExceptions(DocumentError.Encryptionfailed);
                        }
                        return new IdmFileUploadDto
                        {
                            Bytes = byteArray,
                            SubModuleId = inspDoc.SubModuleId,
                            SubModuleType = inspDoc.SubModuleType,
                            MainModule = inspDoc.MainModule
                        };
                    }
                    break;
                case Document.ChangeOfUnit:
                    var cuDoc = await _cuDocumentRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == documentId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                    if (cuDoc != null)
                    {
                        var byteArray = await FileSecurity.EncryptedBytesFromFilePath(cuDoc.FilePath, "password");
                        if (byteArray == null)
                        {
                            throw new FileSecurityExceptions(DocumentError.Encryptionfailed);
                        }
                        return new IdmFileUploadDto
                        {
                            Bytes = byteArray,
                            SubModuleId = cuDoc.SubModuleId,
                            SubModuleType = cuDoc.SubModuleType,
                            MainModule = cuDoc.MainModule
                        };
                    }
                    break;
            }
            throw new FileSecurityExceptions(DocumentError.Encryptionfailed);
        }

        public async Task<List<ldDocumentDto>> GetFileListAsync(string MainModule, CancellationToken token)
        {
            var result = new List<ldDocumentDto>();
            switch (MainModule)
            {
                case Document.LegalDocumentation:
                    var ldDoc = await _ldDocumentRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false && x.MainModule == MainModule, token).ConfigureAwait(false);
                    if (ldDoc.Count > 0)
                        return result = _mapper.Map<List<ldDocumentDto>>(ldDoc);
                    break;
                case Document.AuditClearance:
                    var acDoc = await _acDocumentRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false && x.MainModule == MainModule, token).ConfigureAwait(false);
                    if (acDoc.Count > 0)
                        return result = _mapper.Map<List<ldDocumentDto>>(acDoc);
                    break;
                case Document.DisbursementCondition:
                    var dcDoc = await _dcDocumentRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false && x.MainModule == MainModule, token).ConfigureAwait(false);
                    if (dcDoc.Count > 0)
                        return result = _mapper.Map<List<ldDocumentDto>>(dcDoc);
                    break;
                case Document.InspectionOfUnit:
                    var inspDoc = await _inspDocumentRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false && x.MainModule == MainModule, token).ConfigureAwait(false);
                    if (inspDoc.Count > 0)
                        return result = _mapper.Map<List<ldDocumentDto>>(inspDoc);
                    break;
                case Document.ChangeOfUnit:
                    var cuDoc = await _cuDocumentRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false && x.MainModule == MainModule, token).ConfigureAwait(false);
                    if (cuDoc.Count > 0)
                        return result = _mapper.Map<List<ldDocumentDto>>(cuDoc);
                    break;
            }
            return result;
        }

        public async Task<ldDocumentDto> UploadFile(byte[] bytes, int subModuleId, string subModuleType, string mainModule, IConfiguration configure, CancellationToken token)
        {
            try
            {
                if (!FileTypeExtension.ValidateFileSize(bytes.Length, FileTypesEnum.Pdf, configure))
                {
                    var size = FileTypeExtension.GetFileSize(FileTypesEnum.Pdf, configure);
                    throw new FileSecurityExceptions(_configure["FileSize:Message"]);
                }

                ldDocumentDto ldDocumentDto = new();

                switch (mainModule)
                {
                    case Document.LegalDocumentation:

                        string filePath = FileTypeExtension.GetFilePath(DocumentTypeEnum.LegalDocument1, configure);

                        string fileName = FileTypeExtension.GetFileName(DocumentTypeEnum.LegalDocument1, subModuleId);

                        string filepath = $"{filePath}\\{fileName}.{FileTypesEnum.Pdf.ToString()}";

                        var documentData = new TblLdDocument()
                        {

                            SubModuleId = subModuleId,
                            SubModuleType = subModuleType,
                            MainModule = mainModule,
                            FileName = $"{fileName}.{FileTypesEnum.Pdf.ToString()}",
                            FilePath = filepath,
                            FileType = FileTypesEnum.Pdf.ToString(),
                            UniqueId = Guid.NewGuid().ToString(),
                            IsActive = true,
                            IsDeleted = false,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = _userInfo.Name
                        };
                        var data = await _ldDocumentRepository.AddAsync(documentData, token).ConfigureAwait(false);

                        ldDocumentDto = _mapper.Map<ldDocumentDto>(data);
                        //Decrypting the byte array
                        var byteArray = FileSecurity.DecryptByteArray(bytes, "password");
                        //Save byte array
                        var result = await FileSecurity.SaveBytesToFile(filepath, byteArray);
                        break;

                    case Document.AuditClearance:
                        string filePath1 = FileTypeExtension.GetFilePath(DocumentTypeEnum.AuditClearance, configure);

                        string fileName1 = FileTypeExtension.GetFileName(DocumentTypeEnum.AuditClearance, subModuleId);

                        string filepath1 = $"{filePath1}\\{fileName1}.{FileTypesEnum.Jpeg.ToString()}";

                        var UCdocumentData = new TblUCDocument()
                        {

                            SubModuleId = subModuleId,
                            SubModuleType = subModuleType,
                            MainModule = mainModule,
                            FileName = $"{fileName1}.{FileTypesEnum.Pdf.ToString()}",
                            FilePath = filepath1,
                            FileType = FileTypesEnum.Pdf.ToString(),
                            UniqueId = Guid.NewGuid().ToString(),
                            IsActive = true,
                            IsDeleted = false,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = _userInfo.Name
                        };

                        var ucdata = await _acDocumentRepository.AddAsync(UCdocumentData, token).ConfigureAwait(false);
                        ldDocumentDto = _mapper.Map<ldDocumentDto>(ucdata);
                        //Decrypting the byte array
                        var byteArray1 = FileSecurity.DecryptByteArray(bytes, "password");
                        //Save byte array
                        var result1 = await FileSecurity.SaveBytesToFile(filepath1, byteArray1);
                        break;
                    case Document.DisbursementCondition:
                        string filePath2 = FileTypeExtension.GetFilePath(DocumentTypeEnum.DisbursementCondition, configure);

                        string fileName2 = FileTypeExtension.GetFileName(DocumentTypeEnum.DisbursementCondition, subModuleId);

                        string filepath2 = $"{filePath2}\\{fileName2}.{FileTypesEnum.Pdf.ToString()}";
                        var DCdocumentData = new TblDCDocument()
                        {

                            SubModuleId = subModuleId,
                            SubModuleType = subModuleType,
                            MainModule = mainModule,
                            FileName = $"{fileName2}.{FileTypesEnum.Pdf.ToString()}",
                            FilePath = filepath2,
                            FileType = FileTypesEnum.Pdf.ToString(),
                            UniqueId = Guid.NewGuid().ToString(),
                            IsActive = true,
                            IsDeleted = false,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = _userInfo.Name
                        };
                        var dcdata = await _dcDocumentRepository.AddAsync(DCdocumentData, token).ConfigureAwait(false);
                        ldDocumentDto = _mapper.Map<ldDocumentDto>(dcdata);
                        //Decrypting the byte array
                        var byteArray2 = FileSecurity.DecryptByteArray(bytes, "password");
                        //Save byte array
                        var result2 = await FileSecurity.SaveBytesToFile(filepath2, byteArray2);

                        break;

                    case Document.InspectionOfUnit:
                        string filePathInspectionDetail = FileTypeExtension.GetFilePath(DocumentTypeEnum.InspectionOfUnit, configure);

                        string fileNameInspectionDetail = FileTypeExtension.GetFileName(DocumentTypeEnum.InspectionOfUnit, subModuleId);

                        string filepathInspectionDetail1 = $"{filePathInspectionDetail}\\{fileNameInspectionDetail}.{FileTypesEnum.Pdf.ToString()}";
                        var INSPdocumentData = new TblINSPDocument()
                        {

                            SubModuleId = subModuleId,
                            SubModuleType = subModuleType,
                            MainModule = mainModule,
                            FileName = $"{fileNameInspectionDetail}.{FileTypesEnum.Pdf.ToString()}",
                            FilePath = filepathInspectionDetail1,
                            FileType = FileTypesEnum.Pdf.ToString(),
                            UniqueId = Guid.NewGuid().ToString(),
                            IsActive = true,
                            IsDeleted = false,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = _userInfo.Name
                        };
                        var inspdata = await _inspDocumentRepository.AddAsync(INSPdocumentData, token).ConfigureAwait(false);
                        ldDocumentDto = _mapper.Map<ldDocumentDto>(inspdata);
                        //Decrypting the byte array
                        var byteArrayinsp = FileSecurity.DecryptByteArray(bytes, "password");
                        //Save byte array
                        var resultInsp = await FileSecurity.SaveBytesToFile(filepathInspectionDetail1, byteArrayinsp);
                        break;

                    case Document.ChangeOfUnit:
                        string changeofunitfilePath = FileTypeExtension.GetFilePath(DocumentTypeEnum.ChangeOfUnit, configure);

                        string changeofunitfileName = FileTypeExtension.GetFileName(DocumentTypeEnum.ChangeOfUnit, subModuleId);

                        string changeofunitfilePath1 = $"{changeofunitfilePath}\\{changeofunitfileName}.{FileTypesEnum.Jpeg.ToString()}";

                        var CUphotoData = new TblCUDocument()
                        {

                            SubModuleId = subModuleId,
                            SubModuleType = subModuleType,
                            MainModule = mainModule,
                            FileName = $"{changeofunitfileName}.{FileTypesEnum.Jpeg.ToString()}",
                            FilePath = changeofunitfilePath1,
                            FileType = FileTypesEnum.Jpeg.ToString(),
                            UniqueId = Guid.NewGuid().ToString(),
                            IsActive = true,
                            IsDeleted = false,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = _userInfo.Name
                        };

                        var cudata = await _cuDocumentRepository.AddAsync(CUphotoData, token).ConfigureAwait(false);
                        ldDocumentDto = _mapper.Map<ldDocumentDto>(cudata);
                        //Decrypting the byte array
                        var cubyteArray = FileSecurity.DecryptByteArray(bytes, "password");
                        //Save byte array
                        var curesult = await FileSecurity.SaveBytesToFile(changeofunitfilePath1, cubyteArray);
                        break;

                }
                await _work.CommitAsync(token).ConfigureAwait(false);
                return ldDocumentDto;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }



      
    }
}
