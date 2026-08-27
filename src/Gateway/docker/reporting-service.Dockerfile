# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY NuGet.config .
COPY src/Gateway/docker/restore-with-retry.sh /usr/local/bin/restore-with-retry.sh
RUN chmod +x /usr/local/bin/restore-with-retry.sh

COPY Directory.Build.props .
COPY src/Directory.Build.props src/
COPY src/Shared/Directory.Build.props src/Shared/
COPY src/Shared/Kernel/Fgs.Kernel/Fgs.Kernel.csproj src/Shared/Kernel/Fgs.Kernel/
COPY src/Shared/Contracts/Fgs.Contracts/Fgs.Contracts.csproj src/Shared/Contracts/Fgs.Contracts/
COPY src/Shared/Messaging/Fgs.Messaging/Fgs.Messaging.csproj src/Shared/Messaging/Fgs.Messaging/
COPY src/Shared/Persistence/Fgs.Persistence/Fgs.Persistence.csproj src/Shared/Persistence/Fgs.Persistence/
COPY src/Shared/Security/Fgs.Security/Fgs.Security.csproj src/Shared/Security/Fgs.Security/
COPY src/Shared/MultiTenancy/Fgs.MultiTenancy/Fgs.MultiTenancy.csproj src/Shared/MultiTenancy/Fgs.MultiTenancy/
COPY src/Shared/Foundation/Fgs.Foundation/Fgs.Foundation.csproj src/Shared/Foundation/Fgs.Foundation/
COPY src/Shared/Observability/Fgs.Observability/Fgs.Observability.csproj src/Shared/Observability/Fgs.Observability/
COPY src/Shared/Credentials/Fgs.Credentials/Fgs.Credentials.csproj src/Shared/Credentials/Fgs.Credentials/

COPY src/ReportingService/Fgs.Reporting.API/Fgs.Reporting.API.csproj src/ReportingService/Fgs.Reporting.API/
COPY src/ReportingService/Fgs.Reporting.Application/Fgs.Reporting.Application.csproj src/ReportingService/Fgs.Reporting.Application/
COPY src/ReportingService/Fgs.Reporting.Domain/Fgs.Reporting.Domain.csproj src/ReportingService/Fgs.Reporting.Domain/
COPY src/ReportingService/Fgs.Reporting.Infrastructure/Fgs.Reporting.Infrastructure.csproj src/ReportingService/Fgs.Reporting.Infrastructure/

RUN --mount=type=cache,target=/root/.nuget/packages,sharing=locked \
    /usr/local/bin/restore-with-retry.sh src/ReportingService/Fgs.Reporting.API/Fgs.Reporting.API.csproj

COPY src/Shared/ src/Shared/
COPY src/ReportingService/ src/ReportingService/

WORKDIR /src/src/ReportingService/Fgs.Reporting.API
RUN --mount=type=cache,target=/root/.nuget/packages,sharing=locked \
    dotnet publish Fgs.Reporting.API.csproj -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
RUN apk add --no-cache curl ca-certificates && update-ca-certificates
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5013 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_EnableDiagnostics=0

EXPOSE 5013

HEALTHCHECK --interval=30s --timeout=5s --start-period=90s --retries=5 \
    CMD curl -fsS http://localhost:5013/health || exit 1

ENTRYPOINT ["dotnet", "Fgs.Reporting.API.dll"]