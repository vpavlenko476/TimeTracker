﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TimeTracker.Data;

#nullable disable

namespace TimeTracker.Data.Migrations
{
    [DbContext(typeof(TimeTrackerContext))]
    partial class TimeTrackerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TimeTracker.Data.Entities.JiraItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("JiraItems");
                });

            modelBuilder.Entity("TimeTracker.Data.Entities.WorkingTimePeriod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Begin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("End")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean");

                    b.Property<Guid>("JiraItemId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("JiraItemId");

                    b.ToTable("WorkingTimePeriods");
                });

            modelBuilder.Entity("TimeTracker.Data.Entities.WorkingTimePeriod", b =>
                {
                    b.HasOne("TimeTracker.Data.Entities.JiraItem", "JiraItem")
                        .WithMany()
                        .HasForeignKey("JiraItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JiraItem");
                });
#pragma warning restore 612, 618
        }
    }
}