using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using Domain.CessTransactions;
using System.Collections.Generic;
using Persistence;
using Dapper;
using Common.ConstantVariables;
using Serilog;
using System.Linq;
using System.Net;
using NPOI.OpenXmlFormats.Dml;
using System.Text;

namespace Presentation.Helpers
{
    /// <summary>
    /// Author:Swetha M Date:13/04/2023
    /// Purpose: To run the backgoudn service 
    /// </summary>
    /// <returns></returns>
    /// 
    public class AutoTaskScheduler : BackgroundService
    {
        private readonly ILogger<AutoTaskScheduler> _logger;
        private readonly IConfiguration _configuration;


        /// <summary>
        /// Author:Swetha M Date:13/43/2023
        /// Purpose: Constructor
        /// </summary>
        /// <returns></returns>
        /// 
        public AutoTaskScheduler(ILogger<AutoTaskScheduler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        /// <summary>
        /// Author:Swetha M Date:13/04/2023
        /// Purpose:Execute task to auto proccess the bank transactions
        /// </summary>
        /// <returns></returns>
        /// 
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    //ProcessBank Tansactions
                    await ProcessBankTransactionsAsync();

                    //Auto Match Transactions
                    await AutoMatchTransactionAsync();

                    //get the data from  Cess API
                    var response = await CallApiAndGetResponseAsync();
                    if (response != "Failed" && response != "[]")
                    {
                        List<TransactionsCess> convertedCessTransactions = JsonConvert.DeserializeObject<List<TransactionsCess>>(response);
                        await StoreApiResponseAsync(convertedCessTransactions);
                        if (convertedCessTransactions.Count() > 0)
                        {
                            await UpdatePulledTransactionStatus();

                        }
                    }

                    //get the timing to run the task
                    var delay = GetDelayUntilNextRunTime();

                    // Wait for the delay to elapse
                    await Task.Delay(delay, stoppingToken);
                }
            }
            catch (Exception e)
            {
                Log.Information("Inside ExecuteAsync auto task method");
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
            }
        }

        private async Task ProcessBankTransactionsAsync()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("AccountingConnectionString");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.ProcessBankTransactions", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;
                        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Information("Inside ProcessBankTransactions auto task method");
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
            }

        }
        private async Task UpdatePulledTransactionStatus()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("AccountingConnectionString");
                List<int> cessTransactionIDs = new List<int>();

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT ReferenceId FROM dbo.TransactionCess Where IsAcknowledged=0", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            while (await reader.ReadAsync().ConfigureAwait(false))
                            {
                                // Access data from the current row using reader.GetXXX methods
                                int cessTransactionID = reader.GetInt32(0);

                                // Add the ID to the list
                                cessTransactionIDs.Add(cessTransactionID);
                            }
                        }
                    }
                }

                if (cessTransactionIDs.Count() > 0)
                {
                    using (var httpClient = new HttpClient())
                    {
                        var uri = _configuration.GetValue<string>("BaseURL");
                        var enduri = _configuration.GetValue<string>("CessUpdateAPI");
                        var apiVersion = _configuration.GetValue<string>("APIVersion");
                        var userId = _configuration.GetValue<string>("UserId");

                        httpClient.BaseAddress = new Uri(uri);
                        httpClient.DefaultRequestHeaders.Add("X-Api-Version", apiVersion);
                        httpClient.DefaultRequestHeaders.Add("x-user-id", userId);
                        // Convert the transaction IDs to JSON format
                        var transactionIdsJson = JsonConvert.SerializeObject(cessTransactionIDs);
                        // Create a request content with the transaction IDs
                        var content = new StringContent(transactionIdsJson, Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync(enduri, content).ConfigureAwait(false);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (var connection = new SqlConnection(connectionString))
                            {
                                connection.Open();
                                string idList = string.Join(",", cessTransactionIDs);
                                using (var command = new SqlCommand($"UPDATE dbo.TransactionCess SET IsAcknowledged = 1 WHERE ReferenceId IN ({idList})", connection))
                                {
                                    int rowsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception e)
            {
                Log.Information("Inside UpdatePulledTransactionStatus auto task method");
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
            }

        }
        private async Task AutoMatchTransactionAsync()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("AccountingConnectionString");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.AutoMatchTransaction", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;
                        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Information("Inside AutoMatchTransactionAsync auto task method");
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
            }
        }

        private TimeSpan GetDelayUntilNextRunTime()
        {
            try
            {
                var taskScheduleTimeOfDay = _configuration.GetValue<TimeSpan>("TaskScheduleTimeOfDay");
                var now = DateTimeOffset.Now;
                var nextRunTime = now.Date.Add(taskScheduleTimeOfDay);
                if (now > nextRunTime)
                {
                    nextRunTime = nextRunTime.AddDays(1);
                }
                var delay = nextRunTime - now;

                return delay;
            }

            catch (Exception e)
            {
                var taskScheduleTimeOfDay = _configuration.GetValue<TimeSpan>("TaskScheduleTimeOfDay");

                Log.Information("Inside GetDelayUntilNextRunTime auto task method");
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                return taskScheduleTimeOfDay;
            }
        }
        private async Task<string> CallApiAndGetResponseAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var uri = _configuration.GetValue<string>("BaseURL");
                    var enduri = _configuration.GetValue<string>("CessAPI");
                    var apiVersion = _configuration.GetValue<string>("APIVersion");
                    var userId = _configuration.GetValue<string>("UserId");

                    httpClient.BaseAddress = new Uri(uri);
                    httpClient.DefaultRequestHeaders.Add("X-Api-Version", apiVersion);
                    httpClient.DefaultRequestHeaders.Add("x-user-id", userId);
                    var response = await httpClient.GetAsync(enduri).ConfigureAwait(false);
                    var finalResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    return finalResponse;
                }
            }
            catch (Exception e)
            {
                Log.Information("Inside CallApiAndGetResponseAsync auto task method");
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                return "Failed";
            }
        }

        private async Task StoreApiResponseAsync(List<TransactionsCess> convertedCessTransactions)
        {
            try
            {

                string connectionString = _configuration.GetConnectionString("AccountingConnectionString");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (var transaction in convertedCessTransactions)
                    {
                        transaction.CreatedBy = "AutoFetch";
                        transaction.IsMatched = false;
                        transaction.IsPicked = false;
                        transaction.IsAcknowledged = false;
                        if (transaction.TransactionGeneratedDate.ToString("dd-MM-yyyy").Contains("01/01/0001" ))
                        {
                            transaction.TransactionGeneratedDate = transaction.TransactionDate;

                        }
                       
                        transaction.Status = "Paid";
                        var sql = "INSERT INTO TransactionsCess (ReferenceId, VendorId, VendorName, PhoneNumber,Amount, ChargeOrPayment, TransactionGeneratedDate, TransactionDate, ReferenceNumber, ReferenceType, SystemName, AccountNumber, BankName, UTRNumber, Cheque_No, IFSCCode, CreatedBy, CreatedDate, Status, IsPicked, IsMatched, TransactionDetailedType, PaymentMode) " +
                        "VALUES (@ReferenceId, @VendorId, @VendorName, @PhoneNumber, @Amount, @ChargeOrPayment, @TransactionGeneratedDate, @TransactionDate, @ReferenceNumber, @ReferenceType, @SystemName, @AccountNumber, @BankName, @UTRNumber, @Cheque_No, @IFSCCode, @CreatedBy, @CreatedDate, @Status, @IsPicked, @IsMatched, @TransactionDetailedType, @PaymentMode)";

                        await connection.ExecuteAsync(sql, transaction);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Information("Inside StoreApiResponseAsync auto task method");
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
            }
        }

    }
}
