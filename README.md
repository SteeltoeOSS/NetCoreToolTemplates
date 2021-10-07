# Steeltoe NetCoreToolTemplates

[![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Initializr/SteeltoeOSS.NetCoreToolTemplates?branchName=main)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=46&branchName=main)

Steeltoe NetCoreToolTemplates is a collection of .NET Core Tool templates.

Project templates:
<dl>
  <dt>steeltoe-webapi</dt>
  <dd>Creates a Steeltoe-influenced .NET Web API project</dd>
</dl>

This document describes template installation and template general usage help.
For learning how to develop additional options for the templates, see [DEVELOPER.md](DEVELOPER.md).

## Install

### From NuGet.org (production)

```
$ dotnet new --install Steeltoe.NetCoreTool.Templates
```

### From .NET Foundation (staging)

```
$ dotnet nuget add source https://pkgs.dev.azure.com/dotnet/Steeltoe/_packaging/dev/nuget/v3/index.json -n SteeltoeDev
$ dotnet new --install Steeltoe.NetCoreTool.Templates
```

### From Source (development)

```
$ dotnet new --install src/Content
```

Note: To see templates in Visual Studio, you may need to enable:

_Tools->Options..._ _Preview Features:_ _Show all .NET Core templates in the NEW project dialog_
