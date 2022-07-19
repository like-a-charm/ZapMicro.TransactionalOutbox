FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN dotnet tool install dotnet-ef -g
ENV PATH=${PATH}:/root/.dotnet/tools

FROM build as restore
WORKDIR /src
COPY . .
RUN ls
WORKDIR "/src/samples/create-order-saga/ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService"
CMD ["dotnet", "run", "--project", "ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.csproj", "--urls=http://0.0.0.0:80"]
