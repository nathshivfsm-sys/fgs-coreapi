FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY src/UserService/Fgs.User.API/Fgs.User.API.csproj src/UserService/Fgs.User.API/
COPY src/UserService/Fgs.User.Application/Fgs.User.Application.csproj src/UserService/Fgs.User.Application/
COPY src/UserService/Fgs.User.Domain/Fgs.User.Domain.csproj src/UserService/Fgs.User.Domain/
COPY src/UserService/Fgs.User.Infrastructure/Fgs.User.Infrastructure.csproj src/UserService/Fgs.User.Infrastructure/

RUN dotnet restore src/UserService/Fgs.User.API/Fgs.User.API.csproj

COPY src/UserService/ src/UserService/
WORKDIR /src/src/UserService/Fgs.User.API
RUN dotnet publish Fgs.User.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
RUN apk add --no-cache curl
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5001 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5001

COPY --from=build /app/publish .

HEALTHCHECK --interval=30s --timeout=5s --start-period=90s --retries=5 \
    CMD curl -fsS http://localhost:5001/health || exit 1

ENTRYPOINT ["dotnet", "Fgs.User.API.dll"]
