﻿// <auto-generated />
using System;
using BetControlAuthentication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BetControlAuthentication.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220612123309_BookeId")]
    partial class BookeId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("BetControlAuthentication.Models.Bet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BookieId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Locked")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Odds")
                        .HasColumnType("REAL");

                    b.Property<double>("Result")
                        .HasColumnType("REAL");

                    b.Property<string>("Selection")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("SportId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Stake")
                        .HasColumnType("REAL");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("TipsterId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BookieId");

                    b.HasIndex("SportId");

                    b.HasIndex("TipsterId");

                    b.HasIndex("UserId");

                    b.ToTable("Bets");
                });

            modelBuilder.Entity("BetControlAuthentication.Models.Bookie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Bookies");
                });

            modelBuilder.Entity("BetControlAuthentication.Models.Sport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sports");
                });

            modelBuilder.Entity("BetControlAuthentication.Models.Tipster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Tipsters");
                });

            modelBuilder.Entity("BetControlAuthentication.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bio")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BetControlAuthentication.Models.Bet", b =>
                {
                    b.HasOne("BetControlAuthentication.Models.Bookie", "Bookie")
                        .WithMany("Bets")
                        .HasForeignKey("BookieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BetControlAuthentication.Models.Sport", "Sport")
                        .WithMany("Bets")
                        .HasForeignKey("SportId");

                    b.HasOne("BetControlAuthentication.Models.Tipster", "Tipster")
                        .WithMany("Bets")
                        .HasForeignKey("TipsterId");

                    b.HasOne("BetControlAuthentication.Models.User", null)
                        .WithMany("Bets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bookie");

                    b.Navigation("Sport");

                    b.Navigation("Tipster");
                });

            modelBuilder.Entity("BetControlAuthentication.Models.Bookie", b =>
                {
                    b.Navigation("Bets");
                });

            modelBuilder.Entity("BetControlAuthentication.Models.Sport", b =>
                {
                    b.Navigation("Bets");
                });

            modelBuilder.Entity("BetControlAuthentication.Models.Tipster", b =>
                {
                    b.Navigation("Bets");
                });

            modelBuilder.Entity("BetControlAuthentication.Models.User", b =>
                {
                    b.Navigation("Bets");
                });
#pragma warning restore 612, 618
        }
    }
}
