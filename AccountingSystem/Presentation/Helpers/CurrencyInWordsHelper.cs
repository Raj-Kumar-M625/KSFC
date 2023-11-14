using System.Collections.Generic;
using System.Globalization;
using System;
using System.Text;

namespace Presentation.Helpers
{
    public class CurrencyInWordsHelper
    {
        public static string ToVerbalCurrency(decimal value)
        {
            var valueString = value.ToString("N2", CultureInfo.CreateSpecificCulture("en-IN"));
            var decimalString = valueString.Substring(valueString.LastIndexOf('.') + 1);
            var wholeString = valueString.Substring(0, valueString.LastIndexOf('.'));

            var valueArray = wholeString.Split(',');

            var unitsMap = new[] { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            var tensMap = new[] { "", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            var placeMap = new[] { "", " thousand ", " lakh ", " crore " };

            var outList = new List<string>();

            var placeIndex = 0;

            for (int i = valueArray.Length - 1; i >= 0; i--)
            {
                var intValue = Convert.ToInt32(valueArray[i]);
                var tensValue = intValue % 100;

                var tensString = string.Empty;
                if (tensValue < unitsMap.Length) tensString = unitsMap[tensValue];
                else tensString = tensMap[(tensValue - tensValue % 10) / 10] + " " + unitsMap[tensValue % 10];

                var fullValue = string.Empty;
                if (intValue >= 100) fullValue = unitsMap[(intValue - intValue % 100) / 100] + " hundred " + tensString + placeMap[placeIndex++];
                else if (intValue != 0) fullValue = tensString + placeMap[placeIndex++];
                else placeIndex++;

                outList.Add(fullValue);
            }

            var intPaiseValue = Convert.ToInt32(decimalString);

            var paiseString = string.Empty;
            if (intPaiseValue < unitsMap.Length) paiseString = unitsMap[intPaiseValue];
            else paiseString = tensMap[(intPaiseValue - intPaiseValue % 10) / 10] + " " + unitsMap[intPaiseValue % 10];

            if (intPaiseValue == 0) paiseString = "zero";

            var output = string.Empty;

            StringBuilder sb = new StringBuilder();
            for (int i = outList.Count - 1; i >= 0; i--)
            {
                sb.Append(outList[i]);
            }
            if (string.IsNullOrWhiteSpace(sb.ToString()))
            {
                return paiseString + " paise";
            }
            sb.Append(" rupees and ");
            sb.Append(paiseString);
            sb.Append(" paise");
            return sb.ToString();

            //for (int i = outList.Count - 1; i >= 0; i--) output += outList[i];
            //if (string.IsNullOrWhiteSpace(output))
            //{
            //    return paiseString + " paise";
            //}
            //output += " rupees and " + paiseString + " paise";

            return output;
        }
    }
}
