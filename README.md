# Steeltoe InitializrDotNetNew

Steeltoe InitializrDotNetNew are project templates to be used for creating projects using .NET Core's `dotnet new`
command.

Project templates:
<dl>
  <dt>steeltoe-webapi</dt>
  <dd>Creates a Steeltoe-influenced .NET Web API project</dd>
</dl>

This document describes template installation and template general usage help.
For learning how to develop additional options for the templates, see [DEVELOPER.md](DEVELOPER.md).
## Install

### From Source

```
$ dotnet build
$ dotnet new --install src/DotNetNew.WebApi
```

### Uninstall

```
$ dotnet new --uninstall
# follow instructions to uninstall "Steeltoe WebApi (steeltoe-webapi) C#" template
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
  --azure-spring-cloud    Add hosting support for Microsoft Azure Spring Cloud.
                          bool - Optional
                          Default: false

  --cloud-config          Add client support for Spring Cloud Config.
                          bool - Optional
                          Default: false

  --cloud-foundry         Add hosting support for Cloud Foundry.
                          bool - Optional
                          Default: false

  --docker                Add support for Docker.
                          bool - Optional
                          Default: false

  --dynamic-logger        Add a dynamic logger.
                          bool - Optional
                          Default: false

  --eureka                Add access to Eureka, a REST-based service for locating services.
                          bool - Optional
                          Default: false

  -f|--framework          Set the target framework for the project.
                              net5.0
                              netcoreapp3.1
                              netcoreapp2.1
                          Default: net5.0

  --hystrix               Add support for Netflix Hystrix, a latency and fault tolerance library.
                          bool - Optional
                          Default: false

  --management-endpoints  Add application management endpoints, such as health and metrics.
                          bool - Optional
                          Default: false

  --mongodb               Add access to MongoDB databases.
                          bool - Optional
                          Default: false

  --mysql-efcore          Add access to MySQL databases using Entity Framework Core.
                          bool - Optional
                          Default: false

  --mysql                 Add access to MySQL databases.
                          bool - Optional
                          Default: false

  --oauth                 Add access to OAuth security.
                          bool - Optional
                          Default: false

  --placeholder           Add a placeholder configuration source.
                          bool - Optional
                          Default: false

  --postgresql-efcore     Add access to PostgreSQL databases using Entity Framework Core.
                          bool - Optional
                          Default: false

  --postgresql            Add access to PostgreSQL databases.
                          bool - Optional
                          Default: false

  --rabbitmq              Add access to RabbitMQ message brokers.
                          bool - Optional
                          Default: false

  --random-value          Add a random value configuration source.
                          bool - Optional
                          Default: false

  --redis                 Add access to Redis data stores.
                          bool - Optional
                          Default: false

  --no-restore            Skip the automatic restore of the project on create.
                          bool - Optional
                          Default: false

  --sqlserver             Add access to Microsoft SQL Server databases.
                          bool - Optional
                          Default: false

  -s|--steeltoe           Set the Steeltoe version for the project.
                          string - Optional
                          Default: 3.0.*
```


