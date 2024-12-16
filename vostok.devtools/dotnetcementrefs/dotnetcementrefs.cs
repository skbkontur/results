using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cement.Vostok.Devtools;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace dotnetcementrefs;

public static class Program
{
    private static readonly Dictionary<(string package, bool includePrerelease, string[] sourceUrls), NuGetVersion> NugetCache = new();

    public static async Task Main(string[] args)
    {
        var parameters = new Parameters(args);
        await Console.Out.WriteLineAsync($"Converting cement references to NuGet package references for all projects of solutions located in '{parameters.TargetSlnPath}'.");

        var solutionFiles = parameters.TargetSlnPath.EndsWith(".sln", StringComparison.OrdinalIgnoreCase)
            ? Directory.GetFiles(Environment.CurrentDirectory, parameters.TargetSlnPath)
            : Directory.GetFiles(parameters.TargetSlnPath, "*.sln");
        if (solutionFiles.Length == 0)
        {
            await Console.Out.WriteLineAsync("No solution files found.");
            return;
        }

        await Console.Out.WriteLineAsync($"Found solution files: {Environment.NewLine}\t{string.Join(Environment.NewLine + "\t", solutionFiles)}");
        await Console.Out.WriteLineAsync();
        foreach (var solutionFile in solutionFiles)
        {
            await HandleSolutionAsync(solutionFile, parameters).ConfigureAwait(false);
        }
    }

    private static IEnumerable<string> GetArgsByKey(string[] args, string key)
    {
        return args
            .Where(x => x.StartsWith(key))
            .Select(x => x.Substring(key.Length).Trim());
    }

    private static async Task HandleSolutionAsync(string solutionFile, Parameters parameters)
    {
        var solution = SolutionFile.Parse(solutionFile);
        var solutionName = Path.GetFileName(solutionFile);
        await Console.Out.WriteLineAsync($"Working with '{parameters.SolutionConfiguration}' solution configuration.");
        var projects = FilterProjectsByConfiguration(solution.ProjectsInOrder, parameters.SolutionConfiguration).ToArray();
        if (!projects.Any())
        {
            await Console.Out.WriteLineAsync($"No projects found in solution {solutionName}.");
            return;
        }

        var separator = Environment.NewLine + "\t";
        await Console.Out.WriteLineAsync($"Found projects in solution {solutionName}: {Environment.NewLine}\t{string.Join(separator, projects.Select(project => project.AbsolutePath))}");
        await Console.Out.WriteLineAsync();
        var allProjectsInSolution = projects
            .Select(p => p.ProjectName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var solutionProject in projects)
        {
            await HandleProjectAsync(solutionProject, allProjectsInSolution, parameters).ConfigureAwait(false);
        }
    }

    private static IEnumerable<ProjectInSolution> FilterProjectsByConfiguration(IEnumerable<ProjectInSolution> projects, string configuration)
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

