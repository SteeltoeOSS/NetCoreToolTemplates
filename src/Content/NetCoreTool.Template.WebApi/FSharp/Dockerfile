FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["Company.WebApplication.FS.fsproj", "."]
RUN dotnet restore "Company.WebApplication.FS.fsproj"
COPY . .
RUN dotnet build "Company.WebApplication.FS.fsproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Company.WebApplication.FS.fsproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV DOTNET_URLS http://0.0.0.0:80
ENTRYPOINT ["dotnet", "Company.WebApplication.FS.dll"]
