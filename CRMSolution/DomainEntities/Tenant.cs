using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Tenant
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsProcessingMobileData { get; set; }
        public bool IsTransformingDataFeed { get; set; }
        public System.DateTime MobileDataProcessingAt { get; set; }
        public bool IsSMSEnabled { get; set; }
        public bool IsSendingSMS { get; set; }
        public System.DateTime SMSProcessingAt { get; set; }
        public bool IsUploadingImage { get; set; }
        public System.DateTime UploadingImageAt { get; set; }
    }
}
