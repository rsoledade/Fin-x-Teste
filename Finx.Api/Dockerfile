# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY ["Finx.Solution.sln", "./"]

# Copy csproj files with correct paths
COPY ["Finx.Api/Finx.Api.csproj", "Finx.Api/"]
COPY ["Finx.Domain/Finx.Domain.csproj", "Finx.Domain/"]
COPY ["Finx.Infrastructure/Finx.Infrastructure.csproj", "Finx.Infrastructure/"]
COPY ["Finx.Integrations/Finx.Integrations.csproj", "Finx.Integrations/"]
COPY ["Finx.Application/Finx.Application.csproj", "Finx.Application/"]
COPY ["Finx.Tests/Finx.Tests.csproj", "Finx.Tests/"]

# Restore dependencies
RUN dotnet restore "Finx.Api/Finx.Api.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/Finx.Api"
RUN dotnet publish "Finx.Api.csproj" -c Release -o /app/publish --no-restore

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 8080

ENTRYPOINT ["dotnet", "Finx.Api.dll"]
