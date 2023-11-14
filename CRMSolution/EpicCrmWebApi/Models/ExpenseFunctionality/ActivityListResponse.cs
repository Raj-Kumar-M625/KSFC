using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicCrmWebApi
{
    /// <summary>
    /// This class is used to return the response for Expense functionality
    /// to return a list of activities
    /// </summary>
    public class ActivityListResponse : MinimumResponse
    {
        public int ItemCount { get; set; }
        public IEnumerable<ActivityRecord> Activities { get; set; }
    }
}
