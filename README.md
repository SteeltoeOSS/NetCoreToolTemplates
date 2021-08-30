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
$ dotnet pack src/Steeltoe.NetCoreTool.Templates.csproj
$ dotnet new --install Steeltoe.NetCoreTool.Templates.*.nupkg
```

Note: To see templates in Visual Studio, you may need to enable:

_Tools->Options..._ _Preview Features:_ _Show all .NET Core templates in the NEW project dialog_

### Uninstall

```
$ dotnet new --uninstall Steeltoe.NetCoreTool.Templates
```


## `steeltoe-webapi`

### About

Creates a Steeltoe-influenced .NET Web API project.

### Usage

```
$ dotnet new steeltoe-webapi [options]
```

### Options

```
  --circuit-breaker-hystrix      Add support for Netflix Hystrix, a latency and fault tolerance library.
                                 bool - Optional
                                 Default: false

  --configuration-cloud-config   Add a Spring Cloud Config configuration source.
                                 bool - Optional
                                 Default: false

  --configuration-placeholder    Add a placeholder configuration source.
                                 bool - Optional
                                 Default: false

  --configuration-random-value   Add a random value configuration source.
                                 bool - Optional
                                 Default: false

  --connector-mongodb            Add a connector for MongoDB databases.
                                 bool - Optional
                                 Default: false

  --connector-mysql-efcore       Add a connector for MySQL databases using Entity Framework Core.
                                 bool - Optional
                                 Default: false

  --connector-mysql              Add a connector for MySQL databases.
                                 bool - Optional
                                 Default: false

  --connector-oauth              Add a connector for OAuth security.
                                 bool - Optional
                                 Default: false

  --connector-postgresql-efcore  Add a connector for PostgreSQL databases using Entity Framework Core.
                                 bool - Optional
                                 Default: false

  --connector-postgresql         Add a connector for PostgreSQL databases.

### From Source

```
$ nuget pack src/Steeltoe.NetCoreTool.Templates.nuspec -NoDefaultExcludes
$ dotnet new --install Steeltoe.NetCoreTool.Templates.0.0.1.nupkg
```

Note: To see templates in Visual Studio, you may need to enable:

_Tools->Options..._ _Preview Features:_ _Show all .NET Core templates in the NEW project dialog_

### Uninstall

```
$ dotnet new --uninstall Steeltoe.NetCoreTool.Templates
```


## `steeltoe-webapi`

### About

Creates a Steeltoe-influenced .NET Web API project.

### Usage

```
$ dotnet new steeltoe-webapi [options]
```

### Options

```
  --circuit-breaker-hystrix      Add support for Netflix Hystrix, a latency and fault tolerance library.
                                 bool - Optional
                                 Default: false

  --configuration-cloud-config   Add a Spring Cloud Config configuration source.
                                 bool - Optional
                                 Default: false

  --configuration-placeholder    Add a placeholder configuration source.
                                 bool - Optional
                                 Default: false

  --configuration-random-value   Add a random value configuration source.
                                 bool - Optional
                                 Default: false

  --connector-mongodb            Add a connector for MongoDB databases.
                                 bool - Optional
                                 Default: false

  --connector-mysql-efcore       Add a connector for MySQL databases using Entity Framework Core.
                                 bool - Optional
                                 Default: false

  --connector-mysql              Add a connector for MySQL databases.
                                 bool - Optional
                                 Default: false

  --connector-oauth              Add a connector for OAuth security.
                                 bool - Optional
                                 Default: false

  --connector-postgresql-efcore  Add a connector for PostgreSQL databases using Entity Framework Core.
                                 bool - Optional
                                 Default: false

  --connector-postgresql         Add a connector for PostgreSQL databases.
                                 bool - Optional
                                 Default: false

  --connector-rabbitmq           Add a connector for RabbitMQ message brokers.
                                 bool - Optional
                                 Default: false

  --connector-redis              Add a connector for Redis data stores.
                                 bool - Optional
                                 Default: false

  --connector-sqlserver          Add a connector for Microsoft SQL Server databases.
                                 bool - Optional
                                 Default: false

  -D|--description               Add a project description.
                                 string - Optional

  --discovery-eureka             Add access to Eureka, a REST-based service for locating services.
                                 bool - Optional
                                 Default: false

  --dockerfile                   Add a Dockerfile.
                                 bool - Optional
                                 Default: false

  -f|--framework                 Set the target framework for the project.
                                     net5.0
                                     netcoreapp3.1
                                     netcoreapp2.1
                                 Default: net5.0

  --hosting-azure-spring-cloud   Add hosting support for Microsoft Azure Spring Cloud.
                                 bool - Optional
                                 Default: false

  --hosting-cloud-foundry        Add hosting support for Cloud Foundry.
                                 bool - Optional
                                 Default: false

  --logging-dynamic-logger       Add a dynamic logger.
                                 bool - Optional
                                 Default: false

  --management-endpoints         Add application management endpoints, such as health and metrics.
                                 bool - Optional
                                 Default: false

  --messaging-rabbitmq           Add RabbitMQ messaging support and auto-configuration.
                                 bool - Optional
                                 Default: false

  --no-restore                   Skip the automatic restore of the project on create.
                                 bool - Optional
                                 Default: false

  -s|--steeltoe                  Set the Steeltoe version for the project.
                                 string - Optional
                                 Default: 3.0.*

  --stream-rabbitmq              Add RabbitMQ stream support and auto-configuration.
                                 bool - Optional
                                 Default: false
```
