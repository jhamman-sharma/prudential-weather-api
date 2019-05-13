using System.Configuration;
using System.IO;
using Newtonsoft.Json.Linq;

namespace prudential_weather_api.common
{
    /// <summary>
    /// Class contains storage methods
    /// </summary>
    public static class FileStorage
    {
        /// <summary>
        /// Method used to store JSON files on specified system path
        /// </summary>
        /// <param name="content">JSON Object to store in JSON text format</param>
        /// <param name="fileName">JSON FIle name</param>
        public static void StoreJSONFile(JObject content,string fileName)
        {
            var path = ConfigurationManager.AppSettings["LogFilePath"];     
            Directory.CreateDirectory(path);
            File.WriteAllText($"{path}\\{fileName}.json", content.ToString());
        }
    }
}
