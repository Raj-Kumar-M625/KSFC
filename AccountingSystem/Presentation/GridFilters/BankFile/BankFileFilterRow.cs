using Omu.AwesomeMvc;

namespace Presentation.Extensions.BankFile
{
    /// <summary>
    /// Author:Swetha M Date:06/05/2022
    /// Purpose:Paymnet filter row
    /// </summary>
    /// <returns></returns>
    public class BankFileFilterRow
    {

        public KeyContent[] CreatedOn { get; set; }
        public KeyContent[] NoOfVendors { get; set; }
        public KeyContent[] TotalAmount { get; set; }
    }
}