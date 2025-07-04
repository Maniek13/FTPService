﻿// <auto-generated />
using Configuration.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Configuration.Migrations
{
    [DbContext(typeof(FTPServiceContextBase))]
    [Migration("20240225004514_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FTPServiceLibrary.Models.DbModels.ConfigurationDbModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId")
                        .IsUnique();

                    b.ToTable("FtpConfigurations");
                });

            modelBuilder.Entity("FTPServiceLibrary.Models.DbModels.FilesDbModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServiceActionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ServiceActionId");

                    b.ToTable("FtpFiles");
                });

            modelBuilder.Entity("FTPServiceLibrary.Models.DbModels.ServiceActionDbModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ActionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActionName")
                        .IsUnique();

                    b.HasIndex("ServiceId");

                    b.ToTable("FtpServicesActions");
                });

            modelBuilder.Entity("FTPServiceLibrary.Models.DbModels.ServicesPermisionsDbModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceName")
                        .IsUnique();

                    b.ToTable("ServicesPermisions");
                });

            modelBuilder.Entity("FTPServiceLibrary.Models.DbModels.ConfigurationDbModel", b =>
                {
                    b.HasOne("FTPServiceLibrary.Models.DbModels.ServicesPermisionsDbModel", "ServicesPermisions")
                        .WithOne("Configuration")
                        .HasForeignKey("FTPServiceLibrary.Models.DbModels.ConfigurationDbModel", "ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServicesPermisions");
                });

            modelBuilder.Entity("FTPServiceLibrary.Models.DbModels.FilesDbModel", b =>
                {
                    b.HasOne("FTPServiceLibrary.Models.DbModels.ServiceActionDbModel", "ServiceAction")
                        .WithMany("Files")
                        .HasForeignKey("ServiceActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceAction");
                });

            modelBuilder.Entity("FTPServiceLibrary.Models.DbModels.ServiceActionDbModel", b =>
                {
                    b.HasOne("FTPServiceLibrary.Models.DbModels.ServicesPermisionsDbModel", "ServicesPermisions")
                        .WithMany("ServicesActions")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServicesPermisions");
                });

            modelBuilder.Entity("FTPServiceLibrary.Models.DbModels.ServiceActionDbModel", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("FTPServiceLibrary.Models.DbModels.ServicesPermisionsDbModel", b =>
                {
                    b.Navigation("Configuration")
                        .IsRequired();

                    b.Navigation("ServicesActions");
                });
#pragma warning restore 612, 618
        }
    }
}
