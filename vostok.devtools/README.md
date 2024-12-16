# Vostok devtools

This module contains various tools, helpers and guidelines serving to ease/automate the development of Vostok .NET libraries:

* [Library development conventions](library-dev-conventions/conventions.md) to govern dev and CI processes (also for [source-only libs](library-dev-conventions/src-libs-conventions.md)).
* [Checklist](library-dev-conventions/how-to-create-new-library.md) for developers creating new libraries.
* [Checklist](library-dev-conventions/how-to-build-a-library.md) for developers building existing libraries.
* [Checklist](library-dev-conventions/how-to-release-a-library.md) for developers releasing new library versions.
* [Checklist](library-dev-conventions/http-client-dev-conventions.md) for developers creating clients for HTTP microservices.
* [Launchpad](launchpad) tool to ease bootstrapping of new repositories.
* [Launchpad template](launchpad-templates/library-ordinary) for Vostok ordinary libraries
* [Launchpad template](launchpad-templates/library-source) for Vostok source libraries
* [C# code style](code-style-csharp) to keep the code clean and homogeneous.
* [GitCommit2AssemblyTitle](git-commit-to-assembly-title) build target to enrich assemblies with Git info.
* [dotnetcementrefs](dotnetcementrefs) CLI tool for .NET Core 2.1 to convert Cement references into NuGet package references.
* [dotnetversionsuffix](dotnetversionsuffix) CLI tool for .NET Core 2.1 to patch project versions during CI.
* [configure-await-false](configure-await-false) CLI tool for .NET Core 2.1 to verify that awaits in library code are configured.
* [publicize.roslyn](publicize.roslyn) build target to publicize types in source libraries.
* [vostok-release](vostok-release) tool to release Vostok library updates.
