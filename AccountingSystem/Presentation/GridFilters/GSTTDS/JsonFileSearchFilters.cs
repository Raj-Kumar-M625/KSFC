using Domain.Payment;
using Presentation.Utils;
using System.Linq;
using System;
using Domain.GSTTDS;
using Presentation.GridFilters.GSTTDS;
using Application.DTOs.GSTTDS;
using Presentation.Extensions.Bill;

namespace Presentation.GridFilters.TDS
{
    /// <summary>
    /// Purpose = Json File Search criteria
    /// Author = Karthick J 
    /// Date = 13 09 2022  
    /// </summary>
    public class JsonFileSearchFilters
    {
        /// <summary>
        /// Purpose = Json File Search criteria
        /// Author = Karthick J 
        /// Date = 13 09 2022   
        /// </summary>
        /// <param name="gstTdsPaymentChallanList"></param>
        /// <param name="jsonFilefilters"></param>
        /// <returns></returns>
        public FilterBuilder<GsstPaymentChallanDto> GetJsonFileSearchCriteria(IQueryable<GsstPaymentChallanDto> gstTdsPaymentChallanList, JsonFileFilter jsonFilefilters)
         {


            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);
             DateTime dt;
            if (jsonFilefilters.createDateFrom != null)
            {

                var date = jsonFilefilters.createDateFrom.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    startDate = DateTime.Parse(date);
                else
                    startDate = DateTime.Parse(jsonFilefilters.createDateFrom.Substring(0, 9));

                var dateend = jsonFilefilters.createDateFrom.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    endDate = DateTime.Parse(dateend);
                else
                    endDate = DateTime.Parse(jsonFilefilters.createDateFrom.Substring(12, 11));

            }





            jsonFilefilters.forder ??= new string[] { };

            //IQueryable<GsstPaymentChallanDto> Query = gstTdsPaymentChallanList.AsQueryable();
            var filterBuilder = new FilterBuilder<GsstPaymentChallanDto>();
            filterBuilder
                .Add("jsonFileFilters.createDateFrom", q => jsonFilefilters.createDateFrom != null ? q.Where(o => o.GSTTDSPaymentChallan.CreatedOn >= startDate && o.GSTTDSPaymentChallan.CreatedOn <= endDate) : q)
               // .Add("jsonFileFilters.createDateTo", q => jsonFilefilters.createDateTo != DateTime.MinValue ? q.Where(o => o.GSTTDSPaymentChallan.CreatedOn <= jsonFilefilters.createDateTo) : q)
                .Add("jsonFileFilters.noOfVendors", q => jsonFilefilters.noOfVendors > 0 ? q.Where(o => o.GSTTDSPaymentChallan.NoOfVendors.Equals(jsonFilefilters.noOfVendors)) : q)
                .Add("jsonFileFilters.noOfTrans", q => jsonFilefilters.noOfTrans > 0 ? q.Where(o => o.GSTTDSPaymentChallan.NoOfTrans.Equals(jsonFilefilters.noOfTrans)): q)
                .Add("jsonFileFilters.acknowledgementRefNo", q => jsonFilefilters.acknowledgementRefNo != null ? q.Where(o => o.GSTTDSPaymentChallan.AcknowledgementRefNo.Contains(jsonFilefilters.acknowledgementRefNo)) : q)
                .Add("jsonFileFilters.gstTdsMinAmount", q => jsonFilefilters.gstTdsMinAmount > 0 ? q.Where(o => o.GSTTDSPaymentChallan.TotalGSTTDSPayment >= jsonFilefilters.gstTdsMinAmount) : q)
                .Add("jsonFileFilters.gstTdsMaxAmount", q => jsonFilefilters.gstTdsMaxAmount > 0 ? q.Where(o => o.GSTTDSPaymentChallan.TotalGSTTDSPayment <= jsonFilefilters.gstTdsMaxAmount) : q)
                .Add("jsonFileFilters.gstTdsStatus", q => jsonFilefilters.gstTdsStatus != null ? q.Where(o => o.GSTTDSPaymentChallan.GSTTDSStatus.StatusMaster.CodeName.Contains(jsonFilefilters.gstTdsStatus)) : q)
                .Add("jsonFileFilters.utrNo", q => jsonFilefilters.utrNo != null ? q.Where(o => o.GSTTDSPaymentChallan.UTRNo.Contains(jsonFilefilters.utrNo)) : q);

            return filterBuilder;


        }
    }
}
