# -------- BUILD STAGE --------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

WORKDIR /src/CSharpApp.Api
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


# -------- RUNTIME STAGE --------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Docker healthcheck
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD curl --fail http://localhost:8080/health/ready || exit 1

ENTRYPOINT ["dotnet", "CSharpApp.Api.dll"]
