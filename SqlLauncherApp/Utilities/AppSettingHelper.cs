using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SqlLauncherApp.Utilities
{
    public static class AppSettingHelper
    {
        private static string _connetionString;
        public static string ConnectionString => _connetionString ??= GetBilder().GetConnectionString("MyDatabase");


        private static string _pathFolder;
        public static string PathFolder => _pathFolder ??= GetBilder().GetValue<string>("PathFolder");

        private static IConfigurationRoot GetBilder()
        {
            return new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
        }
    }
}
