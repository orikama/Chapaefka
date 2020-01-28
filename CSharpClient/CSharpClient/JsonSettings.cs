using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace CSharpClient.AppSettings
{
    public class SettingsLoader
    {
        private const string SettingsPath = "settings.json";
        private Settings _settings;
        private bool _canLoadSettings = false;

        private JsonSerializer _jsonSerializer = new JsonSerializer();

        public bool CanLoadSettings => _canLoadSettings;
        public Settings Settings => _settings;

        public SettingsLoader()
        {
            _canLoadSettings = File.Exists(SettingsPath);
        }

        public void LoadSettings()
        {
            using (StreamReader file = new StreamReader(SettingsPath))
            {
                _settings = (Settings)_jsonSerializer.Deserialize(file, typeof(Settings));
            }
        }

        public void SaveSettings()
        {
            using (StreamWriter file = new StreamWriter(SettingsPath, false, Encoding.UTF8))
            {
                _jsonSerializer.Serialize(file, _settings);
            }
        }
    }

    public class Settings
    {
        public string NameOrIP { get; set; }
        public ushort Port { get; set; }
        public bool HideIPandPort { get; set; }
        public string Token { get; set; }
        public decimal MinimumDonation { get; set; }
        public bool UseDenoiser { get; set; }
        public string DenoiserStrength { get; set; }
        public string Sigma { get; set; }
    }
}
