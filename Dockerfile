# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY KasserPro/KasserPro.sln ./KasserPro/
COPY KasserPro/KasserPro/KasserPro.csproj ./KasserPro/KasserPro/

# Restore dependencies
WORKDIR /src/KasserPro
RUN dotnet restore

# Copy everything and build
WORKDIR /src
COPY KasserPro/ ./KasserPro/
WORKDIR /src/KasserPro/KasserPro
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Railway uses PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

EXPOSE 8080
ENTRYPOINT ["dotnet", "KasserPro.dll"]
