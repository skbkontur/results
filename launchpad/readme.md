## Launchpad

Launchpad is a .NET Core CLI tool used to bootstrap repositories from project templates. Templates are NuGet packages with content files that may contain variable placeholders. Read more about template structure [here](../launchpad-templates/readme.md).

### Installation

* Install .NET Core 2.1+ runtime [here](https://www.microsoft.com/net/download)
* Execute this command: `dotnet tool install -g Vostok.Launchpad`

### Update

To update launchpad to the latest version, use this command: `dotnet tool update -g Vostok.Launchpad`

### Usage

Launch the tool without parameters (`launchpad`) to get help and find out about all available commands.

### Compatibility with zsh

zsh users need to add following line to ~/.zshrc (see https://github.com/dotnet/cli/issues/9321#issuecomment-390720940):

export PATH="$HOME/.dotnet/tools:$PATH"
