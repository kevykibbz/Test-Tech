# Use the official .NET 9 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set working directory
WORKDIR /src

# Copy solution file
COPY LawVuTechTest.sln ./

# Copy project files
COPY WebApi/WebApi.csproj WebApi/
COPY AppLogic/AppLogic.csproj AppLogic/
COPY DataAccess/DataAccess.csproj DataAccess/
COPY ServiceModel/ServiceModel.csproj ServiceModel/

# Restore dependencies
RUN dotnet restore WebApi/WebApi.csproj

# Copy source code
COPY . .

# Build the application
WORKDIR /src/WebApi
RUN dotnet build -c Release -o /app/build

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET 9 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set working directory
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Expose port 80 and 443
EXPOSE 80
EXPOSE 443

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Start the application
ENTRYPOINT ["dotnet", "WebApi.dll"]
