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
COPY src/ConsumerService/Fgs.Consumer.API/Fgs.Consumer.API.csproj src/ConsumerService/Fgs.Consumer.API/
COPY src/ConsumerService/Fgs.Consumer.Application/Fgs.Consumer.Application.csproj src/ConsumerService/Fgs.Consumer.Application/
COPY src/ConsumerService/Fgs.Consumer.Domain/Fgs.Consumer.Domain.csproj src/ConsumerService/Fgs.Consumer.Domain/
COPY src/ConsumerService/Fgs.Consumer.Infrastructure/Fgs.Consumer.Infrastructure.csproj src/ConsumerService/Fgs.Consumer.Infrastructure/

RUN dotnet restore src/ConsumerService/Fgs.Consumer.API/Fgs.Consumer.API.csproj

COPY src/Shared/ src/Shared/
COPY src/ConsumerService/ src/ConsumerService/

WORKDIR /src/src/ConsumerService/Fgs.Consumer.API
RUN dotnet build Fgs.Consumer.API.csproj -c Release --no-restore

ENV ASPNETCORE_URLS=http://+:5007 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5007

HEALTHCHECK --interval=30s --timeout=5s --start-period=90s --retries=5 \
    CMD curl -fsS http://localhost:5007/health || exit 1

ENTRYPOINT ["dotnet", "run", "--no-build", "--no-launch-profile", "--project", "Fgs.Consumer.API.csproj", "--configuration", "Release", "--urls", "http://+:5007"]
