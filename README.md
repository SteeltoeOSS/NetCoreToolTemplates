# Steeltoe InitializrDotNetNew

Steeltoe InitializrDotNetNew are project templates to be used for creating projects using .NET Core's `dotnet new`
command.

Project templates:
<dl>
  <dt>stwebapi</dt>
  <dd>Creates a Steeltoe-influenced .NET Web API project</dd>
</dl>

## Install

### From Source

```
$ dotnet build
$ dotnet new --install src/DotNetNew.WebApi
```

### Uninstall

```
$ dotnet new --uninstall
# follow instructions to uninstall "Steeltoe WebApi (stwebapi) C#' template
```


## `stwebapi`

### About

Creates a Steeltoe-influenced .NET Web API project.

### Usage

```
$ dotnet new stwebapi [options]
```

### Options

```
  --docker        Add Docker support.
                  bool - Optional
                  Default: false

  -f|--framework  The target framework for the project.
                      net5.0
                      netcoreapp3.1
                      netcoreapp2.1
                  Default: net5.0

  --no-restore    If specified, skips the automatic restore of the project on create.
                  bool - Optional
                  Default: false

  -s|--steeltoe   The Steeltoe version.
                      3.0.2
                      2.5.3
                  Default: 3.0.2
```


