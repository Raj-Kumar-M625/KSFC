using Omu.AwesomeMvc;

namespace Presentation.Extensions.Payment
{
    /// <summary>
    /// Author:Swetha M Date:06/05/2022
    /// Purpose:Paymnet filter row
    /// </summary>
    /// <returns></returns>
    public class PaymentFilterRow
    {
        
        public KeyContent[] CreatedBy { get; set; }
        public KeyContent[] ApprovedBy { get; set; }
        public KeyContent[] PaymentStatus { get; set; }
    }
}