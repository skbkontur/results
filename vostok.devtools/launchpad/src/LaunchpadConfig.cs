using Newtonsoft.Json;

namespace launchpad
{
    internal class LaunchpadConfig
    {
        [JsonProperty("Definitions")]
        public TemplateDefinition[] Definitions { get; set; }

        [JsonProperty("NugetSources")]
        public string[] NugetSources { get; set; }
    }
}
