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
COPY src/JobService/Fgs.Job.API/Fgs.Job.API.csproj src/JobService/Fgs.Job.API/
COPY src/JobService/Fgs.Job.Application/Fgs.Job.Application.csproj src/JobService/Fgs.Job.Application/
COPY src/JobService/Fgs.Job.Domain/Fgs.Job.Domain.csproj src/JobService/Fgs.Job.Domain/
COPY src/JobService/Fgs.Job.Infrastructure/Fgs.Job.Infrastructure.csproj src/JobService/Fgs.Job.Infrastructure/

RUN dotnet restore src/JobService/Fgs.Job.API/Fgs.Job.API.csproj

COPY src/Shared/ src/Shared/
COPY src/JobService/ src/JobService/

WORKDIR /src/src/JobService/Fgs.Job.API
RUN dotnet build Fgs.Job.API.csproj -c Release --no-restore

ENV ASPNETCORE_URLS=http://+:5003 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5003

HEALTHCHECK --interval=30s --timeout=5s --start-period=20s --retries=3 \
    CMD curl -fsS http://localhost:5003/health || exit 1

ENTRYPOINT ["dotnet", "run", "--no-build", "--no-launch-profile", "--project", "Fgs.Job.API.csproj", "--configuration", "Release", "--urls", "http://+:5003"]
