using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EpicCrmWebApi
{
    public class BulkApproveExpensesModel
    {
        [Display(Name = "No Of Selected Employees")]

        public int SelectedEmployees { get; set; }
        [Display(Name = "Total Amount")]

        public decimal TotalAmount { get; set; }

        public IEnumerable<long>  ExpenseId { get; set; }
    }
}