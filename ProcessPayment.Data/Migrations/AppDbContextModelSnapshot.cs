﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProcessPayment.Data;

namespace ProcessPayment.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("ProcessPayment.Models.PaymentModel", b =>
                {
                    b.Property<string>("PaymentId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasPrecision(8, 2)
                        .HasColumnType("decimal");

                    b.Property<string>("CardHolder")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreditCardNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PaymentStateId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityCode")
                        .HasMaxLength(3)
                        .HasColumnType("TEXT");

                    b.HasKey("PaymentId");

                    b.HasIndex("PaymentStateId");

                    b.ToTable("PaymentModels");
                });

            modelBuilder.Entity("ProcessPayment.Models.PaymentState", b =>
                {
                    b.Property<string>("PaymentStateId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("TEXT");

                    b.HasKey("PaymentStateId");

                    b.ToTable("PaymentStates");
                });

            modelBuilder.Entity("ProcessPayment.Models.PaymentModel", b =>
                {
                    b.HasOne("ProcessPayment.Models.PaymentState", "PaymentState")
                        .WithMany("PaymentModels")
                        .HasForeignKey("PaymentStateId");

                    b.Navigation("PaymentState");
                });

            modelBuilder.Entity("ProcessPayment.Models.PaymentState", b =>
                {
                    b.Navigation("PaymentModels");
                });
#pragma warning restore 612, 618
        }
    }
}