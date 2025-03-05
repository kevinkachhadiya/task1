# Use the official Windows Server Core image with .NET Framework 4.7.2
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019

# Set the working directory inside the container
WORKDIR /inetpub/wwwroot

# Copy the published app to the container
COPY bin/Release/net472/ .

# Expose the port your app runs on (default IIS port is 80)
EXPOSE 80

# Start IIS (handled by the base image automatically)
# No additional ENTRYPOINT needed for ASP.NET Framework with IIS