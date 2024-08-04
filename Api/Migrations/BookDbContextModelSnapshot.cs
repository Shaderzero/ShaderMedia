﻿// <auto-generated />
using System;
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(BookDbContext))]
    partial class BookDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Books")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Api.Data.Books.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("LastName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("LastName");

                    b.HasIndex("LastName", "FirstName", "MiddleName")
                        .IsUnique();

                    b.ToTable("Authors", "Books");
                });

            modelBuilder.Entity("Api.Data.Books.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly?>("Date")
                        .HasColumnType("date");

                    b.Property<bool>("Del")
                        .HasColumnType("boolean");

                    b.Property<string>("Ext")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<int>("File")
                        .HasColumnType("integer");

                    b.Property<int?>("LanguageId")
                        .HasColumnType("integer");

                    b.Property<int?>("LibRate")
                        .HasColumnType("integer");

                    b.Property<int?>("SeriesId")
                        .HasColumnType("integer");

                    b.Property<int?>("SeriesNo")
                        .HasColumnType("integer");

                    b.Property<int>("Size")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("ZipName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("SeriesId");

                    b.HasIndex("Title");

                    b.ToTable("Books", "Books");
                });

            modelBuilder.Entity("Api.Data.Books.BookAuthor", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("integer");

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.HasKey("BookId", "AuthorId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BookId");

                    b.ToTable("BookAuthor", "Books");
                });

            modelBuilder.Entity("Api.Data.Books.BookGenre", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("integer");

                    b.Property<int>("GenreId")
                        .HasColumnType("integer");

                    b.HasKey("BookId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("BookGenre", "Books");
                });

            modelBuilder.Entity("Api.Data.Books.BookKeyword", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("integer");

                    b.Property<int>("KeywordId")
                        .HasColumnType("integer");

                    b.HasKey("BookId", "KeywordId");

                    b.HasIndex("KeywordId");

                    b.ToTable("BookKeyword", "Books");
                });

            modelBuilder.Entity("Api.Data.Books.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Genres", "Books");
                });

            modelBuilder.Entity("Api.Data.Books.Keyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Keywords", "Books");
                });

            modelBuilder.Entity("Api.Data.Books.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("character varying(2)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Languages", "Books");
                });

            modelBuilder.Entity("Api.Data.Books.Series", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Series", "Books");
                });

            modelBuilder.Entity("Api.Data.Books.Book", b =>
                {
                    b.HasOne("Api.Data.Books.Language", "Language")
                        .WithMany("Books")
                        .HasForeignKey("LanguageId");

                    b.HasOne("Api.Data.Books.Series", "Series")
                        .WithMany("Books")
                        .HasForeignKey("SeriesId");

                    b.Navigation("Language");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("Api.Data.Books.BookAuthor", b =>
                {
                    b.HasOne("Api.Data.Books.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Books.Book", "Book")
                        .WithMany("Authors")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("Api.Data.Books.BookGenre", b =>
                {
                    b.HasOne("Api.Data.Books.Book", "Book")
                        .WithMany("Genres")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Books.Genre", "Genre")
                        .WithMany("Books")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Api.Data.Books.BookKeyword", b =>
                {
                    b.HasOne("Api.Data.Books.Book", "Book")
                        .WithMany("Keywords")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Books.Keyword", "Keyword")
                        .WithMany("Books")
                        .HasForeignKey("KeywordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Keyword");
                });

            modelBuilder.Entity("Api.Data.Books.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("Api.Data.Books.Book", b =>
                {
                    b.Navigation("Authors");

                    b.Navigation("Genres");

                    b.Navigation("Keywords");
                });

            modelBuilder.Entity("Api.Data.Books.Genre", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("Api.Data.Books.Keyword", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("Api.Data.Books.Language", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("Api.Data.Books.Series", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}