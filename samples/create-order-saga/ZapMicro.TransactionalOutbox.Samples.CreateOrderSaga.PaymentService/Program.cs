using System;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZapMicro.TransactionalOutbox.Configurations;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.DbContexts;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Handlers;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.OutboxMessages;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Repositories;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Services;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransactionalOutbox<PaymentServiceDbContext>(configBuilder =>
    configBuilder.ConfigureDequeueOutboxMessagesConfiguration(new DequeueOutboxMessagesConfiguration
    {
        EmptyQueueDelayInSeconds = 1
    }).ConfigureOutboxMessageHandler<OnPaymentFailedOutboxMessageHandler, OnPaymentFailedOutboxMessage>()
        .ConfigureOutboxMessageHandler<OnPaymentSucceededOutboxMessageHandler, OnPaymentSucceededOutboxMessage>());
builder.Services.AddDbContext<PaymentServiceDbContext>(options => options.UseSqlServer(
    Environment.GetEnvironmentVariable("PAYMENTS_DATABASE_CONNECTION_STRING") ?? string.Empty
    ));
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IEventHandlers, EventHandlers>();
builder.Services.AddScoped<IEventHandler, OnOrderCreatedEventHandler>();
builder.Services.AddHealthChecks();

QueueClient CreateOrderQueueClientFactory() => new QueueClient(Environment.GetEnvironmentVariable("QUEUE_STORAGE_CONNECTION_STRING"), Environment.GetEnvironmentVariable("CREATE_ORDER_QUEUE_NAME"));

QueueClient PaymentCompletedQueueClientFactory() => new QueueClient(Environment.GetEnvironmentVariable("QUEUE_STORAGE_CONNECTION_STRING"), Environment.GetEnvironmentVariable("PAYMENT_COMPLETED_QUEUE_NAME"));

builder.Services.AddHostedService(sp => new EventProcessingService(sp, CreateOrderQueueClientFactory));
builder.Services.AddScoped(sp => PaymentCompletedQueueClientFactory());

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
    var db = scope.ServiceProvider.GetRequiredService<PaymentServiceDbContext>();
    db.Database.Migrate();
    var queueClient1 = CreateOrderQueueClientFactory();
    await queueClient1.CreateIfNotExistsAsync();
    var queueClient2 = PaymentCompletedQueueClientFactory();
    await queueClient2.CreateIfNotExistsAsync();
}


app.Run();