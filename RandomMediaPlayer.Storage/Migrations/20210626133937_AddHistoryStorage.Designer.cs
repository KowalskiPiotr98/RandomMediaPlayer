﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RandomMediaPlayer.Storage;

namespace RandomMediaPlayer.Storage.Migrations
{
    [DbContext(typeof(StorageContext))]
    [Migration("20210626133937_AddHistoryStorage")]
    partial class AddHistoryStorage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.16");

            modelBuilder.Entity("RandomMediaPlayer.Storage.Models.UriHistory", b =>
                {
                    b.Property<string>("BasePath")
                        .HasColumnType("TEXT");

                    b.Property<string>("EntityName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("BasePath", "EntityName");

                    b.ToTable("UriHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
