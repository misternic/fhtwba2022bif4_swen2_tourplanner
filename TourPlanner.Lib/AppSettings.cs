using System;
using Microsoft.Extensions.Configuration;

namespace TourPlanner.Lib
{
    public sealed class AppSettings
    {
        private static AppSettings _instance;
        public IConfigurationRoot Configuration { get; }

        private AppSettings()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }
        
        public static AppSettings GetInstance()
        {
            return _instance ??= new AppSettings();
        }
    }
}