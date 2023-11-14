using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    /// <summary>
    /// Hierarchy Chair Master, For Workflow - Forwarding Task
    /// </summary>
    public class HierarchyMasterDto
    {
        /// <summary>
        /// HierarchyId 
        /// </summary>
        /// 


        [DisplayName("Id")]
        [Required(ErrorMessage = "The Id is required")]
        public int Id { get; set; }

        [DisplayName("From ChairCode")]
        [Required(ErrorMessage = "The From ChairCode is required")]
        public int FromChairCode { get; set; }

        [DisplayName("To ChairCode")]
        [Required(ErrorMessage = "The To ChairCode is required")]
        public int ToChairCode { get; set; }    
    }
}
