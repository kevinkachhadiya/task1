# Build stage: Use the SDK image to compile the app
FROM mcr.microsoft.com/dotnet/framework/sdk:4.7.2-windowsservercore-ltsc2019 AS build

WORKDIR /app

# Copy project files and restore dependencies
COPY . ./
RUN nuget restore
RUN dotnet publish -c Release -o /app/publish

# Runtime stage: Use the ASP.NET image to run the app
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019

WORKDIR /inetpub/wwwroot

# Copy published files from the build stage
COPY --from=build /app/publish/ .

EXPOSE 80