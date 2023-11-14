using KAR.KSFC.Components.Common.Dto.Enums;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails
{
    public class FileUploadDTO
    {
        public int EnquiryId { get; set; }
        public byte[] Bytes { get; set; }
        public DocumentTypeEnum DocumentType { get; set; }
        public FileTypesEnum FileType { get; set; }
    }
}
