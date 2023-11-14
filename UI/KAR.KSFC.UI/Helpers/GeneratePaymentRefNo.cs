using System.Text.RegularExpressions;

namespace KAR.KSFC.UI.Helpers
{
    public class GeneratePaymentRefNo
    {
       // private string chars = "00000";

        public string Alpha { get; protected set; }

        public int NumericLenght { get; protected set; }

        public string KeyFront { get; protected set; }
        public int KeyEnd { get; protected set; }

        public GeneratePaymentRefNo(int maxnum, string payType, string accountnum, int numericLength)
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

                string PaymentType = Regex.Replace(payType, @"\d", "");
                string Accountnum = Regex.Replace(accountnum, @"\D", "");
                int Maxnum = maxnum;
                if (PaymentType != null)
                {
                    KeyFront = PaymentType;
                }
                else
                {
                    KeyFront = "PYT";
                }
                Alpha = Accountnum;
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
