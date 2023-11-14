namespace Presentation.GridFilters.TDS
{
    public class TdsQuarterlyListFilter
    {
        public string[] forder { get; set; }
        public int noOfChallan { get; set; }
        public decimal payableMinAmount { get; set; }
        public decimal payableMaxAmount { get; set; }
        public string quarter { get; set; }

        public int assessmentYear { get; set; }
    }
}
