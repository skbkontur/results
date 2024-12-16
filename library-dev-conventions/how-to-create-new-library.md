## How to create a new Vostok library

This is a step-by-step guide describing the process of bootstrapping a brand new Vostok library.

* If not already, read the development conventions for [ordinary libraries](conventions.md) and [source libraries](src-libs-conventions.md). 
Most of the requirements described in there are automated by [Launchpad](../launchpad) templates, but it's essential to know what's going on.
 
* Create a new repository in [Vostok organization](https://github.com/vostok) on GitHub. 
Note that repository name should not start with `vostok` prefix (it comes from organization name).

* Create an empty `master` branch in new repository on github

* Select a name for Cement module. It should be in lowercase and start with a common `vostok` prefix (like `vostok.logging.abstractions`).

* Create a Cement module with following command: `cm module add <module-name> <https-repo-url> --pushurl=<ssh-repo-url> --package=vostok`. Here `<https-repo-url>` refers to a repository URL like https://github.com/vostok/devtools.git and `<ssh-repo-url>` is like git@github.com:vostok/devtools.git

* Fetch the Cement module: `cm get <module-name>`

* Move to the module directory: `cd <module-name>`

* Install or update [Launchpad](../launchpad) to latest version.

* Bootstrap repository contents with one of the following launchpad templates:
  * `launchpad new vostok-library` for ordinary libraries
  * `launchpad new vostok-source-library` for source libraries
  * You will be prompted for multiple variables. Follow the provided descriptions to choose correct values.
  
* Manually fix `.github/workflows/ci.yml` file: last line should be `token: ${{ secrets.NUGET_TOKEN }}` (or [fix](https://github.com/vostok/devtools/blob/master/launchpad-templates/library-source/template/.github/workflows/ci.yml#L17) launchpad temlate, but I don't know how to escape it)
    
* Update cement dependencies: `cm update-deps <module-name>`

* Ensure that created project builds with a `dotnet build` command

* Setup CI on GitHub:
  * Copy and workflow from [hosting](https://github.com/vostok/hosting/blob/master/.github/workflows/ci.yml) module
  * Check and fill testing frameworks & platforms

* Push repository to `master` branch.
