﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetFirebase.Api.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetFirebase.Api.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NetFirebase.Api.Models.Domain.Permiso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Permisos", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Nombre = "ReadUsuario"
                        },
                        new
                        {
                            Id = 2,
                            Nombre = "WriteUsuario"
                        });
                });

            modelBuilder.Entity("NetFirebase.Api.Models.Domain.Producto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("text");

                    b.Property<string>("Nombre")
                        .HasColumnType("text");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("NetFirebase.Api.Models.Domain.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Cliente"
                        });
                });

            modelBuilder.Entity("NetFirebase.Api.Models.Domain.RolePermiso", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("PermisoId")
                        .HasColumnType("integer");

                    b.HasKey("RoleId", "PermisoId");

                    b.HasIndex("PermisoId");

                    b.ToTable("RolePermiso");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            PermisoId = 1
                        },
                        new
                        {
                            RoleId = 1,
                            PermisoId = 2
                        });
                });

            modelBuilder.Entity("NetFirebase.Api.Models.Domain.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirebaseId")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Usuarios", (string)null);
                });

            modelBuilder.Entity("NetFirebase.Api.Models.Domain.UsuarioRole", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("integer");

                    b.HasKey("RoleId", "UsuarioId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("UsuarioRole");
                });

            modelBuilder.Entity("NetFirebase.Api.Models.Domain.RolePermiso", b =>
                {
                    b.HasOne("NetFirebase.Api.Models.Domain.Permiso", null)
                        .WithMany()
                        .HasForeignKey("PermisoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NetFirebase.Api.Models.Domain.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetFirebase.Api.Models.Domain.UsuarioRole", b =>
                {
                    b.HasOne("NetFirebase.Api.Models.Domain.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NetFirebase.Api.Models.Domain.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
