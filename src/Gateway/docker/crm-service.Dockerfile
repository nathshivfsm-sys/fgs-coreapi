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

COPY src/CrmService/Fgs.Crm.API/Fgs.Crm.API.csproj src/CrmService/Fgs.Crm.API/
COPY src/CrmService/Fgs.Crm.Application/Fgs.Crm.Application.csproj src/CrmService/Fgs.Crm.Application/
COPY src/CrmService/Fgs.Crm.Domain/Fgs.Crm.Domain.csproj src/CrmService/Fgs.Crm.Domain/
COPY src/CrmService/Fgs.Crm.Infrastructure/Fgs.Crm.Infrastructure.csproj src/CrmService/Fgs.Crm.Infrastructure/

RUN --mount=type=cache,target=/root/.nuget/packages \
    /usr/local/bin/restore-with-retry.sh src/CrmService/Fgs.Crm.API/Fgs.Crm.API.csproj

COPY src/Shared/ src/Shared/
COPY src/CrmService/ src/CrmService/

WORKDIR /src/src/CrmService/Fgs.Crm.API
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish Fgs.Crm.API.csproj -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
RUN apk add --no-cache curl ca-certificates && update-ca-certificates
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5009 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_EnableDiagnostics=0

EXPOSE 5009

HEALTHCHECK --interval=30s --timeout=5s --start-period=90s --retries=5 \
    CMD curl -fsS http://localhost:5009/health || exit 1

ENTRYPOINT ["dotnet", "Fgs.Crm.API.dll"]