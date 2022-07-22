// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.API;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.TestRunner;

Console.WriteLine("Starting create-order-saga test");
var client = RestEase.RestClient.For<IOrderServiceClient>(Environment.GetEnvironmentVariable("ORDER_SERVICE_BASE_URL")!);

var createOrderRequest1 = new CreateOrderRequest
{
    Lines = new List<OrderLineDto>
    {
        new()
        {
            Adjustments = new List<AdjustmentDto>
            {
                new()
                {
                    Total = -5,
                    OfferId = "offer-1"
                }
            },
            ProductId = "product-1",
            ProductPrice = 10,
            ProductQuantity = 2
        }
    },
    Adjustments = Enumerable.Empty<AdjustmentDto>()
};
var createOrderRequest2 = new CreateOrderRequest
{
    Lines = new List<OrderLineDto>
    {
        new()
        {
            Adjustments = new List<AdjustmentDto>
            {
                new()
                {
                    Total = -50,
                    OfferId = "offer-1"
                }
            },
            ProductId = "product-1",
            ProductPrice = 10,
            ProductQuantity = 2
        }
    },
    Adjustments = Enumerable.Empty<AdjustmentDto>()
};

await CreateOrderAndCheckStatus(createOrderRequest1, "Confirmed");
await CreateOrderAndCheckStatus(createOrderRequest2, "Rejected");

async Task CreateOrderAndCheckStatus(CreateOrderRequest request, string status)
{
    var justCreatedOrder = await client.CreateOrderAsync(request);
    await Task.Delay(TimeSpan.FromSeconds(5));
    var updatedOrder = await client.GetOrder(justCreatedOrder.Id);
    if (updatedOrder.Status!=status) throw new Exception($"Expected order status to be {status}");  
};

Console.WriteLine("create-order-saga test terminated successfully");