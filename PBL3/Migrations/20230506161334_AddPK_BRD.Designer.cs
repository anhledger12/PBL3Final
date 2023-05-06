﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PBL3.Models.Entities;

#nullable disable

namespace PBL3.Migrations
{
    [DbContext(typeof(LibraryManagementContext))]
    [Migration("20230506161334_AddPK_BRD")]
    partial class AddPK_BRD
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HashtagTitle", b =>
                {
                    b.Property<int>("IdHashtag")
                        .HasColumnType("int");

                    b.Property<string>("IdTitle")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("IdHashtag", "IdTitle")
                        .HasName("PK_ID");

                    b.HasIndex("IdTitle");

                    b.ToTable("Hashtag_title", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("PBL3.Models.Entities.Account", b =>
                {
                    b.Property<string>("AccName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FullName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Mssv")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("MSSV");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.HasKey("AccName")
                        .HasName("PK__Account__092245520483F489");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("PBL3.Models.Entities.AccountLogin", b =>
                {
                    b.Property<string>("AccName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PassHashCode")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<bool?>("Permission")
                        .HasColumnType("bit");

                    b.HasKey("AccName")
                        .HasName("PK__AccountL__09224552BE35A89A");

                    b.ToTable("AccountLogin", (string)null);
                });

            modelBuilder.Entity("PBL3.Models.Entities.ActionLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Acc")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Time")
                        .HasColumnType("datetime");

                    b.HasKey("Id")
                        .HasName("PK__ActionLo__3214EC0770136D3A");

                    b.HasIndex("Acc");

                    b.ToTable("ActionLog", (string)null);
                });

            modelBuilder.Entity("PBL3.Models.Entities.Book", b =>
                {
                    b.Property<string>("IdBook")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("IdTitle")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool?>("StateRent")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("SubId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdBook")
                        .HasName("PK__Book__2756CBDBD68BBF8F");

                    b.HasIndex("IdTitle");

                    b.ToTable("Book", (string)null);
                });

            modelBuilder.Entity("PBL3.Models.Entities.BookRentDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("IdBook")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("IdBookRental")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime");

                    b.Property<bool?>("StateReturn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<bool?>("StateTake")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.HasKey("Id")
                        .HasName("PK_Id_BookRentDetail");

                    b.HasIndex("IdBook");

                    b.HasIndex("IdBookRental");

                    b.ToTable("BookRentDetail", (string)null);
                });

            modelBuilder.Entity("PBL3.Models.Entities.BookRental", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccApprove")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("AccSending")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool?>("StateApprove")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<bool?>("StateSend")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<DateTime?>("TimeApprove")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TimeCreate")
                        .HasColumnType("datetime");

                    b.HasKey("Id")
                        .HasName("PK__BookRent__3214EC0743132A74");

                    b.HasIndex("AccApprove");

                    b.HasIndex("AccSending");

                    b.ToTable("BookRental", (string)null);
                });

            modelBuilder.Entity("PBL3.Models.Entities.Hashtag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("NameHashTag")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK__Hashtag__3214EC07C7662509");

                    b.ToTable("Hashtag", (string)null);
                });

            modelBuilder.Entity("PBL3.Models.Entities.NewsFeed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK__NewsFeed__4214EC077P06361B");

                    b.ToTable("NewsFeed", (string)null);
                });

            modelBuilder.Entity("PBL3.Models.Entities.Notificate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccReceive")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("StateRead")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("TimeSending")
                        .HasColumnType("datetime");

                    b.HasKey("Id")
                        .HasName("PK__Notifica__3214EC077B06361B");

                    b.HasIndex("AccReceive");

                    b.ToTable("Notificate", (string)null);
                });

            modelBuilder.Entity("PBL3.Models.Entities.Title", b =>
                {
                    b.Property<string>("IdTitle")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NameBook")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameBookshelf")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameWriter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Publisher")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ReleaseYear")
                        .HasColumnType("int");

                    b.HasKey("IdTitle")
                        .HasName("PK__Title__2123011DEE7FEB85");

                    b.ToTable("Title", (string)null);
                });

            modelBuilder.Entity("PBL3.Models.UserIdentity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("HashtagTitle", b =>
                {
                    b.HasOne("PBL3.Models.Entities.Hashtag", null)
                        .WithMany()
                        .HasForeignKey("IdHashtag")
                        .IsRequired()
                        .HasConstraintName("FK__Hashtag_t__IdHas__4BAC3F29");

                    b.HasOne("PBL3.Models.Entities.Title", null)
                        .WithMany()
                        .HasForeignKey("IdTitle")
                        .IsRequired()
                        .HasConstraintName("FK__Hashtag_t__IdTit__4CA06362");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PBL3.Models.UserIdentity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PBL3.Models.UserIdentity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PBL3.Models.UserIdentity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PBL3.Models.UserIdentity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PBL3.Models.Entities.AccountLogin", b =>
                {
                    b.HasOne("PBL3.Models.Entities.Account", "AccNameNavigation")
                        .WithOne("AccountLogin")
                        .HasForeignKey("PBL3.Models.Entities.AccountLogin", "AccName")
                        .IsRequired()
                        .HasConstraintName("FK__AccountLo__Permi__38996AB5");

                    b.Navigation("AccNameNavigation");
                });

            modelBuilder.Entity("PBL3.Models.Entities.ActionLog", b =>
                {
                    b.HasOne("PBL3.Models.Entities.Account", "AccNavigation")
                        .WithMany("ActionLogs")
                        .HasForeignKey("Acc")
                        .HasConstraintName("FK__ActionLog__Acc__3E52440B");

                    b.Navigation("AccNavigation");
                });

            modelBuilder.Entity("PBL3.Models.Entities.Book", b =>
                {
                    b.HasOne("PBL3.Models.Entities.Title", "IdTitleNavigation")
                        .WithMany("Books")
                        .HasForeignKey("IdTitle")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Book__IdTitle__5070F446");

                    b.Navigation("IdTitleNavigation");
                });

            modelBuilder.Entity("PBL3.Models.Entities.BookRentDetail", b =>
                {
                    b.HasOne("PBL3.Models.Entities.Book", "IdBookNavigation")
                        .WithMany("BookRentDetails")
                        .HasForeignKey("IdBook")
                        .IsRequired()
                        .HasConstraintName("FK__BookRentD__IdBoo__571DF1D5");

                    b.HasOne("PBL3.Models.Entities.BookRental", "IdBookRentalNavigation")
                        .WithMany("BookRentDetails")
                        .HasForeignKey("IdBookRental")
                        .IsRequired()
                        .HasConstraintName("FK__BookRentD__IdBoo__5629CD9C");

                    b.Navigation("IdBookNavigation");

                    b.Navigation("IdBookRentalNavigation");
                });

            modelBuilder.Entity("PBL3.Models.Entities.BookRental", b =>
                {
                    b.HasOne("PBL3.Models.Entities.Account", "AccApproveNavigation")
                        .WithMany("BookRentalAccApproveNavigations")
                        .HasForeignKey("AccApprove")
                        .HasConstraintName("FK__BookRenta__AccAp__412EB0B6");

                    b.HasOne("PBL3.Models.Entities.Account", "AccSendingNavigation")
                        .WithMany("BookRentalAccSendingNavigations")
                        .HasForeignKey("AccSending")
                        .HasConstraintName("FK__BookRenta__AccSe__4222D4EF");

                    b.Navigation("AccApproveNavigation");

                    b.Navigation("AccSendingNavigation");
                });

            modelBuilder.Entity("PBL3.Models.Entities.Notificate", b =>
                {
                    b.HasOne("PBL3.Models.Entities.Account", "AccReceiveNavigation")
                        .WithMany("Notificates")
                        .HasForeignKey("AccReceive")
                        .HasConstraintName("FK__Notificat__AccRe__3B75D760");

                    b.Navigation("AccReceiveNavigation");
                });

            modelBuilder.Entity("PBL3.Models.Entities.Account", b =>
                {
                    b.Navigation("AccountLogin");

                    b.Navigation("ActionLogs");

                    b.Navigation("BookRentalAccApproveNavigations");

                    b.Navigation("BookRentalAccSendingNavigations");

                    b.Navigation("Notificates");
                });

            modelBuilder.Entity("PBL3.Models.Entities.Book", b =>
                {
                    b.Navigation("BookRentDetails");
                });

            modelBuilder.Entity("PBL3.Models.Entities.BookRental", b =>
                {
                    b.Navigation("BookRentDetails");
                });

            modelBuilder.Entity("PBL3.Models.Entities.Title", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
