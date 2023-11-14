using Common.InputSearchCriteria;
using Common.OutputSearchCriteria;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public static class HelperSearchCriteria
    {
        public static bool IsValidInput(string criteria)
        {
            if (String.IsNullOrEmpty(criteria) || criteria.Length == 0 || criteria.Equals("0") || criteria.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

        public static Tuple<bool, DateTime, DateTime> ParseAndValidateDates(string DateFrom, string DateTo)
        {
            if (String.IsNullOrEmpty(DateFrom))
            {
                DateFrom = "01-01-0001";
            }
            if (String.IsNullOrEmpty(DateTo))
            {
                DateTo = "01-01-0001";
            }

            var culture = CultureInfo.CreateSpecificCulture("en-GB");

            bool applyDateFilter = false;
            DateTime fromDate;
            DateTime toDate;
            bool isValidFromDate = DateTime.TryParse(DateFrom, culture, DateTimeStyles.None, out fromDate);
            bool isValidToDate = DateTime.TryParse(DateTo, culture, DateTimeStyles.None, out toDate);

            if (isValidFromDate && isValidToDate)
            {
                if (fromDate > DateTime.MinValue && toDate == DateTime.MinValue)
                {
                    //s.DateTo = DateTime.MaxValue;
                    toDate = DateTime.UtcNow.AddDays(1);
                }

                if ((fromDate > DateTime.MinValue && toDate > DateTime.MinValue && toDate >= fromDate) ||
                    (fromDate == DateTime.MinValue && toDate > DateTime.MinValue))
                {
                    applyDateFilter = true;
                }
            }

            if (applyDateFilter)
            {
                if (fromDate == DateTime.MinValue)
                {
                    fromDate = new DateTime(2020, 05, 01);
                }

                if (fromDate == DateTime.MaxValue)
                {
                    fromDate = DateTime.UtcNow.AddDays(1);
                }
            }

            return new Tuple<bool, DateTime, DateTime>(applyDateFilter, fromDate, toDate);
        }

        private static Tuple<bool, Decimal, Decimal> ParseAndValidateAmount(decimal MinAmount, decimal MaxAmount)
        {

            bool applyAmountFilter = false;

            if (MinAmount > 0 && MaxAmount == 0)
            {
                MaxAmount = Decimal.MaxValue;
            }

            if ((MinAmount > 0 && MaxAmount > 0 && MaxAmount >= MinAmount) ||
                (MinAmount == 0 && MaxAmount > 0))
            {
                applyAmountFilter = true;
            }

            return new Tuple<bool, Decimal, Decimal>(applyAmountFilter, MinAmount, MaxAmount);
        }

        public static GenericOutputSearchCriteria GetDefaultBankStatementsFilter()
        {
            return new GenericOutputSearchCriteria()
            {
                ApplyTransactionDateFilter = false,
                ApplyTransactionTypeFilter = false,
                ApplyAmountFilter = false,
                ApplyBankNameFilter = false,
                ApplyAccountNumberFilter = false,
                ApplyFileNameFilter = false
            };
        }

        public static GenericOutputSearchCriteria ParseBankStatementsSearchCriteria(GenericInputSearchCriteria searchCriteria)
        {
            GenericOutputSearchCriteria s = GetDefaultBankStatementsFilter();

            if (searchCriteria == null)
            {
                return s;
            }

            var r = ParseAndValidateDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyTransactionDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3;

            var a = ParseAndValidateAmount(searchCriteria.MinAmount, searchCriteria.MaxAmount);
            s.ApplyAmountFilter = a.Item1;
            s.MinAmount = a.Item2;
            s.MaxAmount = a.Item3;

            s.ApplyTransactionTypeFilter = IsValidInput(searchCriteria.TransactionType);
            s.TransactionType = searchCriteria.TransactionType;

            s.ApplyBankNameFilter = IsValidInput(searchCriteria.BankName);
            s.BankName = searchCriteria.BankName;

            s.ApplyAccountNumberFilter = IsValidInput(searchCriteria.AccountNo.ToString());
            s.AccountNo = searchCriteria.AccountNo;

            s.ApplyFileNameFilter = IsValidInput(searchCriteria.FileName);
            s.FileName = searchCriteria.FileName;

            s.Count = searchCriteria.Count == 0 ? s.Count : searchCriteria.Count;

            return s;
        }
    }
}
