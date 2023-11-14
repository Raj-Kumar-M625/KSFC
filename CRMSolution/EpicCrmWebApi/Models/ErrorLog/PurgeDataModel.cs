using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class PurgeDataModel
    {
        public long Id { get; set; }

        [Display(Name = "Start Date")]
        public DateTime DateFrom { get; set; }

        [Display(Name = "End Date")]
        public DateTime DateTo { get; set; }

        [Display(Name = "SqliteAction (No.)")]
        public long ActionPurged { get; set; }

        [Display(Name = "SqliteDupAction (No.)")]
        public long ActionDupPurged { get; set; }

        [Display(Name = "SqliteExpense (No.)")]
        public long ExpensePurged { get; set; }

        [Display(Name = "SqliteOrder (No.)")]
        public long OrderPurged { get; set; }

        [Display(Name = "SqlitePayment (No.)")]
        public long PaymentPurged { get; set; }

        [Display(Name = "SqliteReturn (No.)")]
        public long ReturnPurged { get; set; }

        [Display(Name = "SqliteEntity (No.)")]
        public long EntityPurged { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }
}