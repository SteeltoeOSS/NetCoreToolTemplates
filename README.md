# Steeltoe.NetCoreTool.Templates

[![Build Status](https://github.com/SteeltoeOSS/NetCoreToolTemplates/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/SteeltoeOSS/NetCoreToolTemplates/actions/workflows/build.yml?query=branch%3Amain)
&nbsp;[![NuGet Version](https://img.shields.io/nuget/v/Steeltoe.NetCoreTool.Templates?style=flat)](https://www.nuget.org/packages/Steeltoe.NetCoreTool.Templates)
&nbsp;[![GitHub License](https://img.shields.io/github/license/SteeltoeOSS/NetCoreToolTemplates)](LICENSE)

This repository contains a collection of [.NET Template Packages](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new),
which can be used from IDEs such as Visual Studio and from the .NET CLI using `dotnet new`. They are also used by [Initializr](https://start.steeltoe.io/).

Project templates:
- `steeltoe-webapi`: Add Steeltoe components to the default `webapi` C# template.

This document describes template installation and template general usage help.
For learning how to develop additional options for the templates, see [DEVELOPER.md](DEVELOPER.md).

## Install

### From NuGet.org (production)

```
$ dotnet new install Steeltoe.NetCoreTool.Templates
```

### From .NET Foundation (staging)

```
$ dotnet nuget add source https://pkgs.dev.azure.com/dotnet/Steeltoe/_packaging/ci/nuget/v3/index.json -n Steeltoe-ci
$ dotnet new install Steeltoe.NetCoreTool.Templates
```

### From Source (development)

```
$ dotnet new install src/Content
```

## Tips when making changes
- When searching for occurrences of options, Visual Studio doesn't show everything by default. Change the **File types** from `!*\bin\*;!*\obj\*;!*\.*\*` to `!*\bin\*;!*\obj\*` to fix this.
- If you've added a new option to `template.json`, `dotnetcli.host.json` and `ide.host.json`, but it doesn't appear, make sure it is listed in one of the `HasAny*` computed options in `template.json`.
- To observe changes in Visual Studio's **File** > **New** > **Project** dialog, close all instances and run `devenv.exe /updateconfiguration`.
- Unit tests are available for all options (CLI only), be sure to update them.
- To verify all relevant options are exposed, temporarily add `"defaultSymbolVisibility": true` to `ide.host.json`.
- Documentation links
  - https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates
  - https://github.com/dotnet/templating/wiki
  - https://github.com/dotnet/templating?tab=readme-ov-file#template-content-repositories
  - https://github.com/sayedihashimi/template-sample
