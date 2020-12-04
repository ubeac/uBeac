using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace uBeac.Configuration
{
    public static class ConfigurationServiceExtensions
    {
        public static IConfigurationBuilder AddJsonConfig(this IConfigurationBuilder configBuilder, IHostEnvironment env)
        {
            // setting base path for config folder
            var rootPath = env.ContentRootPath + "\\Config\\";
            configBuilder.SetBasePath(rootPath);

            foreach (var filename in Directory.GetFiles(rootPath))
            {
                // read only json files
                if (Path.GetExtension(filename) != ".json")
                    continue;

                // accept only root config files
                if (Path.GetFileName(filename).Split(".").Length != 2)
                    continue;

                // adding config file
                configBuilder.AddJsonFile(filename, optional: true, reloadOnChange: true)
                  .AddJsonFile($"{Path.GetFileNameWithoutExtension(filename)}.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }

            return configBuilder;
        }
    }
}
