using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ErrorObject
    {
        public string CellReference { get; set; }
        public string ExpectedValue { get; set; }
        public string ActualValue { get; set; }
        public string MessageType { get; set; }
        public string Description { get; set; }
    }
}