﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.DbContexts;

#nullable disable

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Migrations
{
    [DbContext(typeof(OrderServiceDbContext))]
    partial class OrderServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ZapMicro.TransactionalOutbox.Entities.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Payload")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities.Adjustment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OfferId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OrderLineId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("OrderLineId");

                    b.ToTable("Adjustments");
                });

            modelBuilder.Entity("ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities.OrderLine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ProductPrice")
                        .HasColumnType("float");

                    b.Property<long>("ProductQuantity")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderLines");
                });

            modelBuilder.Entity("ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities.Adjustment", b =>
                {
                    b.HasOne("ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities.Order", null)
                        .WithMany("Adjustments")
                        .HasForeignKey("OrderId");

                    b.HasOne("ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities.OrderLine", null)
                        .WithMany("Adjustments")
                        .HasForeignKey("OrderLineId");
                });

            modelBuilder.Entity("ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities.OrderLine", b =>
                {
                    b.HasOne("ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities.Order", null)
                        .WithMany("Lines")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities.Order", b =>
                {
                    b.Navigation("Adjustments");

                    b.Navigation("Lines");
                });

            modelBuilder.Entity("ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities.OrderLine", b =>
                {
                    b.Navigation("Adjustments");
                });
#pragma warning restore 612, 618
        }
    }
}
