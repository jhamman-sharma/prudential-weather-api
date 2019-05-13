using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace prudential_weather_api.common
{
    public static class FileStorage
    {
        public static void StoreJSONFile(JObject file,string fileName)
        {
            var path = String.Format(@"{0}Output\{1}.json", AppDomain.CurrentDomain.BaseDirectory, fileName);
            File.WriteAllText(path, file.ToString());
        }
    }
}
