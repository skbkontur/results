## Vostok library development conventions

### Table of contents
* [Module granularity](#module-granularity)
* [Cement module requirements](#cement-module-requirements)
* [Project requirements](#project-requirements)
	* [Main project](#main-project)
		* [Common](#common)
		* [Versioning](#versioning)
		* [Packaging](#packaging)
		* [Changelog](#changelog)
		* [References](#references)
		* [Visibility](#visibility)
	* [Tests project](#tests-project)
		* [Common](#common)
		* [Packaging](#packaging)
		* [Testing tools](#testing-tools)
		* [Test naming and placement](#test-naming-and-placement)
		* [Test practices and bad smells](#test-practices-and-bad-smells)
* [Code-related practices](#code-related-practices)
* [Versioning](#versioning)
* [Git workflow](#git-workflow)
* [NuGet publishing](#nuget-publishing)
* [Publishing process](#publishing-process)
* [Continuous integration](#continuous-integration)
* [Change log](#change-log)

<br/>


### Module granularity
* 1 Cement module = 1 Git repository = 1 published NuGet package

* It's generally a good idea to separate public interfaces from implementations. A good example of such separation is [logging.abstractions](https://github.com/vostok/logging.abstractions) library with core interfaces like `ILog` and implementation libraries, such as [logging.console](https://github.com/vostok/logging.console) and [logging.file](https://github.com/vostok/logging.file).

* The most general guideline is to design modules with their external dependencies in mind. We want our users to be able to consume what they wish without pulling any unnecessary dependency packages. This implies that one module should not contain subsets of code with notably different dependencies: if a developer finds himself pulling a JSON parser into an otherwise pristine netstandard library while working on a feature, he should strongly consider moving that particular thing into a module of its own.

* One important exception to the rule above is when an external dependency is only needed for internal consumption and is not exposed from public API in any way. In such cases it's acceptable to just merge this dependency into library with a tool such as ILRepack. References to merged dependencies should be marked private so that they are not listed in final package.  

* Useful classes that are shared between multiple library projects but should not be exposed to consumers of these projects are put into [source-only libraries](src-libs-conventions.md). 

<br/>

### Cement module requirements
* Every Cement module should have 2 configurations defined in `module.yaml`:
	* `notests` which does not include unit tests and is a default configuration for Cement consumers.
	* `full-build` which inherits from `notests` and includes unit tests.
* The build tool in `module.yaml` should be explicitly set to `dotnet`.
* Install section of `module.yaml` should point to a binary built in release mode.
* Install section of `module.yaml` should also contain references to other Сement modules and NuGet packages whose contents are exposed from public APIs.
	* Case 1: module `logging.serilog` exposes an implementation of `ILog` from `vostok.logging.abstractions`, so its install section should contain `module vostok.logging.abstractions`.
	* Case 2: module `logging.serilog` exposes a class constructed from Serilog's `ILogger`, so its install section should contain `nuget Serilog/2.7.1`.
	* Example of full install section:
```
  install:
    - Vostok.Logging.Serilog/bin/Release/netstandard2.0/Vostok.Logging.Serilog.dll
    - module vostok.logging.abstractions
    - nuget Serilog/2.7.1
```

<br/>

### Project requirements

* Library solution should contain 2 projects: the main one and a unit test project.
* The main project should be named `Vostok.<Something>`.
* The tests project should be named `Vostok.<Something>.Tests`.

Most of the project-related requirements are automatically met when using a [Launchpad](../launchpad) template named `vostok-library`. See [template source](../launchpad-templates/library-ordinary/template) for details. 

<br/>

#### Main project

##### Common
* Should target `netstandard2.0`.
* Should generate xml-docs file during build.
* Should include Git commit info in assembly attributes using [git-commit-to-assembly-title](https://github.com/vostok/devtools/tree/master/git-commit-to-assembly-title) target from `vostok.devtools` module.
* Should include common props in assembly attributes using [Main-Project.props](https://github.com/vostok/devtools/blob/master/library-common-props/Main-Project.props).

##### Versioning
* Version in `.csproj` should be split into `VersionPrefix` and `VersionSuffix`.
* `VersionPrefix` is the one and only true source of version. It gets bumped manually in `.csproj`.
* `VersionSuffix` is not for manual editing. CI tools are responsible for changing it on prerelease builds.
* `FileVersion`, `AssemblyVersion` and `PackageVersion` are all derived from `Version`.

##### Packaging
* Packages should not be generated automatically during build.
* Packages with symbols should be generated during packing.
* There should be no separate `.nuspec` file. All packaging metadata should be kept in `.csproj`.
* Following packaging metadata should be correctly filled and maintained in `.csproj`:
	* `Authors` = `Vostok team`
	* `Product` = `Vostok`
	* `Company` = `SKB Kontur`
	* `Copyright` = `Copyright (c) 2017-2018 SKB Kontur`
	* `Description` with a gist of what the library does
	* `Title` with a short version of description
	* `RepositoryType` = `git`
	* `RepositoryUrl` with a link to library's GitHub repository.
	* `PackageProjectUrl` with a link to library's GitHub repository.
	* `PackageLicenseUrl` with a link to license text in library's GitHub repository.
	* `PackageRequireLicenseAcceptance` = `false`
	* `PackageIconUrl` = `https://raw.githubusercontent.com/vostok/devtools/master/assets/Vostok-icon.png`
	* `PackageTags` = `vostok vostok.<library name>`

##### Changelog
* Package release notes should be maintained in a `CHANGELOG.md` file in the repository root. Link to this file should be included to csproj as a `PackageReleaseNotes` property during build. Change log file should be structured in following way:
	* An H2 header with version and date for each release. Example: `## 0.1.1 (10.09.2018):`.
	* Changes for each version should be organized as a list.
	* Changes that address existing GitHub issue should be augmeneted with issue links.
	
Example of a well-formed changelog file:
```
## 0.1.1 (10.09.2018):

* FileLog now creates all the directories on the way to log file (https://github.com/vostok/logging.file/issues/5)

* RollingUpdateCooldown was renamed to FileSettingsUpdateCooldown to indicate that it's not exclusively purposed for rolling settings.


## 0.1.0 (06-09-2018): 

Initial prerelease.
```
	

##### References
* References to other Vostok modules should only be added with `cm ref add` Cement command.
* References to other NuGet packages which are then merged with ILRepack should be marked with `<PrivateAssets>all</PrivateAssets>` tag.
* Links to shared classes from source-only libraries should be prefixed with `Commons\` without any deeper folder structure. For example: 
```xml
        <ItemGroup>
	    <Compile Include="..\..\vostok.commons.helpers\Vostok.Commons.Helpers\Extensions\TaskExtensions.cs" Link="Commons\TaskExtensions.cs" />
       <ItemGroup/>
```


##### Visibility
* There should be no projects with access to internal classes other than unit tests project. That means no `[InternalsVisibleTo]` attributes pointing to assemblies from other modules.

<br/>

#### Tests project

##### Common
* Should target all contemporary .NET runtimes.
* Should include Git commit info in assembly attributes using [git-commit-to-assembly-title](https://github.com/vostok/devtools/tree/master/git-commit-to-assembly-title) target from `vostok.devtools` module.
* Should include common props in assembly attributes using [Test-Project.props](https://github.com/vostok/devtools/blob/master/library-common-props/Test-Project.props).

##### Packaging
* Should not be able to produce packages: `<IsPackable>false</IsPackable>`

##### Testing tools
* Should use NUnit3 as a main unit testing library.
* Should use FluentAssertions for.. well, assertions.
* Should use NSubstitute for mocks.
* Should use BenchmarkDotNet for performance benchmarks.

##### Test naming and placement
* Test fixture for a class should be named by adding a suffix `_Tests` to the class name (`MyClass` + `MyClass_Tests`).
* Benchmarks should be named by adding a suffix `_Benchmarks` to the class name (`MyClass` + `MyClass_Benchmarks`).
* Smoke tests should be named by adding a suffix `_Tests_Smoke` to the class name (`MyClass` + `MyClass_Tests_Smoke`).
* Tests folder structure should mirror corresponding structure of tested classes (`Helpers\MyClass` + `Helpers\MyClass_Tests`).
* Tests for multiple classes should not be placed into a single file.
* Test names should be formed using one of the following patterns:
  * `{MethodName}_should_{verb for expected behaviour}`
  * `{MethodName}_should_{verb for expected behaviour}_when_{condition description}`
  * `{PropertyName}_should_{verb for expected behaviour}`
  * `{PropertyName}_should_{verb for expected behaviour}_when_{condition description}`
  * `Should_{verb for expected behaviour}`
  * `Should_{verb for expected behaviour}_when_{condition description}`

##### Test practices and bad smells
* Tests should not rely on fixed-length pauses with `Thread.Sleep` or `Task.Delay`.
* Tests should not rely on outcome of random operations (smoke tests are a notable exception to this rule).
* Tests should not be long (anything measured in seconds is already a problem).
* Smoke tests and benchmarks should be marked with `[Explicit]` attribute.

<br/><br/>

### Code-related practices
* Should use [code style](https://github.com/vostok/devtools/tree/master/code-style-csharp) provided in `vostok.devtools` module.
* Should include [JetBrains Annotations](https://www.jetbrains.com/help/resharper/Reference__Code_Annotation_Attributes.html) as a link from [vostok.devtools/jetbrains-annotations](https://github.com/vostok/devtools/blob/master/jetbrains-annotations/JetBrainsAnnotations.cs). The link should be named `Annotations\JetBrainsAnnotations.cs`.
* Should decorate appropriate pieces of code with following JetBrains Annotations:
	* `[CanBeNull]` and `[ItemCanBeNull]`
	* `[NotNull]` and `[ItemNotNull]`
	* `[PublicApi]`
	* `[Pure]`

* Every class or interface comprising library's public API should be supplied with xml-docs.
* Every non-trivial class should be unit-tested.
* Internal classes should be exposed to test project or another libraries with `InternalsVisibleTo` assembly attribute.
* All awaited asynchronous calls must be configured with `.ConfigureAwait(false)`.

#### Xml-doc conventions

* Docs for constructors should link to the doc of their class, e. g. `/// <inheritdoc cref="MyClass"/>`.
* Text of list items should be wrapped in a `<description>` tag. Example:
```
/// <list type="bullet">
///     <item><description>a</description></item>
///     <item><description>b</description></item>
///     <item><description>c</description></item>
/// </list>
```
* An em-dash(—) should be used where required by rules of punctuation. Do not replace it with hyphen(-).

<br/>

### Versioning
* Release versions should be selected and incremented according to [SemVer 1.0](https://semver.org/spec/v1.0.0.html)
* Prerelease versions should be produced by adding a suffix of `-preXXXXXX` format to an ordinary version.
	* `XXXXXX` is a zero-padded six-digit build number which grows monotonically. 


<br/>


### Git workflow

**Force push is not allowed in any circumstances!**

#### Branches
* Main module's branch is `master`. Only stable and tested code should get into `master`, because that's what Cement users always consume. The only exception to this rule is early stage of development (prior to version `1.0.0`).

* All of the development happens in feature branches, which get merged into `master` upon completion, successful code review and testing.

* `master` branch of a Vostok library should only depend on `master` branches of other Vostok libraries.

* Feature branch of a Vostok library may depend on feature branches of other Vostok libraries.


#### Tags
* `release/{version}` tag on a commit in `master` branch marks a stable release for publishing. The version in the tag name is cosmetic and only serves informational purpose. It's recommended to set it same to project's version. Example: `release/1.0.4`.

* `release/{version}` tags on commits in any other branch are ignored and should not exist. 

* `prerelease/{anything}` tag on a commit in a feature branch marks an unstable prerelease for publishing. Example = `prerelease/special-for-my-bro-from-other-team`.

* `prerelease/{anything}` tags on commits in `master` branch are redundant as any commit in `master` branch qualifies as prerelease by default.


<br/>


### NuGet publishing
* Both release and prerelease packages should be published to [nuget.org](https://www.nuget.org/).


<br/>


### Publishing process
#### Unstable (prerelease)
* Every push to `master` branch of a Vostok library is considered an unstable prerelease and produces a package with `-pre..` suffix in version.

* To publish a prerelease package from a feature branch, one should put a `prerelease/..` tag on one of its commits.

#### Stable (release)

* To publish a release package, one should do the following:
	* Update project release notes with new changes.
	* Put a `release/..` tag on the master branch commit.
	* Increase the `VersionPrefix` in .csproj.


<br/>


### Continuous integration
* CI happens in [Github Actions](https://github.com/features/actions).

* Github Actions configuration is maintained in an [github_ci.yml](../library-ci/github_ci.yml) file in `vostok.devtools` module and in `vostok.github.ci` module.

* Builds and tests are run in a variety of conditions:
	* On two different operating systems: Windows and Ubuntu
	* With two different dependency resolution mechanisms:
		* From latest sources in Cement.
		* From latest versions of NuGet packages.
	* Modules can override some parameters (i.e. Windows specific modules won't try to launch tests on Ubuntu).

* Tests are executed using all available runtimes:
	* .NET, .NET Core and .NET Framework on Windows
	* .NET, .NET Core on Ubuntu

* If the build process fails or some tests turn red in any of these configurations, the run is considered failed and no packages can be published.

* When publishing a *release* version of NuGet package, all cement dependencies are substituted with references to latest *stable* versions of corresponding NuGet packages before build and testing.

* When publishing a *prerelease* version of NuGet package, all cement dependencies are substituted with references to latest versions (*including unstable*) of corresponding NuGet packages before build and testing.

* A notification to the user which triggered the process is sent in case of failure of CI.

<br/>


### Change log
* 19.07.2022: removed appveyor mentions, added github actions description, added Main.Props description
* 07.11.2018: added a suggestion about xml-docs for ctors
* 10.09.2018: take package release notes from CHANGELOG.md file
* 05.09.2018: added a reminder about .ConfigureAwait(false)
* 15.08.2018: documented additional requirement to Cement module install section
* 14.08.2018: forbade force push
* 13.08.2018: added first conventions about tests naming and placement
* 13.08.2018: replaced links to sample project with launchpad template reference.
* 09.08.2018: added a rule about placing linked sources into `Commons\` folder
* 03.08.2018: mentioned source-only libraries.
* 03.08.2018: added first xml-doc conventions: about list items formatting and the use of em-dash
* 03.08.2018: JetBrainsAnnotations.cs should now be imported as a link.
* 29.07.2018: added PackageIconUrl requirement.
* 29.07.2018: main project internals should only be visible to corresponding unit test projects.
* 25.07.2018: unit test projects should not target full framework when building on Linux.
