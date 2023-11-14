using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KAR.KSFC.UI.Helpers
{
    public class GenerateReceiptRefNo
    {
        private string chars = "00000";

        public string Alpha { get; protected set; }

        public int NumericLenght { get; protected set; }

        public string KeyFront { get; protected set; }
        public int KeyEnd { get; protected set; }

        public GenerateReceiptRefNo(int maxnum, string accountnum,string transType, int numericLength)
        {
            if (accountnum == null)
            {
                // slip accnum to three digit
                KeyFront = "NC";
                KeyEnd = 0;
                NumericLenght = numericLength;
            }
            else
            {
                
                string TransType = Regex.Replace(transType, @"\d", "");
                string Accountnum = Regex.Replace(accountnum, @"\D", "");
                int Maxnum = maxnum;
                if(TransType != null)
                {
                    Alpha = TransType;
                }
                else
                {
                    Alpha = "NA";
                }
                KeyFront = Accountnum;
                KeyEnd = Maxnum;
                NumericLenght = numericLength;
            }
        }

        public void Increment()
        {
            KeyEnd++;
        }

        public override string ToString()
        {
            var result = string.Format("{0}{1}{2}", KeyFront, Alpha, KeyEnd.ToString().PadLeft(NumericLenght, '0'));
            return result;
        }
    
}
}
