# Build stage: Use the .NET Framework 4.7.2 SDK image (Windows-based)
FROM mcr.microsoft.com/dotnet/framework/sdk:4.7.2-windowsservercore-ltsc2019 AS build

WORKDIR /app

# Copy project files and restore dependencies
COPY . ./
RUN nuget restore
RUN msbuild task1.csproj /p:Configuration=Release /p:OutputPath=/app/publish

# Runtime stage: Use the ASP.NET 4.7.2 image (Windows-based)
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019

WORKDIR /inetpub/wwwroot

# Copy published files from the build stage
COPY --from=build /app/publish/ .

EXPOSE 80