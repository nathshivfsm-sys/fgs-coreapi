FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine
RUN apk add --no-cache curl
WORKDIR /src

COPY Directory.Build.props .
COPY src/Directory.Build.props src/
COPY src/Shared/Directory.Build.props src/Shared/
COPY src/Shared/Kernel/Fgs.Kernel/Fgs.Kernel.csproj src/Shared/Kernel/Fgs.Kernel/
COPY src/Shared/Contracts/Fgs.Contracts/Fgs.Contracts.csproj src/Shared/Contracts/Fgs.Contracts/
COPY src/Shared/Messaging/Fgs.Messaging/Fgs.Messaging.csproj src/Shared/Messaging/Fgs.Messaging/
COPY src/Shared/Security/Fgs.Security/Fgs.Security.csproj src/Shared/Security/Fgs.Security/
COPY src/Shared/MultiTenancy/Fgs.MultiTenancy/Fgs.MultiTenancy.csproj src/Shared/MultiTenancy/Fgs.MultiTenancy/
COPY src/Shared/Foundation/Fgs.Foundation/Fgs.Foundation.csproj src/Shared/Foundation/Fgs.Foundation/
COPY src/Shared/Observability/Fgs.Observability/Fgs.Observability.csproj src/Shared/Observability/Fgs.Observability/
COPY src/Shared/Credentials/Fgs.Credentials/Fgs.Credentials.csproj src/Shared/Credentials/Fgs.Credentials/
COPY src/NotificationService/Fgs.Notification.API/Fgs.Notification.API.csproj src/NotificationService/Fgs.Notification.API/
COPY src/NotificationService/Fgs.Notification.Application/Fgs.Notification.Application.csproj src/NotificationService/Fgs.Notification.Application/
COPY src/NotificationService/Fgs.Notification.Domain/Fgs.Notification.Domain.csproj src/NotificationService/Fgs.Notification.Domain/
COPY src/NotificationService/Fgs.Notification.Infrastructure/Fgs.Notification.Infrastructure.csproj src/NotificationService/Fgs.Notification.Infrastructure/

RUN dotnet restore src/NotificationService/Fgs.Notification.API/Fgs.Notification.API.csproj

COPY src/Shared/ src/Shared/
COPY src/NotificationService/ src/NotificationService/

WORKDIR /src/src/NotificationService/Fgs.Notification.API
RUN dotnet build Fgs.Notification.API.csproj -c Release --no-restore

ENV ASPNETCORE_URLS=http://+:5002 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5002

HEALTHCHECK --interval=30s --timeout=5s --start-period=30s --retries=3 \
    CMD curl -fsS http://localhost:5002/health || exit 1

ENTRYPOINT ["dotnet", "run", "--no-build", "--no-launch-profile", "--project", "Fgs.Notification.API.csproj", "--configuration", "Release", "--urls", "http://+:5002"]
