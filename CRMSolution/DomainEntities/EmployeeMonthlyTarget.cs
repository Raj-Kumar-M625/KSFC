namespace DomainEntities
{
    public class EmployeeMonthlyTarget
    {
        public long Id { get; set; }
        public string EmployeeCode { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public decimal MonthlyTarget { get; set; }
    }
}