Installation for .NET Core 2.1+:
 - Windows: `install` 
 - Non-windows: `./install.sh`

Usage:

 1. Update `CHANGELOG.md`: describe the changes you've made in the current release (see VersionPrefix in .csproj)
 2. Run `vostok-release` in solution directory. It will do several things:
    * new tag with current release version will be created and pushed,
    * VersionPrefix in csproj will be incremented, Unshipped file rolled to Shipped, commited and pushed,
    * new package will be built and pushed to NuGet from GitHub Actions CI
