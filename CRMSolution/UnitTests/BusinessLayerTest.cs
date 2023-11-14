using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRMUtilities;
using DomainEntities;
using System.Collections.Generic;
using BusinessLayer;
using System.Diagnostics;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class BusinessLayerTest
    {
        [TestMethod]
        public void TestAccumulateEORPData_01()
        {
            // Arrange
            DateTime startDate = new DateTime(2016, 12, 19);
            DateTime endDate = new DateTime(2017, 10, 28);

            List<EORPData> expenseData = new List<EORPData>();
            List<EORPData> orderData = new List<EORPData>();
            List<EORPData> returnOrderData = new List<EORPData>();
            List<EORPData> paymentData = new List<EORPData>();

            // Act
            EORP eorp = Business.AccumulateEORPData(startDate, endDate, expenseData, orderData, returnOrderData, paymentData);

            // Assert
            Assert.IsNotNull(eorp);
        }

        [TestMethod]
        public void TestAccumulateEORPData_02()
        {
            // Arrange
            DateTime startDate = new DateTime(2016, 12, 19);
            DateTime endDate = new DateTime(2017, 03, 28);

            List<EORPData> expenseData = new List<EORPData>();
            List<EORPData> orderData = new List<EORPData>();
            List<EORPData> returnOrderData = new List<EORPData>();
            List<EORPData> paymentData = new List<EORPData>();

            FillData(startDate, endDate, expenseData);
            FillData(startDate.AddDays(1), endDate, orderData);
            FillData(startDate.AddDays(2), endDate, returnOrderData);
            FillData(startDate.AddDays(3), endDate, paymentData);

            // Act
            EORP eorp = Business.AccumulateEORPData(startDate, endDate, expenseData, orderData, returnOrderData, paymentData);

            // Assert
            Assert.IsNotNull(eorp);
            Assert.AreEqual<DateTime>(startDate, eorp.StartDate);
            Assert.AreEqual<DateTime>(endDate, eorp.EndDate);
            Assert.AreEqual<int>(100, eorp.DayCount);

            Assert.IsNotNull(eorp.EORPSummary);
            Assert.IsNotNull(eorp.EORPMonthlySummary);
            Assert.IsNotNull(eorp.EORPDays);

            // check numbers in overall summary
            Assert.AreEqual<decimal>(50905, eorp.EORPSummary.ExpenseTotal, "Expense Total in Summary mismatch");
            Assert.AreEqual<decimal>(50889, eorp.EORPSummary.OrderTotal, "Order Total in Summary mismatch");
            Assert.AreEqual<decimal>(50914, eorp.EORPSummary.ReturnOrderTotal, "Return Total in Summary mismatch");
            Assert.AreEqual<decimal>(48891, eorp.EORPSummary.PaymentTotal, "Payment Total in Summary mismatch");

            // check daily expense numbers
            foreach(var d in expenseData)
            {
                EORPDay dayRecord = eorp.EORPDays.Where(x => x.Date == d.Date).FirstOrDefault();
                Assert.IsNotNull(dayRecord);
                Assert.AreEqual<decimal>(d.TotalAmountForDay, dayRecord.ExpenseAmount);
                Assert.AreEqual<decimal>(0, dayRecord.OrderAmount);
                Assert.AreEqual<decimal>(0, dayRecord.ReturnOrderAmount);
                Assert.AreEqual<decimal>(0, dayRecord.PaymentAmount);

                Assert.AreEqual<int>(d.TotalItemCountForDay, dayRecord.ExpenseCount);
                Assert.AreEqual<int>(0, dayRecord.OrderCount);
                Assert.AreEqual<int>(0, dayRecord.ReturnOrderCount);
                Assert.AreEqual<int>(0, dayRecord.PaymentCount);
            }

            // check daily order numbers
            foreach (var d in orderData)
            {
                EORPDay dayRecord = eorp.EORPDays.Where(x => x.Date == d.Date).FirstOrDefault();
                Assert.IsNotNull(dayRecord);
                Assert.AreEqual<decimal>(0, dayRecord.ExpenseAmount);
                Assert.AreEqual<decimal>(d.TotalAmountForDay, dayRecord.OrderAmount);
                Assert.AreEqual<decimal>(0, dayRecord.ReturnOrderAmount);
                Assert.AreEqual<decimal>(0, dayRecord.PaymentAmount);

                Assert.AreEqual<int>(0, dayRecord.ExpenseCount);
                Assert.AreEqual<int>(d.TotalItemCountForDay, dayRecord.OrderCount);
                Assert.AreEqual<int>(0, dayRecord.ReturnOrderCount);
                Assert.AreEqual<int>(0, dayRecord.PaymentCount);
            }

            // check daily return numbers
            foreach (var d in returnOrderData)
            {
                EORPDay dayRecord = eorp.EORPDays.Where(x => x.Date == d.Date).FirstOrDefault();
                Assert.IsNotNull(dayRecord);
                Assert.AreEqual<decimal>(0, dayRecord.ExpenseAmount);
                Assert.AreEqual<decimal>(0, dayRecord.OrderAmount);
                Assert.AreEqual<decimal>(d.TotalAmountForDay, dayRecord.ReturnOrderAmount);
                Assert.AreEqual<decimal>(0, dayRecord.PaymentAmount);

                Assert.AreEqual<int>(0, dayRecord.ExpenseCount);
                Assert.AreEqual<int>(0, dayRecord.OrderCount);
                Assert.AreEqual<int>(d.TotalItemCountForDay, dayRecord.ReturnOrderCount);
                Assert.AreEqual<int>(0, dayRecord.PaymentCount);
            }

            // check daily payment numbers
            foreach (var d in paymentData)
            {
                EORPDay dayRecord = eorp.EORPDays.Where(x => x.Date == d.Date).FirstOrDefault();
                Assert.IsNotNull(dayRecord);
                Assert.AreEqual<decimal>(0, dayRecord.ExpenseAmount);
                Assert.AreEqual<decimal>(0, dayRecord.OrderAmount);
                Assert.AreEqual<decimal>(0, dayRecord.ReturnOrderAmount);
                Assert.AreEqual<decimal>(d.TotalAmountForDay, dayRecord.PaymentAmount);

                Assert.AreEqual<int>(0, dayRecord.ExpenseCount);
                Assert.AreEqual<int>(0, dayRecord.OrderCount);
                Assert.AreEqual<int>(0, dayRecord.ReturnOrderCount);
                Assert.AreEqual<int>(d.TotalItemCountForDay, dayRecord.PaymentCount);
            }

            // Test monthly summary numbers
            EORPMonth sumData = null;
            Assert.AreEqual<int>(4, eorp.EORPMonthlySummary.Count(), "Monthly summary count mismatch");

            // assure that the items are ordered by month
            int i = 0;
            foreach( var d in eorp.EORPMonthlySummary)
            {
                DateTime dt;
                switch(i)
                {
                    case 0: dt = new DateTime(2016, 12, 1); break;
                    case 1: dt = new DateTime(2017, 1, 1); break;
                    case 2: dt = new DateTime(2017, 2, 1); break;
                    case 3: dt = new DateTime(2017, 3, 1); break;
                    default: dt = DateTime.MinValue; break;
                }
                Assert.AreEqual<DateTime>(dt, d.Date, "Dates in monthly summary are not sorted");
                i++;
            }

            sumData = eorp.EORPMonthlySummary.Where(x => x.Date == new DateTime(2016, 12, 1)).FirstOrDefault();
            Assert.IsNotNull(sumData);
            Assert.AreEqual<decimal>(8212, sumData.ExpenseAmount);
            Assert.AreEqual<decimal>(6156, sumData.OrderAmount);
            Assert.AreEqual<decimal>(6159, sumData.ReturnOrderAmount);
            Assert.AreEqual<decimal>(6162, sumData.PaymentAmount);


            sumData = eorp.EORPMonthlySummary.Where(x => x.Date == new DateTime(2017, 1, 1)).FirstOrDefault();
            Assert.IsNotNull(sumData);
            Assert.AreEqual<decimal>(14238, sumData.ExpenseAmount);
            Assert.AreEqual<decimal>(16264, sumData.OrderAmount);
            Assert.AreEqual<decimal>(16272, sumData.ReturnOrderAmount);
            Assert.AreEqual<decimal>(16280, sumData.PaymentAmount);


            sumData = eorp.EORPMonthlySummary.Where(x => x.Date == new DateTime(2017, 2, 1)).FirstOrDefault();
            Assert.IsNotNull(sumData);
            Assert.AreEqual<decimal>(14224, sumData.ExpenseAmount);
            Assert.AreEqual<decimal>(14231, sumData.OrderAmount);
            Assert.AreEqual<decimal>(14238, sumData.ReturnOrderAmount);
            Assert.AreEqual<decimal>(14245, sumData.PaymentAmount);


            sumData = eorp.EORPMonthlySummary.Where(x => x.Date == new DateTime(2017, 3, 1)).FirstOrDefault();
            Assert.IsNotNull(sumData);
            Assert.AreEqual<decimal>(14231, sumData.ExpenseAmount);
            Assert.AreEqual<decimal>(14238, sumData.OrderAmount);
            Assert.AreEqual<decimal>(14245, sumData.ReturnOrderAmount);
            Assert.AreEqual<decimal>(12204, sumData.PaymentAmount);
        }

        [TestMethod]
        public void TestGetNextSMSTime_01()
        {
            // Arrange
            DateTime d1 = new DateTime(2017, 11, 09, 9, 00, 00);
            DateTime d2 = new DateTime(2017, 11, 09, 11, 00, 00);
            DateTime d3 = new DateTime(2017, 11, 09, 13, 00, 00);
            DateTime d4 = new DateTime(2017, 11, 09, 15, 00, 00);

            List<DateTime> todaySchedules = new List<DateTime>()
            {
                d1, d4, d3, d2
            };

            DateTime lastSmsTime = DateTime.MinValue;
            DateTime expected = d1;

            // Act
            DateTime result = Business.GetNextSMSTime(todaySchedules, lastSmsTime);

            // Assert
            Assert.AreEqual<DateTime>(expected, result);
        }

        [TestMethod]
        public void TestGetNextSMSTime_02()
        {
            // Arrange
            DateTime d1 = new DateTime(2017, 11, 09, 9, 00, 00);
            DateTime d2 = new DateTime(2017, 11, 09, 11, 00, 00);
            DateTime d3 = new DateTime(2017, 11, 09, 13, 00, 00);
            DateTime d4 = new DateTime(2017, 11, 09, 15, 00, 00);

            List<DateTime> todaySchedules = new List<DateTime>()
            {
                d1, d4, d3, d2
            };

            DateTime lastSmsTime = new DateTime(2017, 11, 09, 08, 30, 00);
            DateTime expected = d1;

            // Act
            DateTime result = Business.GetNextSMSTime(todaySchedules, lastSmsTime);

            // Assert
            Assert.AreEqual<DateTime>(expected, result);
        }

        [TestMethod]
        public void TestGetNextSMSTime_03()
        {
            // Arrange
            DateTime d1 = new DateTime(2017, 11, 09, 9, 00, 00);
            DateTime d2 = new DateTime(2017, 11, 09, 11, 00, 00);
            DateTime d3 = new DateTime(2017, 11, 09, 13, 00, 00);
            DateTime d4 = new DateTime(2017, 11, 09, 15, 00, 00);

            List<DateTime> todaySchedules = new List<DateTime>()
            {
                d1, d4, d3, d2
            };

            DateTime lastSmsTime = new DateTime(2017, 11, 09, 09, 05, 00);
            DateTime expected = d2;

            // Act
            DateTime result = Business.GetNextSMSTime(todaySchedules, lastSmsTime);

            // Assert
            Assert.AreEqual<DateTime>(expected, result);
        }

        [TestMethod]
        public void TestGetNextSMSTime_04()
        {
            // Arrange
            DateTime d1 = new DateTime(2017, 11, 09, 9, 00, 00);
            DateTime d2 = new DateTime(2017, 11, 09, 11, 00, 00);
            DateTime d3 = new DateTime(2017, 11, 09, 13, 00, 00);
            DateTime d4 = new DateTime(2017, 11, 09, 15, 00, 00);

            List<DateTime> todaySchedules = new List<DateTime>()
            {
                d1, d4, d3, d2
            };
            // putting the items in an unordered manner

            DateTime lastSmsTime = new DateTime(2017, 11, 09, 15, 05, 00);
            DateTime expected = DateTime.MinValue;

            // Act
            DateTime result = Business.GetNextSMSTime(todaySchedules, lastSmsTime);

            // Assert
            Assert.AreEqual<DateTime>(expected, result);
        }

        [TestMethod]
        public void TestGetNextSMSTime_05()
        {
            // Arrange
            DateTime d1 = new DateTime(2017, 11, 09, 9, 00, 00);
            DateTime d2 = new DateTime(2017, 11, 09, 11, 00, 00);
            DateTime d3 = new DateTime(2017, 11, 09, 13, 00, 00);
            DateTime d4 = new DateTime(2017, 11, 09, 15, 00, 00);

            List<DateTime> todaySchedules = new List<DateTime>()
            {
                d1, d4, d3, d2
            };
            // putting the items in an unordered manner

            DateTime lastSmsTime = new DateTime(2017, 11, 09, 11, 00, 00);
            DateTime expected = d3;

            // Act
            DateTime result = Business.GetNextSMSTime(todaySchedules, lastSmsTime);

            // Assert
            Assert.AreEqual<DateTime>(expected, result);
        }

        [TestMethod]
        public void TestIsWorkingDay_01()
        {
            // Arrange
            List<TenantWeekDay> weekDays = new List<TenantWeekDay>()
            {
                new TenantWeekDay() { WeekDayName = "sUNDAY", IsWorkingDay = false },
                new TenantWeekDay() { WeekDayName = "monday", IsWorkingDay = true },
                new TenantWeekDay() { WeekDayName = "tuesday", IsWorkingDay = true },
                new TenantWeekDay() { WeekDayName = "wednesday", IsWorkingDay = true },
                new TenantWeekDay() { WeekDayName = "Thursday", IsWorkingDay = true },
                new TenantWeekDay() { WeekDayName = "fRiDaY", IsWorkingDay = true }
            };
            string todayDay = new DateTime(2017, 11, 09).DayOfWeek.ToString(); // Thursday
            bool expected = true;

            // Act
            bool result = Business.IsWorkingDay(weekDays, todayDay);

            // Assert
            Assert.AreEqual<bool>(expected, result);
        }

        [TestMethod]
        public void TestIsWorkingDay_02()
        {
            // Arrange
            List<TenantWeekDay> weekDays = new List<TenantWeekDay>()
            {
                new TenantWeekDay() { WeekDayName = "sUNDAY", IsWorkingDay = false },
                new TenantWeekDay() { WeekDayName = "monday", IsWorkingDay = true },
                new TenantWeekDay() { WeekDayName = "tuesday", IsWorkingDay = true },
                new TenantWeekDay() { WeekDayName = "wednesday", IsWorkingDay = true },
                //new TenantWeekDay() { WeekDayName = "Thursday", IsWorkingDay = true },
                new TenantWeekDay() { WeekDayName = "fRiDaY", IsWorkingDay = true }
            };
            string todayDay = new DateTime(2017, 11, 09).DayOfWeek.ToString(); // Thursday
            bool expected = false;

            //Thursday does not exist at all in weekDays

            // Act
            bool result = Business.IsWorkingDay(weekDays, todayDay);

            // Assert
            Assert.AreEqual<bool>(expected, result);
        }

        [TestMethod]
        public void TestIsWorkingDay_03()
        {
            // Arrange
            List<TenantWeekDay> weekDays = new List<TenantWeekDay>()
            {
                new TenantWeekDay() { WeekDayName = "sUNDAY", IsWorkingDay = false },
                new TenantWeekDay() { WeekDayName = "monday", IsWorkingDay = true },
                new TenantWeekDay() { WeekDayName = "tuesday", IsWorkingDay = true },
                new TenantWeekDay() { WeekDayName = "wednesday", IsWorkingDay = true },
                //new TenantWeekDay() { WeekDayName = "Thursday", IsWorkingDay = true },
                new TenantWeekDay() { WeekDayName = "fRiDaY", IsWorkingDay = true }
            };

            string todayDay = new DateTime(2017, 11, 12).DayOfWeek.ToString(); // Sunday
            bool expected = false;

            // Act
            bool result = Business.IsWorkingDay(weekDays, todayDay);

            // Assert
            Assert.AreEqual<bool>(expected, result);
        }

        private void FillData(DateTime startDate, DateTime endDate, List<EORPData> dataList, int daysInterval = 4)
        {
            while(startDate < endDate)
            {
                EORPData data = new EORPData()
                {
                    Date = startDate,
                    TotalAmountForDay = startDate.Year + startDate.Month + startDate.Day,
                    TotalItemCountForDay = startDate.Day
                };

                dataList.Add(data);
                startDate = startDate.AddDays(4);
            }

            foreach (var d in dataList)
            {
                Debug.WriteLine($"{d.Date.ToString("yyyy-MM-dd")}\t{d.TotalAmountForDay}\t{d.TotalItemCountForDay}");
            }
            Debug.WriteLine("------------------");
        }
    }
}
