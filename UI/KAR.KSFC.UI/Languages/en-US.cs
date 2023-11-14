using KAR.KSFC.UI.Models.Localization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace KAR.KSFC.UI.Languages
{
    public static class en_US
    {
        public static List<Resource> GetList()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            return JsonConvert.DeserializeObject<List<Resource>>(File.ReadAllText("Languages/en-US.json"), jsonSerializerSettings);
        }
    }
}
