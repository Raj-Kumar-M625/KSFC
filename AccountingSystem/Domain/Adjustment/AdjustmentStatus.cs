using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Master;
using Domain.Vendor;

namespace Domain.Adjustment
{
    public class AdjustmentStatus
    {
        [Key]
        public int ID { get; set; }


        [ForeignKey("Adjustment")]
        public int AdjustmentID { get; set; }
        public int VendorID { get; set; }

        [ForeignKey(nameof(StatusMaster))]
        public int StatusCMID { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public virtual CommonMaster StatusMaster { get; set; }
        public virtual Adjustments Adjustment { get; set; }
    }
}
