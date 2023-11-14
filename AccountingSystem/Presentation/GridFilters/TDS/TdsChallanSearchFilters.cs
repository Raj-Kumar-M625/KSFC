using Domain.Bill;
using Domain.Payment;
using Presentation.Extensions.Vendor;
using Presentation.Utils;
using System.Linq;
using System;
using Application.DTOs.TDS;
using Presentation.Extensions.Bill;

namespace Presentation.GridFilters.TDS
{
    /// <summary>
    /// Purpose = TDS Challan Search criteria
    /// Author = Karthick J 
    /// Date = 01 09 2022  
    /// </summary>
    public class TdsChallanSearchFilters
    {
        /// <summary>
        /// Purpose = TDS Challan Search criteria
        /// Author = Karthick J 
        /// Date = 01 09 2022   
        /// </summary>
        /// <param name="challanList"></param>
        /// <param name="challanfilters"></param>
        /// <returns></returns>
        public FilterBuilder<TdssPaymentChallanDto> GetTDSChallanSearchCriteria(IQueryable<TdssPaymentChallanDto> challanList, TdsChallanFilter challanfilters)
        {

            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);
            DateTime dt;
            if (challanfilters.challanDate != null)
            {

                var date = challanfilters.challanDate.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    startDate = DateTime.Parse(date);
                else
                    startDate = DateTime.Parse(challanfilters.challanDate.Substring(0, 9));

                var dateend = challanfilters.challanDate.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    endDate = DateTime.Parse(dateend);
                else
                    endDate = DateTime.Parse(challanfilters.challanDate.Substring(12, 11));

            }





            challanfilters.forder = challanfilters.forder ?? new string[] { };

           // IQueryable<TdssPaymentChallanDto> Query = challanList.AsQueryable();
            var filterBuilder = new FilterBuilder<TdssPaymentChallanDto>();
            filterBuilder
                .Add("challanFilters.tdsSection", q => challanfilters.tdsSection != null ? q.Where(o => o.TDSPaymentChallan.TDSSection.Equals(challanfilters.tdsSection)) : q)
                .Add("challanFilters.noOfVendors", q => challanfilters.noOfVendors > 0 ? q.Where(o => o.TDSPaymentChallan.NoOfVendors.Equals(challanfilters.noOfVendors)) : q)
                .Add("challanFilters.noOfTransactions", q => challanfilters.noOfTransactions > 0 ? q.Where(o => o.TDSPaymentChallan.NoOfTrans.Equals(challanfilters.noOfTransactions)) : q)
                .Add("challanFilters.payableMinAmount", q => challanfilters.payableMinAmount > 0 ? q.Where(o => o.TDSPaymentChallan.TotalTDSPayment >= challanfilters.payableMinAmount) : q)
                .Add("challanFilters.payableMaxAmount", q => challanfilters.payableMaxAmount > 0 ? q.Where(o => o.TDSPaymentChallan.TotalTDSPayment <= challanfilters.payableMaxAmount) : q)
                .Add("challanFilters.bankName", q => challanfilters.bankName != null ? q.Where(o => o.Bank != null && o.Bank.BankName.Equals(challanfilters.bankName)) : q)
                .Add("challanFilters.accountNo", q => challanfilters.accountNo != null ? q.Where(o => o.Bank != null && o.Bank.Accountnumber.Equals(challanfilters.accountNo)) : q)
                .Add("challanFilters.challanDate", q => challanfilters.challanDate != null ? q.Where(o => o.TDSPaymentChallan.CreatedOn.Date >= startDate && o.TDSPaymentChallan.CreatedOn.Date <= endDate) : q);

            return filterBuilder;


        }
    }
}
