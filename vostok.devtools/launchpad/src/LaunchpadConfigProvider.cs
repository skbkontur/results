using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace launchpad
{
    internal class LaunchpadConfigProvider
    {
        private const string DefaultConfigFileName = "launchpad-config.json";

        private const string EnvironmentVariableName = "LAUNCHPAD_CONFIG_PATH";

        public LaunchpadConfig GetConfig()
        {
            return JsonConvert.DeserializeObject<LaunchpadConfig>(ObtainConfigContent());
        }

        public void SetupConfigSource(string source)
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableName, source, EnvironmentVariableTarget.User);
        }

        public void ResetToDefaultConfig()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableName, " ", EnvironmentVariableTarget.User);
        }

        private static string ObtainConfigContent()
        {
            var sourceFromEnvironment = Environment.GetEnvironmentVariable(EnvironmentVariableName);

            if (!string.IsNullOrWhiteSpace(sourceFromEnvironment))
            {
                if (Uri.TryCreate(sourceFromEnvironment, UriKind.Absolute, out var url) && url.Scheme.StartsWith("http"))
                {
                    return new HttpClient().GetStringAsync(url).GetAwaiter().GetResult();
                }

                if (File.Exists(sourceFromEnvironment))
                {
                    return File.ReadAllText(sourceFromEnvironment);
                }
            }

            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultConfigFileName));
        }
    }
}
