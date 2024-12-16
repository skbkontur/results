using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;

public static class Program
{
    public static void Main(string[] args)
    {
        var parameters = new Parameters(args);
        
        Console.Out.WriteLine(
            $"Setting version {parameters.VersionPart} '{parameters.VersionValue}' for all projects of solutions located in '{parameters.WorkingDirectory}'.");

        var solutionFiles = Directory.GetFiles(parameters.WorkingDirectory, "*.sln");
        if (solutionFiles.Length == 0)
        {
            Console.Out.WriteLine("No solution files found.");
            return;
        }

        Console.Out.WriteLine(
            $"Found solution files: {Environment.NewLine}\t{string.Join(Environment.NewLine + "\t", solutionFiles)}");

        foreach (var solutionFile in solutionFiles)
            HandleSolution(solutionFile, parameters);
    }

    private static void HandleSolution(string solutionFile, Parameters parameters)
    {
        var parsedSolution = SolutionFile.Parse(solutionFile);
                
        Console.Out.WriteLine($"Working with '{solutionFile}.");
        Console.Out.WriteLine($"Working with '{parameters.SolutionConfiguration}' solution configuration.");

        var projects = FilterProjectsByConfiguration(parsedSolution.ProjectsInOrder, parameters.SolutionConfiguration);

        var projectFiles = projects
            .Select(project => project.AbsolutePath)
            .ToArray();

        if (projectFiles.Length == 0)
        {
            Console.Out.WriteLine("No projects found in solutions.");
            return;
        }

        Console.Out.WriteLine(
            $"Found project files: {Environment.NewLine}\t{string.Join(Environment.NewLine + "\t", projectFiles)}");

        foreach (var projectFile in projectFiles)
        {
            if (!File.Exists(projectFile))
            {
                Console.Out.WriteLine($"Project file '{projectFile}' doesn't exists.");
                continue;
            }

            Console.Out.WriteLine($"Working with project '{Path.GetFileName(projectFile)}'..");

            var project = Project.FromFile(projectFile, new ProjectOptions
            {
                LoadSettings = ProjectLoadSettings.IgnoreMissingImports
            });

            project.SetProperty(parameters.CsProjProperty, parameters.VersionValue);
            project.Save();
        }
    }

    private static IEnumerable<ProjectInSolution> FilterProjectsByConfiguration(
        IEnumerable<ProjectInSolution> projects,
        string configuration)
    {
        var keyPrefix = configuration + "|";

        foreach (var project in projects)
        {
            var configurations = project.ProjectConfigurations;
            var enabledConfigurations = configurations.Where(x => x.Value.IncludeInBuild);

            if (enabledConfigurations.Any(x => x.Key.StartsWith(keyPrefix, StringComparison.OrdinalIgnoreCase)))
                yield return project;
        }
    }

    private class Parameters
    {
        public string VersionPart { get; }
        public string CsProjProperty { get; }
        public string VersionValue { get; }
        public string WorkingDirectory { get; }
        public string SolutionConfiguration { get; }

        public Parameters(string[] args)
        {
            VersionPart = args.Contains("--version")
                ? "version"
                : args.Contains("--prefix")
                    ? "prefix"
                    : "suffix";

            CsProjProperty = args.Contains("--version")
                ? "Version"
                : args.Contains("--prefix")
                    ? "VersionPrefix"
                    : "VersionSuffix";

            var positionalArgs = args.Where(x => !x.StartsWith("--")).ToArray();

            if (positionalArgs.Length <= 0)
                throw new Exception($"Missing required argument: version {VersionPart}.");

            VersionValue = positionalArgs[0];
            WorkingDirectory = positionalArgs.Length > 1 ? positionalArgs[1] : Environment.CurrentDirectory;
            SolutionConfiguration =
                GetArgsByKey(args, "--solutionConfiguration:").FirstOrDefault() ?? "Release";
        }

        private static IEnumerable<string> GetArgsByKey(string[] args, string key)
        {
            return args
                .Where(x => x.StartsWith(key))
                .Select(x => x.Substring(key.Length).Trim());
        }
    }
}