## Installation for .NET Core 2.1+:

```
dotnet build
dotnet tool update --add-source nupkg -g dotnetcementrefs
```


## Usage examples:

* `dotnetcementrefs` - converts cement references to nuget package references (with latest applicable versions) for all solutions in current directory.
* `dotnetcementrefs <module directory>` - converts cement references to nuget package references (with latest applicable versions) for all solutions in <module directory>.

The following parameters could be specified multiple times:
* `--source:https://api.nuget.org/v3/index.json` - include additional [package source](https://learn.microsoft.com/en-us/nuget/reference/nuget-config-file#package-source-sections)
* `--refPrefix:Vostok.` - reference prefix to check what references should be replaced with the nuget package references (default value is `Vostok.`)
* `--remove:NuGet.` - reference prefix to remove completely instead of replacing them with the nuget package reference
* `--removeMissing:NuGet.` - in case the tool has failed to resolve the nuget package reference, remove the reference with specified prefix

Other parameters:
* `--ignoreMissingPackages` - in case the tool has failed to resolve the nuget package reference and wasn't removed, leave the reference as is instead of stopping with error
* `--solutionConfiguration:Release` - solution configuration to use when processing the references; default is `Release`
* `--allowLocalProjects` - do not replace assembly references to other projects in the same solution
* `--allowPrereleasePackages` - allow prerelease versions in resolved nuget package references
* `--ensureMultitargeted` - check that every reference has multiple targets in path, and one of them is `netstandard2.0`
* `--copyPrivateAssets` - copy `PrivateAssets` element from the source `Reference` to the replacing `PackageReference`
* `--useFloatingVersions` - use `*` instead of the tertiary version number (and prerelease tag; e.g. `1.2.*`)

## Supported msbuild properties in .csproj files

These go in `Project` > `PropertyGroup`

* `DotnetCementRefsUsePrereleaseForPrefixes` - list any prefix (up to the full name of the package) that could reference prerelease version of the package; supported delimiters are `;`, `,` and ` ` (space)
* `DotnetCementRefsExclude` - if set to `true`, the project will be skipped from processing by the tool

Example:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <DotnetCementRefsUsePrereleaseForPrefixes>Vostok.;Microsoft.;NuGet.;</DotnetCementRefsUsePrereleaseForPrefixes>
    <DotnetCementRefsExclude>true</DotnetCementRefsExclude>
  </PropertyGroup>
</Project>
```

## Supported reference properties

These go in `ItemGroup` > `Reference` as attributes

* `NugetPackageName` - name of the nuget package containing this library reference (useful when you have multiple package editions, when the package name doesn't have the same name as the assembly, or when one package contains multiple assemblies)
* `NugetPackageAllowPrerelease` - if set to `true` or `false`, it will override the command line parameter and the project property `DotnetCementRefsUsePrereleaseForPrefixes`

Note that you can't mix `NugetPackageAllowPrerelease` values for multiple assemblies contained in one package, the first encountered value per package name will be used.

Example:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <Reference Include="Vostok.Logging.Abstractions" NugetPackageName="Vostok.Logging.Meta" NugetPackageAllowPrerelease="False">
      <HintPath>..\..\vostok.logging.abstractions\Vostok.Logging.Abstractions\bin\Release\netstandard2.0\Vostok.Logging.Abstractions.dll</HintPath>
    </Reference>
    <Reference Include="Vostok.Logging.Tracing">
      <NugetPackageName>Vostok.Logging.Meta</NugetPackageName>
      <NugetPackageAllowPrerelease>False</NugetPackageAllowPrerelease>
      <SpecificVersion>False</SpecificVersion>
      <HintPath>..\..\vostok.logging.tracing\Vostok.Logging.Tracing\bin\Release\netstandard2.0\Vostok.Logging.Tracing.dll</HintPath>
    </Reference>
  </ItemGroup>
</Project>
```