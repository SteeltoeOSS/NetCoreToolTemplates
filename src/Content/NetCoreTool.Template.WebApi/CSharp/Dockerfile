# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
#if (IsFrameworkNet60)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#elif (IsFrameworkNet80)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#elif (IsFrameworkNet90)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#endif
#if (IsFrameworkNet80 || IsFrameworkNet90)
USER $APP_UID
#endif
WORKDIR /app
#if (IsFrameworkNet60)
EXPOSE 80
EXPOSE 443
#else
EXPOSE 8080
EXPOSE 8081
#endif


# This stage is used to build the service project
#if (IsFrameworkNet60)
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#endif
#if (IsFrameworkNet80)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#endif
#if (IsFrameworkNet90)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#endif
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Company.WebApplication.CS.csproj", "."]
RUN dotnet restore "./Company.WebApplication.CS.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Company.WebApplication.CS.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Company.WebApplication.CS.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.WebApplication.CS.dll"]
