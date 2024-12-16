using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Build.Construction;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

public static class Program
{
    public static void Main(string[] args)
    {
        var workingDirectory = args.Length > 0 ? args[0] : Environment.CurrentDirectory;

        Console.Out.WriteLine($"Releases vostok library located in '{workingDirectory}'.");

        var solutionFiles = Directory.GetFiles(workingDirectory, "*.sln");        
        
        if (solutionFiles.Length == 0)
        {
            Console.Out.WriteLine("No solution files found.");
            return;
        }

        Console.Out.WriteLine($"Found solution files: {Environment.NewLine}\t{string.Join(Environment.NewLine + "\t", solutionFiles)}");
        Console.Out.WriteLine();

        foreach (var solutionFile in solutionFiles)
        {
            HandleSolution(solutionFile);
        }
    }

    private static void HandleSolution(string solutionFile)
    {
        var solution = SolutionFile.Parse(solutionFile);
        var solutionName = Path.GetFileName(solutionFile);

        Console.Out.WriteLine($"Found projects in solution {solutionName}: {Environment.NewLine}\t{string.Join(Environment.NewLine + "\t", solution.ProjectsInOrder.Select(project => project.AbsolutePath))}");
        Console.Out.WriteLine();

        var projectInSolution = solution.ProjectsInOrder.Single(x => !x.ProjectName.EndsWith(".Tests") && !x.ProjectName.EndsWith(".Benchmark"));
        
        HandleProject(projectInSolution);
    }

    private static void HandleProject(ProjectInSolution solutionProject)
    {
        if (!File.Exists(solutionProject.AbsolutePath))
        {
            Console.Out.WriteLine($"Project '{solutionProject.AbsolutePath}' doesn't exists.");
            return;
        }
    
        Console.Out.WriteLine($"Working with project '{solutionProject.ProjectName}'..");

        var projectRoot = ProjectRootElement.Open(solutionProject.AbsolutePath, ProjectCollection.GlobalProjectCollection, true);
        var project = Project.FromProjectRootElement(projectRoot, new ProjectOptions
        {
            LoadSettings = ProjectLoadSettings.IgnoreMissingImports
        });

        var version = project.GetPropertyValue("VersionPrefix");

        var changelog = File.ReadAllText("CHANGELOG.md");
        
        if (!changelog.Contains($@"## {version} ("))
            throw new Exception($"Describe changes for version {version} in CHANGELOG.md before release!");

        var latestNugetVersion = GetLatestNugetVersion(solutionProject.ProjectName, false);
        if (latestNugetVersion != null && latestNugetVersion.Version >= Version.Parse(version))
            throw new Exception($"Bump version first. Version {latestNugetVersion.Version.ToShortString()} found on nuget.org");

        Console.Out.WriteLine($"Release version {version}.");
        Exec("git tag release/" + version).exitCode.EnsureSuccess();
        Exec("git push origin release/" + version).exitCode.EnsureSuccess();

        var oldVersion = Version.Parse(version);
        var newVersion = new Version(oldVersion.Major, oldVersion.Minor, oldVersion.Build + 1, 0)
            .ToShortString();

        Console.Out.WriteLine($"Bump version in csproj to {newVersion}.");
        
        project.SetProperty("VersionPrefix", newVersion);
        project.Save();

        var publicApiFiles = RotateShipped(solutionProject);
        
        Exec($@"git add ""{solutionProject.ProjectName}""").exitCode.EnsureSuccess();
        if (publicApiFiles)
        {
            Exec(@"git add ""*PublicAPI.Shipped.txt""").exitCode.EnsureSuccess();
            Exec(@"git add ""*PublicAPI.Unshipped.txt""").exitCode.EnsureSuccess();
        }
        Exec($@"git commit -m ""Bumped version to {newVersion}"".").exitCode.EnsureSuccess();
        Exec("git push").exitCode.EnsureSuccess();
        
        Console.Out.WriteLine();
    }

    private static bool RotateShipped(ProjectInSolution solutionProject)
    {
        var changes = false;
        var publicApiFiles = Directory.GetFiles(Path.GetDirectoryName(solutionProject.AbsolutePath), "PublicAPI.Unshipped.txt", SearchOption.AllDirectories);
        foreach (var unshipped in publicApiFiles)
        {
            var shipped = unshipped.Replace("PublicAPI.Unshipped.txt", "PublicAPI.Shipped.txt");
            var unshippedLines = GetLines(unshipped);
            var shippedLines = GetLines(shipped);
            
            if (!unshippedLines.Any())
                continue;

            foreach (var unshippedLine in unshippedLines)
            {
                if (unshippedLine.StartsWith("*REMOVED*", StringComparison.InvariantCultureIgnoreCase))
                    shippedLines = shippedLines.Where(l => !string.Equals($"*REMOVED*{l}", unshippedLine, StringComparison.InvariantCultureIgnoreCase)).ToList();
                else
                    shippedLines.Add(unshippedLine);
            }
            
            shippedLines.Sort();
            File.WriteAllLines(shipped, shippedLines, Encoding.UTF8);
            File.WriteAllText(unshipped, "");
            changes = true;
        }

        return changes;

        List<string> GetLines(string path) =>
            File.ReadAllLines(path).Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToList();
    }

    private static (int exitCode, string stdOut, string stdErr) Exec(string command)
    {
        var parts = command.Split(' ', 2);
        
        var process = Process.Start(new ProcessStartInfo
        {
            FileName = parts[0],
            Arguments = parts.Length > 1 ? parts[1] : "",
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        });

        var stdOut = process.StandardOutput.ReadToEnd();
        var stdErr = process.StandardError.ReadToEnd();
        var exitCode = process.ExitCode;

        if (!string.IsNullOrEmpty(stdOut))
            Console.WriteLine(stdOut);
        
        if (!string.IsNullOrEmpty(stdErr))
            Console.WriteLine(stdErr);

        return (exitCode, stdOut, stdErr);
    }

    private static void EnsureSuccess(this int exitCode)
    {
        if (exitCode != 0)
            throw new Exception(exitCode.ToString());
    }

    private static string ToShortString(this Version version)
    {
        return version.ToString(3);
    }

    private static NuGetVersion GetLatestNugetVersion(string package, bool includePrerelease)
    {
        var providers = new List<Lazy<INuGetResourceProvider>>();

        providers.AddRange(Repository.Provider.GetCoreV3());

        var sourceUrl = "https://api.nuget.org/v3/index.json";

        var packageSource = new PackageSource(sourceUrl);

        var sourceRepository = new SourceRepository(packageSource, providers);

        var metadataResource = sourceRepository.GetResource<PackageMetadataResource>();

        var versions = metadataResource.GetMetadataAsync(package, includePrerelease, false, new SourceCacheContext(), new NullLogger(), CancellationToken.None)
            .GetAwaiter()
            .GetResult()
            .Where(data => data.Identity.Id == package)
            .Select(data => data.Identity.Version)
            .ToArray();

        return versions.Any()
            ? versions.Max()
            : null;
    }
}
