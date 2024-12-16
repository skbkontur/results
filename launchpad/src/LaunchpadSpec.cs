using Newtonsoft.Json;

namespace launchpad
{
    internal class LaunchpadSpec
    {
        [JsonProperty("Variables")]
        public VariableDefinition[] Variables { get; set; }
    }
}
