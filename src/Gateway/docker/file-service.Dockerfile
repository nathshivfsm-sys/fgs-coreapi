FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine
RUN apk add --no-cache curl
WORKDIR /src

COPY src/Shared/Directory.Build.props src/Shared/
COPY src/Shared/Kernel/Fgs.Kernel/Fgs.Kernel.csproj src/Shared/Kernel/Fgs.Kernel/
COPY src/Shared/Contracts/Fgs.Contracts/Fgs.Contracts.csproj src/Shared/Contracts/Fgs.Contracts/
COPY src/Shared/Persistence/Fgs.Persistence/Fgs.Persistence.csproj src/Shared/Persistence/Fgs.Persistence/
COPY src/Shared/Security/Fgs.Security/Fgs.Security.csproj src/Shared/Security/Fgs.Security/
COPY src/Shared/MultiTenancy/Fgs.MultiTenancy/Fgs.MultiTenancy.csproj src/Shared/MultiTenancy/Fgs.MultiTenancy/
COPY src/Shared/Foundation/Fgs.Foundation/Fgs.Foundation.csproj src/Shared/Foundation/Fgs.Foundation/
COPY src/Shared/Observability/Fgs.Observability/Fgs.Observability.csproj src/Shared/Observability/Fgs.Observability/
COPY src/FileService/Fgs.File.API/Fgs.File.API.csproj src/FileService/Fgs.File.API/
COPY src/FileService/Fgs.File.Application/Fgs.File.Application.csproj src/FileService/Fgs.File.Application/
COPY src/FileService/Fgs.File.Domain/Fgs.File.Domain.csproj src/FileService/Fgs.File.Domain/
COPY src/FileService/Fgs.File.Infrastructure/Fgs.File.Infrastructure.csproj src/FileService/Fgs.File.Infrastructure/

RUN dotnet restore src/FileService/Fgs.File.API/Fgs.File.API.csproj

COPY src/Shared/ src/Shared/
COPY src/FileService/ src/FileService/

WORKDIR /src/src/FileService/Fgs.File.API
RUN dotnet build Fgs.File.API.csproj -c Release --no-restore

ENV ASPNETCORE_URLS=http://+:5005 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5005

HEALTHCHECK --interval=30s --timeout=5s --start-period=30s --retries=3 \
    CMD curl -fsS http://localhost:5005/health || exit 1

ENTRYPOINT ["dotnet", "run", "--no-build", "--no-launch-profile", "--project", "Fgs.File.API.csproj", "--configuration", "Release", "--urls", "http://+:5005"]
