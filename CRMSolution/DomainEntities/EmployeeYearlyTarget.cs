namespace DomainEntities
{
    public class EmployeeYearlyTarget
    {
        public long Id { get; set; }
        public string EmployeeCode { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public decimal YearlyTarget { get; set; }
    }
}