    private static async Task HandleProjectAsync(
        ProjectInSolution solutionProject,
        ISet<string> allProjectsInSolution,
        Parameters parameters)
    {
        if (!File.Exists(solutionProject.AbsolutePath))
        {
            await Console.Out.WriteLineAsync($"Project '{solutionProject.AbsolutePath}' doesn't exists.");
            return;
        }

        await Console.Out.WriteLineAsync($"Working with project '{solutionProject.ProjectName}'..");
        var project = Project.FromFile(solutionProject.AbsolutePath, new()
        {
            LoadSettings = ProjectLoadSettings.IgnoreMissingImports
        });
        if (ShouldIgnore(project))
        {
            await Console.Out.WriteLineAsync($"Ignore project  '{solutionProject.ProjectName}' due to DotnetCementRefsExclude property in csproj.");
            return;
        }

        ReplaceModuleReferences(project);

        var references = FindCementReferences(project, allProjectsInSolution, parameters.CementReferencePrefixes);
        if (parameters.AllowLocalProjects)
        {
            references.AddRange(FindLocalProjectReferences(project, allProjectsInSolution));
        }
        if (parameters.EnsureMultitargeted)
        {
            var singleTargeted = references.Select(r => r.DirectMetadata.Single(m => m.Name == "HintPath").UnevaluatedValue).Where(r => !r.Contains("$(") && r.Contains("netstandard2.0")).ToArray();
            if (singleTargeted.Any())
            {
                throw new($"All cement references should support multitargeting and contain $(ReferencesFramework). But {string.Join(",", singleTargeted)} don't.");
            }
        }
        
        if (!references.Any())
        {
            await Console.Out.WriteLineAsync($"No references found in project {solutionProject.ProjectName}.");
            return;
        }

        await Console.Out.WriteLineAsync($"Found references in {solutionProject.ProjectName}: {Environment.NewLine}\t{string.Join(Environment.NewLine + "\t", references.Select(item => item.EvaluatedInclude))}");
        await Console.Out.WriteLineAsync();
        var allowPrereleasePackagesForAll = HasPrereleaseVersionSuffix(project, out var versionSuffix) || parameters.AllowPrereleasePackages;
        if (allowPrereleasePackagesForAll)
        {
            await Console.Out.WriteLineAsync($"Will allow prerelease versions in package references due to prerelease version suffix '{versionSuffix}'.");
        }
        else
        {
            await Console.Out.WriteLineAsync("Won't allow prerelease versions in package due to stable version of the project itself.");
        }
        await Console.Out.WriteLineAsync();
        var usePrereleaseForPrefixes = GetUsePrereleaseForPrefixes(project);
        if (!allowPrereleasePackagesForAll && usePrereleaseForPrefixes.Length > 0)
        {
            await Console.Out.WriteLineAsync($"prerelease allowed for prefixes {string.Join(';', usePrereleaseForPrefixes)} by .csproj properties");
        }
        foreach (var reference in references)
        {
            var allowPrereleaseForThisReference = usePrereleaseForPrefixes.Any(reference.EvaluatedInclude.StartsWith);
            await HandleReferenceAsync(project, reference, allowPrereleasePackagesForAll || allowPrereleaseForThisReference, parameters).ConfigureAwait(false);
        }
        project.Save();
        await Console.Out.WriteLineAsync();
    }

    private static void ReplaceModuleReferences(Project project)
    {
        var moduleReferences = project.ItemsIgnoringCondition
            .Where(item => item.ItemType == "ModuleReference")
            .ToList();

        if (moduleReferences.Count == 0)
        {
            return;
        }

        var workspacePath = FileUtilities.GetDirectoryNameOfDirectoryAbove(project.FullPath, ".cement");
        if (workspacePath == null)
        {
            throw new($"Failed to find cement workspace for '{project.FullPath}'. " +
                      $"Make sure project is inside cement workspace.");
        }

        var moduleReferenceTransformer = new ModuleReferenceTransformer();

        foreach (var moduleReference in moduleReferences)
        {
            try
            {
                var references = moduleReferenceTransformer
                    .ToReferences(workspacePath, moduleReference.EvaluatedInclude);
                
                foreach (var reference in references)
                {
                    if (moduleReference.Xml.Parent is ProjectItemGroupElement groupElement)
                    {
                        groupElement.AddItem("Reference", reference.Include, reference.Metadata);
                    }
                    else
                    {
                        project.AddItem("Reference", reference.Include, reference.Metadata);
                    }

                    project.RemoveItem(moduleReference);
                }
            }
            catch (Exception e)
            {
                throw new($"Failed to replace ModuleReference at '{moduleReference.Xml.Location}'", e);
            }
        }

        project.ReevaluateIfNecessary();
    }

