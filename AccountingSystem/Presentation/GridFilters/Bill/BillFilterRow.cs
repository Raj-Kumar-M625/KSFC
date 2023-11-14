using Omu.AwesomeMvc;

namespace Presentation.Extensions.Bill
{
    public class BillFilterRow
    {
        public KeyContent[] Category { get; set; }
        public KeyContent[] CreatedBy { get; set; }
        public KeyContent[] Status { get; set; }

    }
}
