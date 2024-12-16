## How to build a Vostok library.

This page describes the prerequisites and procedure for building Vostok libraries.

### Prerequisites

- Download and install .NET Core SDK 2.1+ and .NET Framework 4.7.1+ dev pack from [here](https://dotnet.microsoft.com/download).
- Download and install [Cement](https://github.com/skbkontur/cement) — a source-based dependency manager.
- Select a local directory for Vostok libraries and run a `cm init` command there once to make it Cement-tracked.

### Fetch Cement module (once)

- Find out Cement module name for the library in question. All Vostok modules are listed in [this file](https://github.com/vostok/cement-modules/blob/master/modules). Their names generally coincide with full repository names.
- Execute a `cm get {module}` command from Cement-tracked directory. It will fetch the module itself and all of its dependencies.

### Update and build dependencies

- Move to module directory: `cd {module}`
- Update deps: `cm update-deps`
- Build deps: `cm build-deps`

### Build the module itself

- `cm build`
