using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ModelFilter
    {
        public IEnumerable<CodeTableEx> Zones { get; set; }
        public IEnumerable<CodeTableEx> Areas { get; set; }
        public IEnumerable<CodeTableEx> Territories { get; set; }
        public IEnumerable<CodeTableEx> HeadQuarters { get; set; }
        public IEnumerable<CodeTableEx> StatusOptions { get; set; }
        public IEnumerable<CodeTableEx> ActivityType { get; set; }
        public IEnumerable<CodeTableEx> ClientType { get; set; }
        //public IEnumerable<CodeTableEx> ReportTypes { get; set; }
        public IEnumerable<CodeTableEx> Departments { get; set; }
        public IEnumerable<CodeTableEx> Designations { get; set; }
        public IEnumerable<CodeTableEx> WorkFlowPhases { get; set; }

        public IEnumerable<CodeTableEx> AgreementStatus { get; set; }
        public IEnumerable<CodeTable> ActiveCrops { get; set; }

        // Author:Pankaj Kumar; Purpose: Day Planning Report filters; Date: 30/04/2021
        public IEnumerable<CodeTableEx> DayPlanTypes { get; set; }
        public IEnumerable<CodeTableEx> TargetStatuses { get; set; }

        //Author: SA; Purpose : Transporter Payment Screen filters; May 17 2021
        public IEnumerable<CodeTable> STRPaymentStatus { get; set; }
        public IEnumerable<CodeTableEx> TaskStatus { get; set; }
        public IEnumerable<CodeTableEx> BankDetailStatus { get; set; }
        public string EmployeeCode { get; set; }
        public string GeolocationType { get; set; }
        public string EmployeeStatus { get; set; }
        public string EntityName { get; set; }
        public string EntityNumber { get; set; }
        public IEnumerable<CodeTableEx> GeoTagStatus { get; set; }
        public string ProfileStatus { get; set; }

    }
}