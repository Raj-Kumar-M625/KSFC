using Domain.Transactions;
using Domain.Vendor;
using Presentation.Extensions.Payment;
using Presentation.Extensions.Vendor;
using Presentation.GridFilters.TDS;
using Presentation.Utils;
using System;
using System.Linq;

namespace Presentation.GridFilters.ListOfTransaction
{
      public class ListOfTransactionSearchFilters
    {
        ///<summary>
        /// Purpose = Vendor Search criteria
        /// Author = Swetha M 
        /// Date = 29 06 2022  
        /// </summary>
        public FilterBuilder<Transaction> GetListOfTransactionSearchCriteria(IQueryable<Transaction> transactionList, ListOfTransactionFilter transactionFilter)
        {
            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);
            DateTime dt;
            if (transactionFilter.transactionDate != null)
            {

                var date = transactionFilter.transactionDate.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    startDate = DateTime.Parse(date);
                else
                    startDate = DateTime.Parse(transactionFilter.transactionDate.Substring(0, 9));

                var dateend = transactionFilter.transactionDate.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    endDate = DateTime.Parse(dateend);
                else
                    endDate = DateTime.Parse(transactionFilter.transactionDate.Substring(12, 11));
            }


            transactionFilter.forder = transactionFilter.forder ?? new string[] { };

            IQueryable<Transaction> Query = transactionList.AsQueryable();
            var frow = new ListOfTransactionFilterRow();
            var filterBuilder = new FilterBuilder<Transaction>();
            filterBuilder
                .Add("transactionFilter.minimumAmount", q => q.Where(o => o.Amount >= transactionFilter.minimumAmount))
                .Add("transactionFilter.transactionDate", q => q.Where(o => o.TransactionDate.Date >= startDate && o.TransactionDate.Date <= endDate))
                .Add("transactionFilter.maximumAmount", q => q.Where(o => o.Amount <= transactionFilter.maximumAmount))
                .Add("transactionFilter.UTRNumber", q => transactionFilter.UTRNumber != null ? q.Where(o => o.UTRNumber.Equals(transactionFilter.UTRNumber)) : q)
                .Add("transactionFilter.vendorName", q => transactionFilter.vendorName != null ? q.Where(o => o.VendorName.Contains(transactionFilter.vendorName)) : q)
                .Add("transactionFilter.status", q => transactionFilter.status != null ? q.Where(o => o.Status.Equals(transactionFilter.status)) : q)
                .Add("transactionFilter.accountNumber", q => transactionFilter.accountNumber != null ? q.Where(o => o.AccountNumber.Equals(transactionFilter.accountNumber)) : q)
                 .Add("transactionFilter.assessmentYear", q => q.Where(o => o.AssesmentYear == transactionFilter.assessmentYear))
                 .Add("transactionFilter.transactionType", q => q.Where(o => o.TransactionDetailedType == transactionFilter.transactionType))
                 .Add("transactionFilter.approvedBy", q => q.Where(o => o.ApprovedBy == transactionFilter.approvedBy))
                 .Add("transactionFilter.billNo", q => q.Where(o => o.BillNo == transactionFilter.billNo))
                 .Add("transactionFilter.referenceNumber", q => transactionFilter.referenceNumber != null ? q.Where(o => o.BillReferenceNo.Contains(transactionFilter.referenceNumber)) : q);

                  //.Add("transactionFilter.assessmentYear", q => q.Where(o => o.AssesmentYear == transactionFilter.assessmentYear)) : q);
                            
               
            return filterBuilder;


        }
    }
}
