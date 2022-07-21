FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
RUN apt-get update && \
    apt-get install curl -y

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
CMD ["dotnet", "ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.dll", "--urls=http://0.0.0.0:80"]