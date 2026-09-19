using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CodingTracker.Configuration
{
    public static class ConfigurationLoader
    {
        public static AppSettings Load()
        {
            string json = File.ReadAllText("appsettings.json");

            return JsonSerializer.Deserialize<AppSettings>(json) ?? throw new InvalidOperationException(
                "Unable to load appsettings.json");
        }
    }
}
