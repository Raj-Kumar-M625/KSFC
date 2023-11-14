using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ExcelUploadError
    {
        public long Id { get; set; }
        public long ExcelUploadStatusId { get; set; }

        public string MessageType { get; set; }
        public string CellReference { get; set; }
        public string ExpectedValue { get; set; }
        public string ActualValue { get; set; }
        public string Description { get; set; }
    }
}
