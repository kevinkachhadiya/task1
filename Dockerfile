# Use the .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:6.0

# Set working directory
WORKDIR /app

# Copy all files to the container
COPY . ./

# Restore dependencies
RUN dotnet restore

# Publish the app
RUN dotnet publish -c Release -o /app/publish