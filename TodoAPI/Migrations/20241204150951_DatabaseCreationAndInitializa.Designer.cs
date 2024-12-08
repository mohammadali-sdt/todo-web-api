﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository;

#nullable disable

namespace TodoAPI.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20241204150951_DatabaseCreationAndInitializa")]
    partial class DatabaseCreationAndInitializa
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Entities.Models.Todo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasColumnName("TodoId");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDone")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Todos");

                    b.HasData(
                        new
                        {
                            Id = new Guid("6a241878-2e19-4115-982a-984983ad6b96"),
                            Description = "Some Description",
                            IsDone = false,
                            Title = "Title1",
                            UserId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
                        },
                        new
                        {
                            Id = new Guid("a82bdd43-ab9c-48b6-a433-eedc515215ab"),
                            Description = "Some Description 2",
                            IsDone = false,
                            Title = "Title2",
                            UserId = new Guid("134103d7-6c6a-4c1f-93e9-eb2f15367ca5")
                        },
                        new
                        {
                            Id = new Guid("6ad8dc78-2f22-477d-9665-cedad4d9880a"),
                            Description = "Some Description 3",
                            IsDone = true,
                            Title = "Title3",
                            UserId = new Guid("134103d7-6c6a-4c1f-93e9-eb2f15367ca5")
                        });
                });

            modelBuilder.Entity("Entities.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasColumnName("UserId");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                            Age = 18,
                            Email = "foo@foo.com",
                            Name = "MR.Foo",
                            Password = "123456789",
                            Username = "foo"
                        },
                        new
                        {
                            Id = new Guid("134103d7-6c6a-4c1f-93e9-eb2f15367ca5"),
                            Age = 28,
                            Email = "boo@boo.com",
                            Name = "MR.Boo",
                            Password = "123456789",
                            Username = "boo"
                        });
                });

            modelBuilder.Entity("Entities.Models.Todo", b =>
                {
                    b.HasOne("Entities.Models.User", "User")
                        .WithMany("Todos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Entities.Models.User", b =>
                {
                    b.Navigation("Todos");
                });
#pragma warning restore 612, 618
        }
    }
}
