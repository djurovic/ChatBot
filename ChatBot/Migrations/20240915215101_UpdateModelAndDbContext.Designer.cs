﻿// <auto-generated />
using ChatBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatBot.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240915215101_UpdateModelAndDbContext")]
    partial class UpdateModelAndDbContext
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ChatBot.Models.MicroserviceCatalog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("MainLink")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("MicroserviceCatalogs");
                });

            modelBuilder.Entity("ChatBot.Models.MicroserviceMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("DateInterpretationNeeded")
                        .HasColumnType("bit");

                    b.Property<string>("MethodLink")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("MethodName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("MicroserviceCatalogId")
                        .HasColumnType("int");

                    b.Property<string>("QuestionExample")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("MicroserviceCatalogId");

                    b.ToTable("MicroserviceMethods");
                });

            modelBuilder.Entity("ChatBot.Models.MicroserviceMethod", b =>
                {
                    b.HasOne("ChatBot.Models.MicroserviceCatalog", "MicroserviceCatalog")
                        .WithMany("Methods")
                        .HasForeignKey("MicroserviceCatalogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MicroserviceCatalog");
                });

            modelBuilder.Entity("ChatBot.Models.MicroserviceCatalog", b =>
                {
                    b.Navigation("Methods");
                });
#pragma warning restore 612, 618
        }
    }
}
