using System;
using System.Threading.Tasks;

namespace Presentation.Services.Infra
{
    public static class ExecUtil
    {
        public static async Task Try(Action action, int attempts = 3, int delay = 300)
        {
            var count = 0;
            try
            {
                action();
            }
            catch (Exception)
            {
                count++;
                if (count < attempts)
                {
                    await Task.Delay(delay);
                    action();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}