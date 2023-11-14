using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace EpicCrmWebApi
{
    [Authorize]
    public class EORPController : BaseApiController
    {
        /// <summary>
        /// Get Expense Order Return Payment Data - used for dashboard summary
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public EORPResponse DataByDay([FromUri]SearchCriteria searchCriteria)
        {
            DomainEntities.SearchCriteria criteria = Helper.ParseSearchCriteria(searchCriteria);

            // create skeleton response data
            EORPResponse responseData = new EORPResponse()
            {
                DateTimeUtc = DateTime.UtcNow,
                Content = "Bad Request",
                StatusCode = HttpStatusCode.BadRequest,
                EORPSummary = null,
                EORPDays = null
            };

            try
            {
                DateTime startDate = Utils.ConvertStringToDate(searchCriteria.DateFrom);
                DateTime endDate = Utils.ConvertStringToDate(searchCriteria.DateTo);
                if ((startDate == DateTime.MinValue || endDate == DateTime.MinValue) || endDate < startDate)
                {
                    responseData.Content = "Invalid Date / Range";
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }

                EORP eorp = Business.GetEORPData(criteria);


                // put data in response
                responseData.Startdate = eorp.StartDate;
                responseData.EndDate = eorp.EndDate;
                responseData.DayCount = eorp.DayCount;
                responseData.EORPSummary = new EORPSummaryResponse();

                responseData.EORPSummary.ExpenseTotal = eorp.EORPSummary.ExpenseTotal;
                responseData.EORPSummary.ExpenseAverage = Math.Round(eorp.EORPSummary.ExpenseAverage, 2);

                responseData.EORPSummary.ExpenseTotalDisplay = $"{responseData.EORPSummary.ExpenseTotal:N}";
                responseData.EORPSummary.ExpenseAverageDisplay = $"{responseData.EORPSummary.ExpenseAverage:N}";
                //

                responseData.EORPSummary.OrderTotal = eorp.EORPSummary.OrderTotal;
                responseData.EORPSummary.OrderAverage = Math.Round(eorp.EORPSummary.OrderAverage, 2);

                responseData.EORPSummary.OrderTotalDisplay = $"{responseData.EORPSummary.OrderTotal:N}";
                responseData.EORPSummary.OrderAverageDisplay = $"{responseData.EORPSummary.OrderAverage:N}";
                //

                responseData.EORPSummary.ReturnOrderTotal = eorp.EORPSummary.ReturnOrderTotal;
                responseData.EORPSummary.ReturnOrderAverage = Math.Round(eorp.EORPSummary.ReturnOrderAverage, 2);

                responseData.EORPSummary.ReturnOrderTotalDisplay = $"{responseData.EORPSummary.ReturnOrderTotal:N}";
                responseData.EORPSummary.ReturnOrderAverageDisplay = $"{responseData.EORPSummary.ReturnOrderAverage:N}";
                //

                responseData.EORPSummary.PaymentTotal = eorp.EORPSummary.PaymentTotal;
                responseData.EORPSummary.PaymentAverage = Math.Round(eorp.EORPSummary.PaymentAverage, 2);

                responseData.EORPSummary.PaymentTotalDisplay = $"{responseData.EORPSummary.PaymentTotal:N}";
                responseData.EORPSummary.PaymentAverageDisplay = $"{responseData.EORPSummary.PaymentAverage:N}";
                //

                responseData.EORPDays = eorp.EORPDays.Select(x => new EORPDayResponse()
                {
                    Date = x.Date.ToString("dd/MM/yyyy"),
                    ExpenseAmount = x.ExpenseAmount,
                    OrderAmount = x.OrderAmount,
                    ReturnOrderAmount = x.ReturnOrderAmount,
                    PaymentAmount = x.PaymentAmount
                }).ToList();

                responseData.Content = "";
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch(Exception ex)
            {
                BusinessLayer.Business.LogError(nameof(DataByDay), ex.ToString(), " ");
                responseData.Content = ex.ToString();
            }

            return responseData;
        }

        [HttpGet]
        public EORPByMonth DataByMonth([FromUri]SearchCriteria searchCriteria)
        {
            DomainEntities.SearchCriteria criteria = Helper.ParseSearchCriteria(searchCriteria);

            // create skeleton response data
            EORPByMonth responseData = new EORPByMonth()
            {
                DateTimeUtc = DateTime.UtcNow,
                Content = "Bad Request",
                StatusCode = HttpStatusCode.BadRequest,
                EORPMonths = null
            };

            try
            {
                DateTime startDate = Utils.ConvertStringToDate(searchCriteria.DateFrom);
                DateTime endDate = Utils.ConvertStringToDate(searchCriteria.DateTo);
                int monthsInDates = Utils.MonthsInDates(startDate, endDate);
                if (monthsInDates <= 0 || monthsInDates > 12)
                {
                    responseData.Content = "InvalidDates";
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }

                EORP eorp = Business.GetEORPData(criteria);

                responseData.EORPMonths = eorp.EORPMonthlySummary.Select(x => new EORPMonthResponse()
                {
                    DisplayMonth = x.Date.ToString("MMM\\'yy"),
                    ExpenseAmount = x.ExpenseAmount,
                    OrderAmount = x.OrderAmount,
                    ReturnOrderAmount = x.ReturnOrderAmount,
                    PaymentAmount = x.PaymentAmount
                }).ToList();

                responseData.Content = "";
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(DataByMonth), ex.ToString(), " ");
                responseData.Content = ex.ToString();
            }

            return responseData;
        }
    }
}
