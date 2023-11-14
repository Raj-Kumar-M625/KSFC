using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteBase
    {
        public IEnumerable<SqliteImage> Images { get; set; }  // deprecated
        public IEnumerable<string> ImageFileNames { get; set; }
    }
}