using System;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZapMicro.TransactionalOutbox.Configurations;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.DbContexts;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Handlers;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.OutboxMessages;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Profiles;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Repositories;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Services;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddTransactionalOutbox<OrderServiceDbContext>(configBuilder =>
    configBuilder.ConfigureDequeueOutboxMessagesConfiguration(new DequeueOutboxMessagesConfiguration
    {
        EmptyQueueDelayInSeconds = 1
    }).ConfigureOutboxMessageHandler<OnOrderCreatedOutboxMessageHandler, OnOrderCreatedOutboxMessage>());
builder.Services.AddDbContext<OrderServiceDbContext>(options => options.UseSqlServer(
    Environment.GetEnvironmentVariable("ORDERS_DATABASE_CONNECTION_STRING") ?? string.Empty
    ));
builder.Services.AddAutoMapper(config => config.AddProfile(new OrderServiceProfile()));
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddHostedService<EventProcessingService>();
builder.Services.AddScoped<IEventHandlers, EventHandlers>();
builder.Services.AddScoped<IEventHandler, OnPaymentFailedEventHandler>();
builder.Services.AddScoped<IEventHandler, OnPaymentSuccededEventHandler>();
builder.Services.AddHealthChecks();

QueueClient CreateOrderQueueClientFactory() => new QueueClient(Environment.GetEnvironmentVariable("QUEUE_STORAGE_CONNECTION_STRING"), Environment.GetEnvironmentVariable("CREATE_ORDER_QUEUE_NAME"));

QueueClient PaymentCompletedQueueClientFactory() => new QueueClient(Environment.GetEnvironmentVariable("QUEUE_STORAGE_CONNECTION_STRING"), Environment.GetEnvironmentVariable("PAYMENT_COMPLETED_QUEUE_NAME"));

builder.Services.AddScoped(sp => new EventProcessingService(sp, PaymentCompletedQueueClientFactory));
builder.Services.AddScoped(sp => CreateOrderQueueClientFactory());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthcheck");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderServiceDbContext>();
    db.Database.Migrate();
    var queueClient1 = CreateOrderQueueClientFactory();
    await queueClient1.CreateIfNotExistsAsync();
    var queueClient2 = PaymentCompletedQueueClientFactory();
    await queueClient2.CreateIfNotExistsAsync();
}


app.Run();