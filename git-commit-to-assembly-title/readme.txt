An msbuild task that dynamically creates an auxiliary file with following assembly attributes during build process:

1. AssemblyTitle with current git commit hash, author, branch name, commit and build dates.

2. AssemblyInformationalVersion with a short commit hash suffix appended to current version.


To install it into any project, just include the .props file directly into .csproj:

<Import Project="<path to props file relative to target csproj>" />

It may look like this in a project that resides in a subdirectory of its cement module:

<Import Project="..\..\vostok.devtools\git-commit-to-assembly-title\Vostok.Tools.GitCommit2AssemblyTitle.props" />

Then include AssemblyTitle.cs to module's .gitignore file.