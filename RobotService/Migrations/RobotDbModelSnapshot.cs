﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RobotService.Migrations
{
    [DbContext(typeof(RobotDb))]
    partial class RobotDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CleaningResult", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("Commands")
                        .HasColumnType("integer")
                        .HasColumnName("commands");

                    b.Property<double>("Duration")
                        .HasColumnType("double precision")
                        .HasColumnName("duration");

                    b.Property<int>("Result")
                        .HasColumnType("integer")
                        .HasColumnName("result");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("timestamp");

                    b.HasKey("ID")
                        .HasName("pk_cleaning_results");

                    b.ToTable("cleaning_results", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
