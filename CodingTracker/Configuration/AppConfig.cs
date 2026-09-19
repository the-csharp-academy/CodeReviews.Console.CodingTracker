using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Configuration
{
    public static class AppConfig
    {
        public static AppSettings Settings { get; private set; } = null!;

        public static void Initialize(AppSettings settings)
        {
            Settings = settings;
        }
    }
}
