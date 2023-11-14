using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using BusinessLayer;
using CRMUtilities;
using System.Globalization;

namespace EpicCrmWebApi
{
    [Authorize]
    public class TOPPRSController : BaseApiController
    {
        [HttpGet]
        public TOPItemsResponse GetTopItemsData([FromUri]SearchCriteria searchCriteria, int topItemCount = 5)
        {
            DomainEntities.SearchCriteria criteria = Helper.ParseSearchCriteria(searchCriteria);

            // create skeleton response data
            TOPItemsResponse responseData = new TOPItemsResponse()
            {
                DateTimeUtc = DateTime.UtcNow,
                Content = "Bad Request",
                StatusCode = HttpStatusCode.BadRequest,
                Products = null,
                Returns = null,
                SalesPersons = null,
                SalesPersonsByPayments = null
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
              
                var topItemsData = Business.GetTopItemsData(criteria, topItemCount);

                //Top Selling Products
                responseData.Products = topItemsData.Products.Select(x => new TOPItemsData()
                {
                    Amount = $"{x.Amount:N}",
                    Name = x.Name
                }).ToList();

                //Top Returns
                responseData.Returns = topItemsData.Returns.Select(x => new TOPItemsData()
                {
                    Amount = $"{x.Amount:N}",
                    Name = x.Name
                }).ToList();

                //Top SalesPersons based on orders
                responseData.SalesPersons = topItemsData.SalesPersonsByOrders.Select(x => new TOPItemsData()
                {
                    Amount = $"{x.Amount:N}",
                    Name = $"{x.Name} ({x.Code})"
                }).ToList();

                //Top SalesPersons based on Payments
                responseData.SalesPersonsByPayments = topItemsData.SalesPersonsByPayments.Select(x => new TOPItemsData()
                {
                    Amount = $"{x.Amount:N}",
                    Name = $"{x.Name} ({x.Code})"
                }).ToList();

                responseData.Content = "";
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                BusinessLayer.Business.LogError(nameof(TOPPRSController), ex.ToString(), " ");
                responseData.Content = ex.ToString();
            }

            return responseData;
        }
    }
}
