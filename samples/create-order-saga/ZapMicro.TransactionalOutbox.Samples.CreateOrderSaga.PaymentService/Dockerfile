FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
RUN apt-get update && apt-get -y install curl
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS sdk
RUN dotnet tool install dotnet-ef -g
ENV PATH=${PATH}:/root/.dotnet/tools

FROM sdk as build
WORKDIR /src
COPY . .
WORKDIR "/src/samples/create-order-saga/ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService"
RUN dotnet build "ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.csproj" -c Release

FROM build AS publish
RUN dotnet publish "ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
HEALTHCHECK CMD curl --fail http://localhost/healthcheck || exit 1
CMD ["dotnet", "ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.dll", "--urls=http://0.0.0.0:80"]