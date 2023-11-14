using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Helper.EnsureEventSourceExist();
                Process.ProcessFiles();
            }
            catch(Exception ex)
            {
                Helper.LogError(ex.Message);

                // log more details in event viewer
                Helper.LogError(ex.ToString(), false);
            }
        }
    }
}
