using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ExpenseDetail
    {
        public long ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; }
        public decimal ExpenseAmount { get; set; }
    }
}
