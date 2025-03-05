# Use the official Windows Server Core image with .NET Framework 4.7.2 SDK (for building)
FROM mcr.microsoft.com/dotnet/framework/sdk:4.7.2-windowsservercore-ltsc2019 AS build

# Set the working directory for the build
WORKDIR /app

# Copy the project files (source code, .csproj, etc.)
COPY . ./

# Restore NuGet packages and publish the app
RUN nuget restore
RUN dotnet publish -c Release -o /app/publish

# Use the runtime image for the final container
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019

# Set the working directory for the runtime
WORKDIR /inetpub/wwwroot

# Copy the published files from the build stage
COPY --from=build /app/publish/ .

# Expose port 80 for IIS
EXPOSE 80