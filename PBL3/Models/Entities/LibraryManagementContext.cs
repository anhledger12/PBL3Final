﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PBL3.Models.Entities;

public partial class LibraryManagementContext : IdentityDbContext<UserIdentity>
{
    public LibraryManagementContext(DbContextOptions<LibraryManagementContext> options)
        : base(options)
    {

    }   

    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<ActionLog> ActionLogs { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookRentDetail> BookRentDetails { get; set; }

    public virtual DbSet<BookRental> BookRentals { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Notificate> Notificates { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    public virtual DbSet<NewsFeed> NewsFeeds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccName).HasName("PK__Account__092245520483F489");

            entity.ToTable("Account");

            entity.Property(e => e.AccName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Mssv)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MSSV");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserIdentity>(entity =>
        {
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.HasOne(d => d.account).WithOne(p => p.UserIdentity)
            .HasForeignKey<UserIdentity>(d => d.UserName)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Asp_Account");
        });

        modelBuilder.Entity<ActionLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ActionLo__3214EC0770136D3A");

            entity.ToTable("ActionLog");

            entity.Property(e => e.Acc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Time).HasColumnType("datetime");

            entity.HasOne(d => d.AccNavigation).WithMany(p => p.ActionLogs)
                .HasForeignKey(d => d.Acc)
                .HasConstraintName("FK__ActionLog__Acc__3E52440B");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.IdBook).HasName("PK__Book__2756CBDBD68BBF8F");

            entity.ToTable("Book");

            entity.Property(e => e.IdBook)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdTitle)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StateRent).HasDefaultValueSql("((0))");
            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.HasOne(d => d.IdTitleNavigation).WithMany(p => p.Books)
                .HasForeignKey(d => d.IdTitle)
                .HasConstraintName("FK__Book__IdTitle__5070F446");
        });

        modelBuilder.Entity<BookRentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Id_BookRentDetail");

            entity.ToTable("BookRentDetail");

            entity.Property(e => e.IdBook)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.StateReturn).HasDefaultValueSql("((0))");
            entity.Property(e => e.StateTake).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.IdBookNavigation).WithMany(p => p.BookRentDetails)
                .HasForeignKey(d => d.IdBook)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookRentD__IdBoo__571DF1D5");

            entity.HasOne(d => d.IdBookRentalNavigation).WithMany(p => p.BookRentDetails)
                .HasForeignKey(d => d.IdBookRental)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookRentD__IdBoo__5629CD9C");
        });

        modelBuilder.Entity<BookRental>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookRent__3214EC0743132A74");

            entity.ToTable("BookRental");

            entity.Property(e => e.AccApprove)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccSending)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeCreate).HasColumnType("datetime");
            entity.Property(e => e.StateSend).HasDefaultValueSql("((0))");  
            entity.Property(e => e.StateApprove).HasDefaultValueSql("((0))");
            entity.HasOne(d => d.AccApproveNavigation).WithMany(p => p.BookRentalAccApproveNavigations)
                .HasForeignKey(d => d.AccApprove)
                .HasConstraintName("FK__BookRenta__AccAp__412EB0B6");

            entity.HasOne(d => d.AccSendingNavigation).WithMany(p => p.BookRentalAccSendingNavigations)
                .HasForeignKey(d => d.AccSending)
                .HasConstraintName("FK__BookRenta__AccSe__4222D4EF");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07C7662509");
            entity.ToTable("Category");

        });

        modelBuilder.Entity<Notificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC077B06361B");

            entity.ToTable("Notificate");

            entity.Property(e => e.AccReceive)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeSending).HasColumnType("datetime");

            entity.HasOne(d => d.AccReceiveNavigation).WithMany(p => p.Notificates)
                .HasForeignKey(d => d.AccReceive)
                .HasConstraintName("FK__Notificat__AccRe__3B75D760");
        });
        modelBuilder.Entity<NewsFeed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NewsFeed__4214EC077P06361B");
            entity.ToTable("NewsFeed");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => e.IdTitle).HasName("PK__Title__2123011DEE7FEB85");

            entity.ToTable("Title");
            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.IdTitle)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.HasOne(d => d.Category).WithMany(p => p.IdTitles)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false)
                .HasConstraintName("FK__titlecate__5629CD9C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
