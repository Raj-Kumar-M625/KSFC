using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteImage
    {
        public string Id { get; set; } // this is to be used as file name
        public byte[] BinaryData { get; set; }
    }
}