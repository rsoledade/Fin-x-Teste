# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY Finx.Solution.sln ./
COPY Finx.Api/Finx.Api.csproj Finx.Api/
COPY Finx.Domain/Finx.Domain.csproj Finx.Domain/
COPY Finx.Infrastructure/Finx.Infrastructure.csproj Finx.Infrastructure/
COPY Finx.Integrations/Finx.Integrations.csproj Finx.Integrations/
COPY Finx.Api.Tests/Finx.Api.Tests.csproj Finx.Api.Tests/

RUN dotnet restore Finx.Solution.sln

# Copy everything else and build
COPY . .
WORKDIR /src/Finx.Api
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Finx.Api.dll"]
