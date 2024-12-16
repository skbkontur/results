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

        var solutionFiles = Directory.GetFiles(parameters.WorkingDirectory, "*.sln");
        if (solutionFiles.Length == 0)
        {
            Console.Out.WriteLine("No solution files found.");
            return;
        }

        Console.Out.WriteLine(
            $"Found solution files: {Environment.NewLine}\t{string.Join(Environment.NewLine + "\t", solutionFiles)}");
        Console.Out.WriteLine();

        foreach (var solutionFile in solutionFiles)
        {
            HandleSolution(solutionFile, parameters);
        }
    }

    private static IEnumerable<string> GetArgsByKey(string[] args, string key)
    {
        return args
            .Where(x => x.StartsWith(key))
            .Select(x => x.Substring(key.Length).Trim());
    }

    private static void HandleSolution(string solutionFile, Parameters parameters)
    {
        var solution = SolutionFile.Parse(solutionFile);
        var solutionName = Path.GetFileName(solutionFile);

        Console.Out.WriteLine($"Working with '{parameters.SolutionConfiguration}' solution configuration.");

        var projects = FilterProjectsByConfiguration(solution.ProjectsInOrder, parameters.SolutionConfiguration)
            .ToArray();

        if (!projects.Any())
        {
            Console.Out.WriteLine($"No projects found in solution {solutionName}.");
            return;
        }

        Console.Out.WriteLine(
            $"Found projects in solution {solutionName}: {Environment.NewLine}\t{string.Join(Environment.NewLine + "\t", projects.Select(project => project.AbsolutePath))}");
        Console.Out.WriteLine();


        var graph = new Dictionary<string, string[]>();
        foreach (var solutionProject in projects)
        {
            var referencedProjects = GetReferencedProjects(solutionProject)
                .Select(x => Path.GetFullPath(Path.Combine(Path.GetDirectoryName(solutionProject.AbsolutePath), x)));

            var dependenciesAsProjectNames = solutionProject.Dependencies.Select(d => solution.ProjectsByGuid[d].RelativePath)
                .Select(x => Path.GetFullPath(Path.Combine(Path.GetDirectoryName(solutionFile), x)));
            
            graph[solutionProject.AbsolutePath] = referencedProjects.Concat(dependenciesAsProjectNames).Distinct().ToArray();
        }

        var sorted = SortTopological(graph);

        Console.Out.WriteLine($"Projects in topological sorted order:\r\n\t{string.Join("\r\n\t", sorted)}");
        File.WriteAllLines(parameters.OutputFile, sorted);
    }

    private static string[] SortTopological(Dictionary<string, string[]> graph)
    {
        var visited = new HashSet<string>();
        var ordered = new List<string>();
        foreach (var node in graph.Keys)
        {
            Visit(node, visited, graph, ordered);
        }

        return ordered.ToArray();
    }

    private static void Visit(
        string node, 
        HashSet<string> visited, 
        Dictionary<string, string[]> graph,
        List<string> ordered)
    {
        if (visited.Contains(node))
            return;

        visited.Add(node);
        if (graph.TryGetValue(node, out var children))
        {
            foreach (var child in children)
            {
                Visit(child, visited, graph, ordered);
            }

            ordered.Add(node);
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

    private static string[] GetReferencedProjects(ProjectInSolution solutionProject)
    {
        if (!File.Exists(solutionProject.AbsolutePath))
        {
            Console.Out.WriteLine($"Project '{solutionProject.AbsolutePath}' doesn't exists.");
            return new string[0];
        }

        var project = Project.FromFile(solutionProject.AbsolutePath, new ProjectOptions
        {
            LoadSettings = ProjectLoadSettings.IgnoreMissingImports
        });
        
        var references = FindProjectReferences(project);

        return references.Select(x => x.EvaluatedInclude).ToArray();
    }

    private static List<ProjectItem> FindProjectReferences(Project project)
    {
        return project.GetItems("ProjectReference")
            .ToList();
    }

    private class Parameters
    {
        public string WorkingDirectory { get; }
        public string SolutionConfiguration { get; }
        public string OutputFile { get; }

        public Parameters(string[] args)
        {
            var positionalArgs = args.Where(x => !x.StartsWith("-")).ToArray();
            WorkingDirectory = positionalArgs.Length > 0 ? positionalArgs[0] : Environment.CurrentDirectory;
            SolutionConfiguration = GetArgsByKey(args, "--solutionConfiguration:").FirstOrDefault() ?? "Release";
            OutputFile = GetArgsByKey(args, "--output:").FirstOrDefault() ?? "orderedProjects";
        }
    }
}
