FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY src/WorkOrderService/Fgs.WorkOrder.API/Fgs.WorkOrder.API.csproj src/WorkOrderService/Fgs.WorkOrder.API/
COPY src/WorkOrderService/Fgs.WorkOrder.Application/Fgs.WorkOrder.Application.csproj src/WorkOrderService/Fgs.WorkOrder.Application/
COPY src/WorkOrderService/Fgs.WorkOrder.Domain/Fgs.WorkOrder.Domain.csproj src/WorkOrderService/Fgs.WorkOrder.Domain/
COPY src/WorkOrderService/Fgs.WorkOrder.Infrastructure/Fgs.WorkOrder.Infrastructure.csproj src/WorkOrderService/Fgs.WorkOrder.Infrastructure/

RUN dotnet restore src/WorkOrderService/Fgs.WorkOrder.API/Fgs.WorkOrder.API.csproj

COPY src/WorkOrderService/ src/WorkOrderService/
WORKDIR /src/src/WorkOrderService/Fgs.WorkOrder.API
RUN dotnet publish Fgs.WorkOrder.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
RUN apk add --no-cache curl
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5003 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5003

COPY --from=build /app/publish .

HEALTHCHECK --interval=30s --timeout=5s --start-period=20s --retries=3 \
    CMD curl -fsS http://localhost:5003/health || exit 1

ENTRYPOINT ["dotnet", "Fgs.WorkOrder.API.dll"]
