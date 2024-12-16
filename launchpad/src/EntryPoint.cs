using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace launchpad
{
    public static class EntryPoint
    {
        public static void Main(string[] args)
        {
            var parameters = new ConsoleParameters(args);
            if (parameters.MainParametersCount == 0)
            {
                PrintHelp();
                return;
            }

            switch (parameters.GetMainValue(0))
            {
                case "version":
                    HandleVersionCommand(); return;

                case "config":
                    HandleConfigCommand(); return;

                case "list":
                    HandleListCommand(); return;

                case "process":
                    HandleProcessCommand(); return;

                case "reset-config":
                    HandleResetConfigCommand(); return;

                case "set-config":
                    if (parameters.MainParametersCount == 2)
                    {
                        HandleSetConfigCommand(parameters.GetMainValue(1));
                        return;
                    }
                    break;

                case "new":
                    if (parameters.MainParametersCount == 2)
                    {
                        HandleNewCommand(parameters);
                        return;
                    }
                    break;
            }

            PrintHelp();
        }

        private static void HandleListCommand()
        {
            var config = new LaunchpadConfigProvider().GetConfig();

            foreach (var definition in config.Definitions)
            {
                Console.Out.WriteLine($"{definition.Name}");
                Console.Out.WriteLine($"\t(from '{definition.PackageName}' package)");
            }
        }

        private static void HandleNewCommand(ConsoleParameters parameters)
        {
            var templateName = parameters.GetMainValue(1);
            var config = new LaunchpadConfigProvider().GetConfig();

            var packageName = GetPackageNameOrNull(templateName, config);
            if (packageName == null)
            {
                return;
            }

            using (var tempDirectory = new TemporaryDirectory())
            {
                var packageFetcher = new PackageFetcher();
                var specProvider = new LaunchpadSpecProvider();
                var variablesFiller = new VariableFiller();
                var templateProcessor = new TemplateProcessor();
                var nugetSources = GetNugetSources(parameters, config);

                packageFetcher.Fetch(packageName, nugetSources, tempDirectory.FullPath);

                var templateSpec = specProvider.ProvideFrom(tempDirectory.FullPath);
                var variables = variablesFiller.FillVariables(templateSpec.Variables);

                templateProcessor.Process(tempDirectory.FullPath, variables);

                specProvider.CleanupIn(tempDirectory.FullPath);

                tempDirectory.CopyContentsTo(Environment.CurrentDirectory);
            }

            Console.Out.WriteLine();
            Console.Out.WriteLine("Done!");
        }

        private static string[] GetNugetSources(ConsoleParameters parameters, LaunchpadConfig config)
        {
            const string sourceParameter = "--source";

            if (!parameters.HasParameterWithValue(sourceParameter))
                return config.NugetSources;

            return parameters.GetValues(sourceParameter).ToArray();
        }

        private static string GetPackageNameOrNull(string templateName, LaunchpadConfig config)
        {
            var templateDefinition = config.Definitions.FirstOrDefault(d => d.Name == templateName);
            if (templateDefinition != null)
            {
                return templateDefinition.PackageName;
            }

            const string nugetPrefix = "nuget::";
            if (templateName.StartsWith(nugetPrefix))
            {
                return templateName.Remove(0, nugetPrefix.Length);
            }

            Console.Out.WriteLine($"There's no template named '{templateName}'. Use 'list' command to view available ones. Also you can specify nuget package directly like this: 'nuget::Vostok.Launchpad.Templates.Library'.");
            return null;
        }

        private static void HandleProcessCommand()
        {
            var specProvider = new LaunchpadSpecProvider();
            var variablesFiller = new VariableFiller();
            var templateProcessor = new TemplateProcessor();

            var templateSpec = specProvider.ProvideFrom(Environment.CurrentDirectory);
            var variables = variablesFiller.FillVariables(templateSpec.Variables);

            templateProcessor.Process(Environment.CurrentDirectory, variables);

            Console.Out.WriteLine();
            Console.Out.WriteLine("Done!");
        }

        private static void HandleVersionCommand()
        {
            var attribute = typeof (EntryPoint).Assembly.GetCustomAttribute(typeof (AssemblyInformationalVersionAttribute)) as AssemblyInformationalVersionAttribute;

            var version = attribute?.InformationalVersion;

            Console.Out.WriteLine(version);
        }

        private static void HandleConfigCommand()
        {
            Console.Out.WriteLine(JsonConvert.SerializeObject(new LaunchpadConfigProvider().GetConfig(), new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            }));
        }

        private static void HandleSetConfigCommand(string source)
        {
            new LaunchpadConfigProvider().SetupConfigSource(source);
        }

        private static void HandleResetConfigCommand()
        {
            new LaunchpadConfigProvider().ResetToDefaultConfig();
        }

        private static void PrintHelp()
        {
            Console.Out.WriteLine(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "launchpad-help.txt")));
            Console.Out.WriteLine();
        }
    }
}
