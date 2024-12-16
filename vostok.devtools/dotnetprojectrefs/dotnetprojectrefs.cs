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
            $"Converting cement references to NuGet package references for all projects of solutions located in '{parameters.WorkingDirectory}'.");

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

        var allProjectsInSolution = projects
            .Select(p => p.ProjectName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var solutionProject in projects)
        {
            HandleProject(solutionProject, allProjectsInSolution, parameters);
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

    private static void HandleProject(
        ProjectInSolution solutionProject,
        ISet<string> allProjectsInSolution,
        Parameters parameters)
    {
        if (!File.Exists(solutionProject.AbsolutePath))
        {
            Console.Out.WriteLine($"Project '{solutionProject.AbsolutePath}' doesn't exists.");
            return;
        }

        Console.Out.WriteLine($"Working with project '{solutionProject.ProjectName}'..");

        var project = Project.FromFile(solutionProject.AbsolutePath, new ProjectOptions
        {
            LoadSettings = ProjectLoadSettings.IgnoreMissingImports
        });

        if (ShouldIgnore(project))
        {
            Console.Out.WriteLine(
                $"Ignore project  '{solutionProject.ProjectName}' due to DotnetCementRefsExclude property in csproj.");
            return;
        }

        var references = FindProjectReferences(project);

        if (!references.Any())
        {
            Console.Out.WriteLine($"No references found in project {solutionProject.ProjectName}.");
            return;
        }

        Console.Out.WriteLine(
            $"Found references in {solutionProject.ProjectName}: {Environment.NewLine}\t{string.Join(Environment.NewLine + "\t", references.Select(item => item.EvaluatedInclude))}");
        Console.Out.WriteLine();

        var version = project.GetProperty("Version");

        Console.Out.WriteLine($"Future version of all NuGet packages is '{version}'");

        Console.Out.WriteLine();

        foreach (var reference in references)
        {
            HandleProjectReference(project, reference, version?.EvaluatedValue, parameters);
        }

        project.Save();

        Console.Out.WriteLine();
    }

    private static bool ShouldIgnore(Project project)
    {
        return project
            .Properties
            .Any(item => item.Name == "DotnetCementRefsExclude" &&
                         item.EvaluatedValue == "true");
    }

    private static List<ProjectItem> FindProjectReferences(Project project)
    {
        return project.GetItems("ProjectReference")
            .ToList();
    }

    private static void HandleProjectReference(Project project, ProjectItem reference, string version, Parameters parameters)
    {
        var packageName = AdjustPackageName(reference.EvaluatedInclude);

        //note skip project-to project dependencies without dll reference
        if (reference.Metadata.Any(x =>
            x.Name == "ReferenceOutputAssembly" &&
            string.Equals(x.EvaluatedValue, "false", StringComparison.OrdinalIgnoreCase)))
        {
            Console.Out.WriteLine($"Skip dependency reference to '{reference.EvaluatedInclude}'.");
            return;
        }

        if (parameters.ReferencesToRemove.Contains(packageName, StringComparer.OrdinalIgnoreCase))
        {
            Console.Out.WriteLine($"Removed project reference to '{reference.EvaluatedInclude}'.");
            project.RemoveItem(reference);
            return;
        }

        if (version == null)
        {
            if (parameters.MissingReferencesToRemove.Any(x => packageName.StartsWith(x)))
            {
                Console.Out.WriteLine($"Removed project reference to '{reference.EvaluatedInclude}'.");
                project.RemoveItem(reference);
                return;
            }


            if (parameters.FailOnNotFoundPackage)
                throw new Exception(
                    $"No versions of package '{packageName}' were found on '{string.Join(", ", parameters.SourceUrls)}'.");
            return;
        }

        project.RemoveItem(reference);

        Console.Out.WriteLine($"Removed project reference to '{reference.EvaluatedInclude}'.");

        project.AddItem("PackageReference", packageName, new[]
        {
            new KeyValuePair<string, string>("Version", version)
        });

        Console.Out.WriteLine($"Added package reference to '{packageName}' of version '{version}'.");
        Console.Out.WriteLine();
    }

    private static string AdjustPackageName(string referenceEvaluatedInclude)
    {
        return referenceEvaluatedInclude.Split(new []{"\\", "/"}, StringSplitOptions.RemoveEmptyEntries)
            .Last()
            .Replace(".csproj", String.Empty);
    }

    private class Parameters
    {
        public string WorkingDirectory { get; }
        public string SolutionConfiguration { get; }
        public string[] SourceUrls { get; }
        public string[] CementReferencePrefixes { get; }
        public string[] MissingReferencesToRemove { get; }
        public string[] ReferencesToRemove { get; }
        public bool FailOnNotFoundPackage { get; }
        public bool AllowLocalProjects { get; }

        public Parameters(string[] args)
        {
            var positionalArgs = args.Where(x => !x.StartsWith("-")).ToArray();
            WorkingDirectory = positionalArgs.Length > 0 ? positionalArgs[0] : Environment.CurrentDirectory;
            SourceUrls = new[] {"https://api.nuget.org/v3/index.json"}.Concat(GetArgsByKey(args, "--source:"))
                .ToArray();
            CementReferencePrefixes = new[] {"Vostok."}.Concat(GetArgsByKey(args, "--refPrefix:")).ToArray();
            MissingReferencesToRemove = GetArgsByKey(args, "--removeMissing:").ToArray();
            ReferencesToRemove = GetArgsByKey(args, "--remove:").ToArray();
            FailOnNotFoundPackage = !args.Contains("--ignoreMissingPackages");
            SolutionConfiguration = GetArgsByKey(args, "--solutionConfiguration:").FirstOrDefault() ?? "Release";
            AllowLocalProjects = args.Contains("--allowLocalProjects");
        }
    }
}
