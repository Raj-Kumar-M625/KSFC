using KAR.KSFC.UI.Models.Localization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace KAR.KSFC.UI.Languages
{
    public static class kn_IN
    {
        public static List<Resource> GetList()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            return JsonConvert.DeserializeObject<List<Resource>>(File.ReadAllText("Languages/kn-IN.json", Encoding.UTF8), jsonSerializerSettings);
        }
    }
}
