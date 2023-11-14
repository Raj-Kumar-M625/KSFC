using System;
using System.Text.RegularExpressions;

namespace KAR.KSFC.Components.Common.Utilities.Validations
{
    public static class Validation
    {
        public static bool Mobile(string mobileNum)
        {
            var match = Regex.Match(mobileNum, CustomRegex.mobileReg, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return false;
            }

            return true;
        }
        public static bool Pan(string panNum)
        {
            var match = Regex.Match(panNum, CustomRegex.panReg, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return false;
            }

            return true;
        }      
    }
}
