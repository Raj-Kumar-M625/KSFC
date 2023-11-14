using Application.DTOs.Bank;
using Domain.Bank;
using Domain.Bill;
using Presentation.Extensions.Bill;
using Presentation.Utils;
using System;
using System.Linq;

namespace Presentation.GridFilters.Reconcile
{
    public class ReconcileSearchFilters
    {
        public FilterBuilder<BankTransactions> GetReconcileSearchCriteria(IQueryable<BankTransactions> list, ReconcileFilter Reconcilefilter)
        {
            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);
            DateTime dt;
            if (Reconcilefilter.Date != null)
            {

                var date = Reconcilefilter.Date.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    startDate = DateTime.Parse(date);
                else
                    startDate = DateTime.Parse(Reconcilefilter.Date.Substring(0, 9));

                var dateend = Reconcilefilter.Date.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    endDate = DateTime.Parse(dateend);
                else
                    endDate = DateTime.Parse(Reconcilefilter.Date.Substring(12, 11));

            }
            Reconcilefilter.forder = Reconcilefilter.forder ?? new string[] { };
            IQueryable<BankTransactions> Query = list.AsQueryable();
            var frow = new ReconcileFilterRow();
            var filterBuilder = new FilterBuilder<BankTransactions>();
            filterBuilder
               .Add("Reconcilefilter.Date", q => Reconcilefilter.Date != null ? q.Where(o => o.Transaction_Date >= startDate && o.Transaction_Date <= endDate) : q)
                .Add("Reconcilefilter.TransactionType", q => Reconcilefilter.TransactionType != null ? Reconcilefilter.TransactionType == "Credit" ? q.Where(o => o.Credit != null) : q.Where(o => o.Debit != null) : q)
               .Add("Reconcilefilter.Status", q => Reconcilefilter.Status != null ? q.Where(o => (o.Status).Equals(Reconcilefilter.Status)) : q)
               .Add("Reconcilefilter.Bankname", q => Reconcilefilter.Bankname != null ? q.Where(o => o.BankName.Equals(Reconcilefilter.Bankname)) : q)
               .Add("Reconcilefilter.Description", q => Reconcilefilter.Description != null ? q.Where(o => o.Description.Equals(Reconcilefilter.Description)) : q)
               .Add("Reconcilefilter.PayableAmount", q => q.Where(o => o.Credit != null ? o.Credit >= (Reconcilefilter.PayableAmount) : o.Debit >= Reconcilefilter.PayableAmount))
               .Add("Reconcilefilter.PayableMaxAmount", q => q.Where(o => o.Credit != null ? o.Credit <= (Reconcilefilter.PayableMaxAmount) : o.Debit <= Reconcilefilter.PayableMaxAmount))
               .Add("Reconcilefilter.AccountNumber", q => Reconcilefilter.AccountNumber != null ? q.Where(o => o.AccountNo.Equals(Reconcilefilter.AccountNumber)) : q);
            return filterBuilder;
        }

    }
}
