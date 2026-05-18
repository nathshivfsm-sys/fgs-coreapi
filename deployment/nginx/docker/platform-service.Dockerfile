FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY src/PlatformService/Fgs.Platform.API/Fgs.Platform.API.csproj src/PlatformService/Fgs.Platform.API/
COPY src/PlatformService/Fgs.Platform.Application/Fgs.Platform.Application.csproj src/PlatformService/Fgs.Platform.Application/
COPY src/PlatformService/Fgs.Platform.Domain/Fgs.Platform.Domain.csproj src/PlatformService/Fgs.Platform.Domain/
COPY src/PlatformService/Fgs.Platform.Infrastructure/Fgs.Platform.Infrastructure.csproj src/PlatformService/Fgs.Platform.Infrastructure/

RUN dotnet restore src/PlatformService/Fgs.Platform.API/Fgs.Platform.API.csproj

COPY src/PlatformService/ src/PlatformService/
WORKDIR /src/src/PlatformService/Fgs.Platform.API
RUN dotnet publish Fgs.Platform.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
RUN apk add --no-cache curl
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5002 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5002

COPY --from=build /app/publish .

HEALTHCHECK --interval=30s --timeout=5s --start-period=30s --retries=3 \
    CMD curl -fsS http://localhost:5002/health || exit 1

ENTRYPOINT ["dotnet", "Fgs.Platform.API.dll"]
