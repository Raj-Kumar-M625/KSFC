using Domain.Adjustment;
using Domain.Master;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.Adjustment
{
    public class AdjustmentStatusModel
    {
        
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
        public virtual AdjustmentViewModel Adjustment { get; set; }
    }
}
