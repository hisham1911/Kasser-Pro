# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["KasserPro/KasserPro/KasserPro.csproj", "KasserPro/KasserPro/"]
RUN dotnet restore "KasserPro/KasserPro/KasserPro.csproj"
COPY . .
WORKDIR "/src/KasserPro/KasserPro"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Railway uses PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

EXPOSE 8080
ENTRYPOINT ["dotnet", "KasserPro.dll"]
