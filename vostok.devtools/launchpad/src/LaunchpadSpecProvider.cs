using System.IO;
using Newtonsoft.Json;

namespace launchpad
{
    internal class LaunchpadSpecProvider
    {
        private const string FileName = "launchpad.json";

        public LaunchpadSpec ProvideFrom(string directory)
        {
            var specContent = File.ReadAllText(Path.Combine(directory, FileName));

            return JsonConvert.DeserializeObject<LaunchpadSpec>(specContent);
        }

        public void CleanupIn(string directory)
        {
            File.Delete(Path.Combine(directory, FileName));
        }
    }
}
