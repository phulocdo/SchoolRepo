﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SchoolRepo.Data;
using System;

namespace SchoolRepo.Migrations
{
    [DbContext(typeof(RepoDBContext))]
    [Migration("20180421061629_initialMigration")]
    partial class initialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SchoolRepo.Models.Event", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("End");

                    b.Property<int>("GradeID");

                    b.Property<DateTime>("Start");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.HasIndex("GradeID");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("SchoolRepo.Models.File", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("FileName");

                    b.Property<string>("FilePath");

                    b.Property<int>("StudentID");

                    b.HasKey("ID");

                    b.HasIndex("StudentID");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("SchoolRepo.Models.Grade", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Level");

                    b.HasKey("ID");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("SchoolRepo.Models.Student", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("SchoolRepo.Models.User", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Grade");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SchoolRepo.Models.Event", b =>
                {
                    b.HasOne("SchoolRepo.Models.Grade", "Grade")
                        .WithMany("Events")
                        .HasForeignKey("GradeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SchoolRepo.Models.File", b =>
                {
                    b.HasOne("SchoolRepo.Models.Student", "Students")
                        .WithMany("Files")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
