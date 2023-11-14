using Application.DTOs.GenerateBankFile;
using Domain.GenarteBankfile;
using Domain.Payment;
using Presentation.Extensions.Bill;
using Presentation.Extensions.Payment;
using Presentation.GridFilters.TDS;
using Presentation.Utils;
using System;
using System.Linq;

namespace Presentation.Extensions.BankFile
{
    /// <summary>
    /// Author:Swetha M Date:06/05/2022
    /// Purpose:Payment Search Filter
    /// </summary>
    /// <returns></returns>
    public class BankFileSerachFilter
    {
        /// <summary>
        /// Author:Swetha M Date:05/11/2022
        /// Purpose:Get Bank SearchCriteria and pass it to the controller
        /// </summary>
        /// <returns></returns>
        public FilterBuilder<GenerateBankFile> GetBankFileSearchCriteria(IQueryable<GenerateBankFile> bankFileList, BankFileFilters bankFileFilters)
        {

            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);           
            DateTime dt;
            string status = string.Empty;
            if(bankFileFilters.paymentStatus == "please select")
            {
                status = "Bank File Generated";
            }
            else
            {
                status = bankFileFilters.paymentStatus;
            }

            if (bankFileFilters.fileGenDate != null)
            {
              
                var date = bankFileFilters.fileGenDate.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    startDate = DateTime.Parse(date);
                else
                    startDate = DateTime.Parse(bankFileFilters.fileGenDate.Substring(0, 9));

                var dateend = bankFileFilters.fileGenDate.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    endDate = DateTime.Parse(dateend);
                else
                    endDate = DateTime.Parse(bankFileFilters.fileGenDate.Substring(12, 11));

            }            

            bankFileFilters.forder = bankFileFilters.forder ?? new string[] { };
            Console.WriteLine(bankFileFilters.paymentStatus);
           // IQueryable<GenerateBankFile> Query = bankFileList.AsQueryable();
           // var frow = new BankFileFilterRow();
            var filterBuilder = new FilterBuilder<GenerateBankFile>();
            filterBuilder
                 .Add("bankFileFilters.noOfVendors", q => bankFileFilters.noOfVendors != 0 ? q.Where(o => o.NoOfVendors.Equals(bankFileFilters.noOfVendors)) : q)
                 .Add("bankFileFilters.noOfTransactions", q => bankFileFilters.noOfTransactions != 0 ? q.Where(o => o.NoOfTransactions.Equals(bankFileFilters.noOfTransactions)) : q)
                 .Add("bankFileFilters.fileGenDate", q => q.Where(o => o.CreatedOn.Date >= startDate && o.CreatedOn.Date <= endDate))
                  .Add("bankFileFilters.bankName", q => bankFileFilters.bankName !=null ? q.Where(o => o.Bank.BankName.Equals(bankFileFilters.bankName)) : q)
                   .Add("bankFileFilters.accountNo", q => bankFileFilters.accountNo != null ? q.Where(o => o.Bank.Accountnumber.Contains(bankFileFilters.accountNo)) : q)
                 .Add("bankFileFilters.paidAmount", q => bankFileFilters.paidAmount != 0 ? q.Where(o => o.TotalAmount.Equals(bankFileFilters.paidAmount)):q) 
                 .Add("bankFileFilters.paymentStatus", q => bankFileFilters.paymentStatus != null ? q.Where(o => o.CodeValue.Equals(status)):q) ;
            return filterBuilder;

        }
    }
    
}