    private static string[] GetUsePrereleaseForPrefixes(Project project)
    {
        var property = project.Properties.FirstOrDefault(x => x.Name == "DotnetCementRefsUsePrereleaseForPrefixes");
        var usePrereleaseForPrefixes = property?.EvaluatedValue.Split(new[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                       ?? new string[] { };
        return usePrereleaseForPrefixes;
    }

    private static bool HasPrereleaseVersionSuffix(Project project, out string? suffix)
    {
        suffix = project.GetProperty("VersionSuffix")?.EvaluatedValue;
        return !string.IsNullOrWhiteSpace(suffix);
    }

    private static bool ShouldIgnore(Project project)
    {
        return project
            .Properties
            .Any(item => item.Name == "DotnetCementRefsExclude" && item.EvaluatedValue.ToLower() is "true");
    }


    private static List<ProjectItem> FindCementReferences(Project project, ISet<string> localProjects,
        string[] cementRefPrefixes)
    {
        return project.ItemsIgnoringCondition
            .Where(item => item.ItemType == "Reference")
            .Where(item => cementRefPrefixes.Any(x => item.EvaluatedInclude.StartsWith(x)))
            .Where(item => !localProjects.Contains(item.EvaluatedInclude))
            .ToList();
    }

    private static List<ProjectItem> FindLocalProjectReferences(Project project, ISet<string> localProjects)
    {
        return project.ItemsIgnoringCondition
            .Where(item => item.ItemType == "Reference")
            .Where(item => localProjects.Contains(item.EvaluatedInclude))
            .ToList();
    }

    private static async Task HandleReferenceAsync(Project project, ProjectItem reference, bool allowPrereleasePackages, Parameters parameters)
    {
        var packageName = reference.GetMetadataValue("NugetPackageName");
        if (packageName is "")
            packageName = reference.EvaluatedInclude;
        if (packageName.Contains(","))
            throw new($"Fix reference format for '{packageName}' (there shouldn't be any explicit versions or architecture references).");

        if (parameters.ReferencesToRemove.Contains(packageName, StringComparer.OrdinalIgnoreCase))
        {
            await Console.Out.WriteLineAsync($"Removed cement reference to '{reference.EvaluatedInclude}'.");
            project.RemoveItem(reference);
            return;
        }

        var explicitPrereleaseFlag = reference.GetMetadataValue("NugetPackageAllowPrerelease").ToLower();
        if (explicitPrereleaseFlag is "true" or "false")
            allowPrereleasePackages = bool.Parse(explicitPrereleaseFlag);
        var packageVersion = await GetLatestNugetVersionWithCacheAsync(packageName, allowPrereleasePackages, parameters.SourceUrls);
        if (packageVersion == null)
        {
            if (parameters.MissingReferencesToRemove.Any(x => packageName.StartsWith(x)))
            {
                await Console.Out.WriteLineAsync($"Removed cement reference to '{reference.EvaluatedInclude}'.");
                project.RemoveItem(reference);
                return;
            }

            if (parameters.FailOnNotFoundPackage)
                throw new($"No versions of package '{packageName}' were found on '{string.Join(", ", parameters.SourceUrls)}'.");
            return;
        }
        await Console.Out.WriteLineAsync($"Latest version of NuGet package '{packageName}' is '{packageVersion}'");

        var itemGroupsWithCementRef = project.Xml.ItemGroups.Where(g => g.Items.Any(r => r.ElementName == "Reference" && r.Include == reference.EvaluatedInclude)).ToList();
        var itemGroupsWithPackageRef = project.Xml.ItemGroups.Where(ig => ig.Items.Any(i => i.ElementName == "PackageReference" && i.Include == packageName)).ToList();
        var metadata = ConstructMetadata(reference, parameters, packageVersion);
        if (itemGroupsWithCementRef.Any())
            foreach (var group in itemGroupsWithCementRef)
            {
                if (itemGroupsWithPackageRef.Any(ig => ReferenceEquals(ig, group)))
                    continue;
                
                group.AddItem("PackageReference", packageName, metadata);
            }
        else
        {
            if (!project.Items.Any(i => i.ItemType == "PackageReference" && i.EvaluatedInclude == packageName))
                project.AddItem("PackageReference", packageName, metadata);
        }
        await Console.Out.WriteLineAsync($"Added package reference to '{packageName}' of version '{metadata.First().Value}'.");
        project.RemoveItem(reference);
        await Console.Out.WriteLineAsync($"Removed cement reference to '{reference.EvaluatedInclude}'.");
        await Console.Out.WriteLineAsync();
    }

    private static IEnumerable<KeyValuePair<string, string>> ConstructMetadata(ProjectItem reference, Parameters parameters, NuGetVersion packageVersion)
    {
        var version = packageVersion.ToString();
        if (parameters.UseFloatingVersions)
        {
            version = $"{packageVersion.Version.Major}.{packageVersion.Version.Minor}.";
            version += packageVersion.IsPrerelease ? "*-*" : "*";
        }
        var metadata = new List<KeyValuePair<string, string>> { new("Version", version) };
        var privateAssets = reference.GetMetadataValue("PrivateAssets");
        if (parameters.CopyPrivateAssetsMetadata && !string.IsNullOrEmpty(privateAssets))
            metadata.Add(new("PrivateAssets", privateAssets));
        return metadata;
    }

    private static async Task<NuGetVersion?> GetLatestNugetVersionWithCacheAsync(string package, bool includePrerelease, string[] sourceUrls)
    {
        if (NugetCache.TryGetValue((package, includePrerelease, sourceUrls), out var value))
            return value;

        var version = await GetLatestNugetVersionDirectAsync(package, includePrerelease, sourceUrls).ConfigureAwait(false);
        if (version is not null)
            NugetCache.Add((package, includePrerelease, sourceUrls), version);
        return version;
    }

    private static async Task<NuGetVersion?> GetLatestNugetVersionDirectAsync(string package, bool includePrerelease, string[] sourceUrls)
    {
        const int attempts = 3;
        for (var attempt = 1; attempt <= attempts; attempt++)
        {
            try
            {
                foreach (var source in sourceUrls)
                {
                    var latestVersion = await GetLatestNugetVersionAsync(package, includePrerelease, source).ConfigureAwait(false);
                    if (latestVersion != null)
                    {
                        return latestVersion;
                    }
                }
                return null;
            }
            catch (Exception error)
            {
                if (attempt == attempts)
                    throw;

                await Console.Out.WriteLineAsync($"Failed to fetch version of package '{package}'. Attempt = {attempt}. Error = {error}.");
            }
        }
        return null;
    }

    private static async Task<NuGetVersion?> GetLatestNugetVersionAsync(string package, bool includePrerelease, string sourceUrl)
    {
        var providers = new List<Lazy<INuGetResourceProvider>>();
        providers.AddRange(Repository.Provider.GetCoreV3());
        var packageSource = new PackageSource(sourceUrl);
        var sourceRepository = new SourceRepository(packageSource, providers);
        var metadataResource = sourceRepository.GetResource<PackageMetadataResource>();
        var searchResult = await metadataResource.GetMetadataAsync(
            package,
            includePrerelease,
            false,
            new(),
            new NullLogger(),
            CancellationToken.None
        ).ConfigureAwait(false);
        var versions = searchResult 
            .Where(data => data.Identity.Id == package)
            .OrderBy(data => data.Published)
            .Select(data => data.Identity.Version)
            .ToArray();
        if (versions.Length == 0)
            return null;
        
        var maxVer = versions.Max();
        // semver doesn't sort suffix numerically, so .Max() will return the oldest prerelease version
        // there could be a better solution with proper string comparer,
        // but it'll only help if all prerelease versions have the same tag name with numbered suffix
        // additionally, if there's a release version available, it must be the most relevant one
        // even if there are later prerelease versions published after it
        var latest = versions.LastOrDefault(v => v.Version == maxVer.Version && !v.IsPrerelease)
            ?? versions.Last(v => v.Version == maxVer.Version);
        return latest;
    }

    private class Parameters
    {
        public string TargetSlnPath { get; }
        public string SolutionConfiguration { get; }
        public string[] SourceUrls { get; }
        public string[] CementReferencePrefixes { get; }
        public string[] MissingReferencesToRemove { get; }
        public string[] ReferencesToRemove { get; }
        public bool FailOnNotFoundPackage { get; }
        public bool AllowLocalProjects { get; }
        public bool AllowPrereleasePackages { get; }
        public bool EnsureMultitargeted { get; }
        public bool CopyPrivateAssetsMetadata { get; }
        public bool UseFloatingVersions { get; }

        public Parameters(string[] args)
        {
            var positionalArgs = args.Where(x => !x.StartsWith("-")).ToArray();
            TargetSlnPath = positionalArgs.Length > 0 ? positionalArgs[0] : Environment.CurrentDirectory;
            SourceUrls = GetArgsByKey(args, "--source:").ToArray();
            CementReferencePrefixes = new[] {"Vostok."}.Concat(GetArgsByKey(args, "--refPrefix:")).ToArray();
            MissingReferencesToRemove = GetArgsByKey(args, "--removeMissing:").ToArray();
            ReferencesToRemove = GetArgsByKey(args, "--remove:").ToArray();
            FailOnNotFoundPackage = !args.Contains("--ignoreMissingPackages");
            SolutionConfiguration = GetArgsByKey(args, "--solutionConfiguration:").FirstOrDefault() ?? "Release";
            AllowLocalProjects = args.Contains("--allowLocalProjects");
            AllowPrereleasePackages = args.Contains("--allowPrereleasePackages");
            EnsureMultitargeted = args.Contains("--ensureMultitargeted");
            CopyPrivateAssetsMetadata = args.Contains("--copyPrivateAssets");
            UseFloatingVersions = args.Contains("--useFloatingVersions");
        }
    }
}
