using System.Linq;

namespace AweCoreDemo.ViewModels.Input.Settings
{
    public class SettingsVal
    {
        public string Theme { get; set; }

        public string ThemeBodyClass
        {
            get
            {
                var res = Theme;
                
                if (new[] {"bts", "black-cab", "met", "val", "start", "gtx"}.Contains(Theme))
                {
                    res += " dark";
                }

                if (new[] {"black-cab", "gtx"}.Contains(Theme))
                {
                    res += " vdark";
                }

                return res;
            }
        }
    }
}