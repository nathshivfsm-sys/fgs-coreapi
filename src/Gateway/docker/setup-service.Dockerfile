FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine
RUN apk add --no-cache curl
WORKDIR /src

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
COPY src/AuditService/Fgs.Audit.Domain/Fgs.Audit.Domain.csproj src/AuditService/Fgs.Audit.Domain/
COPY src/AuditService/Fgs.Audit.Application/Fgs.Audit.Application.csproj src/AuditService/Fgs.Audit.Application/
COPY src/AuditService/Fgs.Audit.Infrastructure/Fgs.Audit.Infrastructure.csproj src/AuditService/Fgs.Audit.Infrastructure/
COPY src/SetupService/Fgs.Setup.API/Fgs.Setup.API.csproj src/SetupService/Fgs.Setup.API/
COPY src/SetupService/Fgs.Setup.Application/Fgs.Setup.Application.csproj src/SetupService/Fgs.Setup.Application/
COPY src/SetupService/Fgs.Setup.Domain/Fgs.Setup.Domain.csproj src/SetupService/Fgs.Setup.Domain/
COPY src/SetupService/Fgs.Setup.Infrastructure/Fgs.Setup.Infrastructure.csproj src/SetupService/Fgs.Setup.Infrastructure/

RUN dotnet restore src/SetupService/Fgs.Setup.API/Fgs.Setup.API.csproj

COPY src/Shared/ src/Shared/
COPY src/AuditService/ src/AuditService/
COPY src/SetupService/ src/SetupService/

WORKDIR /src/src/SetupService/Fgs.Setup.API
RUN dotnet build Fgs.Setup.API.csproj -c Release --no-restore

ENV ASPNETCORE_URLS=http://+:5004 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5004

HEALTHCHECK --interval=30s --timeout=5s --start-period=90s --retries=5 \
    CMD curl -fsS http://localhost:5004/health || exit 1

ENTRYPOINT ["dotnet", "run", "--no-build", "--no-launch-profile", "--project", "Fgs.Setup.API.csproj", "--configuration", "Release", "--urls", "http://+:5004"]
