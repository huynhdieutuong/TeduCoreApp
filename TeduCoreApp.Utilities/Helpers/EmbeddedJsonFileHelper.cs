using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace TeduCoreApp.Utilities.Helpers
{
    public static class EmbeddedJsonFileHelper
    {
        public static T GetContent<T>(string filename)
        {
            string fullPath = Path.GetFullPath($"{filename}.json");
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"json file \"{fullPath}\" doesn't exist");
            }
            return JsonConvert.DeserializeObject<T>(File.ReadAllText($"{filename}.json"));
        }

        public static T ConvertJson<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static JObject GetJObjectFromFile(string filename)
        {
            string fullPath = Path.GetFullPath($"{filename}.json");

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"json file \"{fullPath}\" doesn't exist");
            }

            string content = File.ReadAllText(fullPath);

            return JObject.Parse(content);
        }

        public static string ReadJsonFile(string filename)
        {
            try
            {
                string fullPath = Path.GetFullPath($"{filename}.json");

                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"json file \"{fullPath}\" doesn't exist");
                }

                return File.ReadAllText($"{filename}.json");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ConvertFileIntoBase64String(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"file \"{filename}\" doesn't exist");
            }

            byte[] bytes = File.ReadAllBytes(filename);
            return Convert.ToBase64String(bytes);
        }
    }
}
