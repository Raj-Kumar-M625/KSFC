using DomainEntities;
using System;

namespace BusinessLayer
{
    public interface IParse
    {
        void Parse(ExcelUploadStatus row, int maxErrors);
    }
}
