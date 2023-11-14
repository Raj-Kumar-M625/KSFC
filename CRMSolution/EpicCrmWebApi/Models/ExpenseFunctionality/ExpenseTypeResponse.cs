using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicCrmWebApi
{
    /// <summary>
    /// This class is used to return response of Get All Valid Expense Types
    /// 
    /// </summary>
    public class ExpenseTypeResponse : MinimumResponse
    {
        public int ItemCount { get; set; }
        public IEnumerable<ExpenseType> ExpenseTypes { get; set; }
    }
}
