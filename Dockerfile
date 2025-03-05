# Use the official Windows Server Core image with .NET Framework 4.7.2 and ASP.NET
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019

# Set the working directory to the IIS default web root
WORKDIR /inetpub/wwwroot

# Copy the published ASP.NET MVC application files
# Note: Ensure your local path matches your build output
COPY bin/Release/net472/ .

# Expose port 80 for IIS (default HTTP port)
EXPOSE 80

# Optional: Ensure IIS is properly configured (usually handled by base image)
# Healthcheck is optional but recommended for production
HEALTHCHECK CMD powershell -command `
    try { `
     $response = iwr http://localhost -UseBasicParsing; `
     if ($response.StatusCode -eq 200) { return 0} `
     else {return 1}; `
    } catch { return 1 }

ENV ASPNETCORE_ENVIRONMENT=Production