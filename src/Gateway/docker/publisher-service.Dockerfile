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
COPY src/Shared/Credentials/Fgs.Credentials/Fgs.Credentials.csproj src/Shared/Credentials/Fgs.Credentials/
COPY src/PublisherService/Fgs.Publisher.API/Fgs.Publisher.API.csproj src/PublisherService/Fgs.Publisher.API/
COPY src/PublisherService/Fgs.Publisher.Application/Fgs.Publisher.Application.csproj src/PublisherService/Fgs.Publisher.Application/
COPY src/PublisherService/Fgs.Publisher.Domain/Fgs.Publisher.Domain.csproj src/PublisherService/Fgs.Publisher.Domain/
COPY src/PublisherService/Fgs.Publisher.Infrastructure/Fgs.Publisher.Infrastructure.csproj src/PublisherService/Fgs.Publisher.Infrastructure/

RUN dotnet restore src/PublisherService/Fgs.Publisher.API/Fgs.Publisher.API.csproj

COPY src/Shared/ src/Shared/
COPY src/PublisherService/ src/PublisherService/

WORKDIR /src/src/PublisherService/Fgs.Publisher.API
RUN dotnet build Fgs.Publisher.API.csproj -c Release --no-restore

ENV ASPNETCORE_URLS=http://+:5006 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5006

HEALTHCHECK --interval=30s --timeout=5s --start-period=90s --retries=5 \
    CMD curl -fsS http://localhost:5006/health || exit 1

ENTRYPOINT ["dotnet", "run", "--no-build", "--no-launch-profile", "--project", "Fgs.Publisher.API.csproj", "--configuration", "Release", "--urls", "http://+:5006"]
