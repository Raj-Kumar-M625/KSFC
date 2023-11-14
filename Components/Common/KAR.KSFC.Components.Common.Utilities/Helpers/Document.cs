using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Utilities.Helpers
{
    public class Document
    {
        #region Modules
        public const string LegalDocumentation = "LegalDocumentation";
        public const string AuditClearance = "AuditClearance";
        public const string DisbursementCondition = "DisbursementCondition";
        public const string InspectionOfUnit = "InspectionOfUnit";
        public const string ChangeOfUnit = "ChangeOfUnit";
        #endregion
    }

    public class DocumentError
    {
        #region Modules
        public const string Encryptionfailed = "File encryption failed.";
        
        #endregion
    }

}
