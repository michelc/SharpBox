using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace SharpBox.Models
{
    public class ConnexionData
    {
        [Required]
        public string ApiKey { get; set; }
        [Required]
        public string ApiSecret { get; set; }

        public string UserToken { get; set; }
        public string UserSecret { get; set; }
    }

    public static class Configuration
    {
        public static ConnexionData ConnexionData { get; set; }

        private static string ConfigurationFile = "~/App_Data/SharpBoxConfiguration.txt";

        public static bool Load()
        {
            try
            {
                var filename = HttpContext.Current.Server.MapPath(ConfigurationFile);
                string[] lines = File.ReadAllLines(filename);

                if (lines.Length == 4)
                {
                    ConnexionData = new ConnexionData();
                    ConnexionData.ApiKey = lines[0];
                    ConnexionData.ApiSecret = lines[1];
                    ConnexionData.UserToken = lines[2];
                    ConnexionData.UserSecret = lines[3];

                    return true;
                }
            }
            catch { }

            return false;
        }

        public static void Save()
        {
            var lines = new string[] {
                ConnexionData.ApiKey,
                ConnexionData.ApiSecret,
                ConnexionData.UserToken,
                ConnexionData.UserSecret
            };

            var filename = HttpContext.Current.Server.MapPath(ConfigurationFile);
            File.WriteAllLines(filename, lines);
        }
    }
}