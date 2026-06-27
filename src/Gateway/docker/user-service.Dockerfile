# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine
RUN apk add --no-cache curl ca-certificates && update-ca-certificates
WORKDIR /src

COPY NuGet.config .
COPY src/Gateway/docker/restore-with-retry.sh /usr/local/bin/restore-with-retry.sh
RUN chmod +x /usr/local/bin/restore-with-retry.sh

COPY Directory.Build.props .
COPY src/Directory.Build.props src/
COPY src/Shared/Directory.Build.props src/Shared/
COPY src/Shared/Kernel/Fgs.Kernel/Fgs.Kernel.csproj src/Shared/Kernel/Fgs.Kernel/
COPY src/Shared/Contracts/Fgs.Contracts/Fgs.Contracts.csproj src/Shared/Contracts/Fgs.Contracts/
COPY src/Shared/Persistence/Fgs.Persistence/Fgs.Persistence.csproj src/Shared/Persistence/Fgs.Persistence/
COPY src/Shared/Messaging/Fgs.Messaging/Fgs.Messaging.csproj src/Shared/Messaging/Fgs.Messaging/
COPY src/Shared/Security/Fgs.Security/Fgs.Security.csproj src/Shared/Security/Fgs.Security/
COPY src/Shared/MultiTenancy/Fgs.MultiTenancy/Fgs.MultiTenancy.csproj src/Shared/MultiTenancy/Fgs.MultiTenancy/
COPY src/Shared/Foundation/Fgs.Foundation/Fgs.Foundation.csproj src/Shared/Foundation/Fgs.Foundation/
COPY src/Shared/Observability/Fgs.Observability/Fgs.Observability.csproj src/Shared/Observability/Fgs.Observability/
COPY src/Shared/Credentials/Fgs.Credentials/Fgs.Credentials.csproj src/Shared/Credentials/Fgs.Credentials/
COPY src/UserService/Fgs.User.API/Fgs.User.API.csproj src/UserService/Fgs.User.API/
COPY src/UserService/Fgs.User.Application/Fgs.User.Application.csproj src/UserService/Fgs.User.Application/
COPY src/UserService/Fgs.User.Domain/Fgs.User.Domain.csproj src/UserService/Fgs.User.Domain/
COPY src/UserService/Fgs.User.Infrastructure/Fgs.User.Infrastructure.csproj src/UserService/Fgs.User.Infrastructure/

RUN --mount=type=cache,target=/root/.nuget/packages \
    /usr/local/bin/restore-with-retry.sh src/UserService/Fgs.User.API/Fgs.User.API.csproj

COPY src/Shared/ src/Shared/
COPY src/UserService/ src/UserService/

WORKDIR /src/src/UserService/Fgs.User.API
RUN --mount=type=cache,target=/root/.nuget/packages \
    /usr/local/bin/restore-with-retry.sh Fgs.User.API.csproj && \
    dotnet build Fgs.User.API.csproj -c Release --no-restore

ENV ASPNETCORE_URLS=http://+:5001 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5001

HEALTHCHECK --interval=30s --timeout=5s --start-period=90s --retries=5 \
    CMD curl -fsS http://localhost:5001/health || exit 1

ENTRYPOINT ["dotnet", "run", "--no-build", "--no-launch-profile", "--project", "Fgs.User.API.csproj", "--configuration", "Release", "--urls", "http://+:5001"]
