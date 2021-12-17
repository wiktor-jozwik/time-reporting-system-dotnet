﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NtrTrs;

namespace NtrTrs.Migrations
{
    [DbContext(typeof(NtrTrsContext))]
    [Migration("20211216203653_AddUserToMonthEntries")]
    partial class AddUserToMonthEntries
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("NtrTrs.AcceptedEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ActivityId")
                        .HasColumnType("integer");

                    b.Property<int?>("MonthEntryId")
                        .HasColumnType("integer");

                    b.Property<int>("Time")
                        .HasColumnType("integer")
                        .HasColumnName("time");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("MonthEntryId");

                    b.ToTable("accepted_entries");
                });

            modelBuilder.Entity("NtrTrs.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("active");

                    b.Property<int>("Budget")
                        .HasColumnType("integer")
                        .HasColumnName("budget");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("activities");
                });

            modelBuilder.Entity("NtrTrs.Entry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ActivityId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int?>("MonthEntryId")
                        .HasColumnType("integer");

                    b.Property<string>("Subcode")
                        .HasColumnType("text")
                        .HasColumnName("subcode");

                    b.Property<int>("Time")
                        .HasColumnType("integer")
                        .HasColumnName("time");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("MonthEntryId");

                    b.HasIndex("UserId");

                    b.ToTable("entries");
                });

            modelBuilder.Entity("NtrTrs.MonthEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date");

                    b.Property<bool>("Frozen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("frozen");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("month_entries");
                });

            modelBuilder.Entity("NtrTrs.Subactivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ActivityId")
                        .HasColumnType("integer");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.ToTable("subactivities");
                });

            modelBuilder.Entity("NtrTrs.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("LoggedIn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("logged_in");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("NtrTrs.AcceptedEntry", b =>
                {
                    b.HasOne("NtrTrs.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId");

                    b.HasOne("NtrTrs.MonthEntry", null)
                        .WithMany("Accepted")
                        .HasForeignKey("MonthEntryId");

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("NtrTrs.Activity", b =>
                {
                    b.HasOne("NtrTrs.User", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("NtrTrs.Entry", b =>
                {
                    b.HasOne("NtrTrs.Activity", "Activity")
                        .WithMany("Entries")
                        .HasForeignKey("ActivityId");

                    b.HasOne("NtrTrs.MonthEntry", "MonthEntry")
                        .WithMany("Entries")
                        .HasForeignKey("MonthEntryId");

                    b.HasOne("NtrTrs.User", "User")
                        .WithMany("Entries")
                        .HasForeignKey("UserId");

                    b.Navigation("Activity");

                    b.Navigation("MonthEntry");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NtrTrs.MonthEntry", b =>
                {
                    b.HasOne("NtrTrs.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NtrTrs.Subactivity", b =>
                {
                    b.HasOne("NtrTrs.Activity", null)
                        .WithMany("Subactivities")
                        .HasForeignKey("ActivityId");
                });

            modelBuilder.Entity("NtrTrs.Activity", b =>
                {
                    b.Navigation("Entries");

                    b.Navigation("Subactivities");
                });

            modelBuilder.Entity("NtrTrs.MonthEntry", b =>
                {
                    b.Navigation("Accepted");

                    b.Navigation("Entries");
                });

            modelBuilder.Entity("NtrTrs.User", b =>
                {
                    b.Navigation("Entries");
                });
#pragma warning restore 612, 618
        }
    }
}
