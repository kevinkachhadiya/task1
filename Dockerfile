
# Use the official Windows Server Core image with .NET Framework 4.7.2
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019

# Set the working directory inside the container
WORKDIR /inetpub/wwwroot

# Copy the published app files
COPY bin/Release/net472/ .

# Expose port 80 (default for IIS)
EXPOSE 80
