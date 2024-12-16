## How to release a Vostok library

This is a step-by-step guide describing the process of releasing a new version of Vostok library.

1. Select version number for the new release according to [SemVer](https://semver.org/spec/v1.0.0.html) conventions. Remember to bump major version when making changes that break backward compatibility.

2. Ensure that `VersionPrefix` tag in library's .csproj contains selected version number.

3. Document changes about to be released in `CHANGELOG.md` file.

4. Commit your changes to repository's `master` branch.

5. While staying on `master` branch, run [vostok-release](https://github.com/vostok/devtools/tree/master/vostok-release) tool.

6. This tool pushes the tag which starts a special build on GitHub Actions CI. Let it finish and ensure there were no failures. Check that a NuGet package with proper version and dependencies was published to [nuget.org](https://nuget.org).

<br/>

### What if something goes wrong with GitHub Actions CI build?

If the failure is transient or not caused by the library itself, you can restart the build by pressing retry button.

Otherwise make fix and release it again.
