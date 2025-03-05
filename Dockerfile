# Use the official Windows Server Core image with .NET Framework 4.7.2 and ASP.NET
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019

# Set the working directory to the IIS default web root
WORKDIR /inetpub/wwwroot

# Copy the published ASP.NET MVC application files
COPY bin/Release/net472/ .

# Expose port 80 for IIS (default HTTP port)
EXPOSE 80

# Healthcheck using a single-line PowerShell command
HEALTHCHECK CMD powershell -command "try { $response = Invoke-WebRequest http://localhost -UseBasicParsing; if ($response.StatusCode -eq 200) { exit 0 } else { exit 1 } } catch { exit 1 }"