using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EntityCropModel
    {
        public long Id { get; set; }
        public long EntityId { get; set; }
        public string CropName { get; set; }
    }
}