﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ActivityContactData
    {
        public long Id { get; set; }
        public long ActivityId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPrimary { get; set; }
    }
}