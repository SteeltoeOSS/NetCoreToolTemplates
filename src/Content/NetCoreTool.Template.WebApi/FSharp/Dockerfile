#if (FrameworkNet60)
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
#endif
#if (FrameworkNet80)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
#endif
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
#if (FrameworkNet60)
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
#endif
#if (FrameworkNet80)
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
#endif
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Company.WebApplication.FS.fsproj", "."]
RUN dotnet restore "Company.WebApplication.FS.fsproj"
COPY . .
RUN dotnet build "Company.WebApplication.FS.fsproj" -c $BUILD_CONFIGURATION -o /app

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Company.WebApplication.FS.fsproj" -c $BUILD_CONFIGURATION -o /app /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Company.WebApplication.FS.dll"]
