using System;
using Microsoft.Extensions.Configuration;

namespace FeatureLibrary
{
    public sealed class AppSettings
    {
        private static AppSettings _instance;
        public IConfigurationRoot Root { get; }

        private AppSettings()
        {
            Root = new ConfigurationBuilder()
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