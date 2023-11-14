namespace KAR.KSFC.Components.Common.Utilities.Validations
{
    public static class CustomRegex
    {
        public const string mobileReg = @"^([6-9]{1}[0-9]{9})$";
        public const string panReg = @"^([A-Za-z]{3}[p|c|h|a|b|g|j|l|f|t|P|C|H|A|B|G|J|L|F|T]{1}[A-Za-z]{1}[0-9]{4}[A-Za-z]{1})$";
        public const string emailReg = @"^[a - z0 - 9][-a - z0 - 9._] +@([-a - z0 - 9]+.)+[a-z]{2,5}$";
    }
}
