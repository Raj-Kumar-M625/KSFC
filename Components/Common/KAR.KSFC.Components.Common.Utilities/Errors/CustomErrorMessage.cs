using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Utilities.Errors
{
    public class CustomErrorMessage
    {
        public const string E01 = "Mobile Number with PAN already available. Please login using PAN.";
        public const string E02 = "Mobile Number already available. Please update PAN by approaching the KSFC head office."; //discussed with ajay in the unit mobile number field not available to verify and take branch so contact KSFC head office.
        public const string E03 = "PAN already available in Branch xxxxx. Please update Mobile Number by approaching the Branch or nearby KSFC Branch.";
        public const string E04 = "PAN already available with different Mobile No.{0}. Request you to please login using your registered PAN. If you intend to update Mobile Number, please approach nearby KSFC Branch";
        public const string E10 = "PAN already available with different Mobile No.{0}. Please login as registered user using PAN. If you intend to update Mobile Number, please approach KSFC head office";
        public const string E05 = "PAN Inactive in Income Tax Portal. Please enter Active PAN.";
        public const string E06 = "Bad reuest.";
        public const string E07 = "Invalid PAN or PAN not registered. Please register PAN";
        public const string E08 = "Please update Mobile No.in Branch xxxxx or any nearby KSFC Branch.";
        public const string E09 = "OTP already been sent to same mobile number. Valid only for 3 min.";
        public const string E11 = "OTP Verified succesfully.";
        public const string E12 = "Entered OTP is incorrect or has been expired. Please note that the OTP is valid for 10 minutes only.";
        public const string E13 = "Invalid Mobile Number.";

    }
}
