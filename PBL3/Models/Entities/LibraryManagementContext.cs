using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PBL3.Models.Entities;

public partial class LibraryManagementContext : DbContext
{
    public LibraryManagementContext()
    {
    }

    public LibraryManagementContext(DbContextOptions<LibraryManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountLogin> AccountLogins { get; set; }

    public virtual DbSet<ActionLog> ActionLogs { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookRentDetail> BookRentDetails { get; set; }

    public virtual DbSet<BookRental> BookRentals { get; set; }

    public virtual DbSet<Hashtag> Hashtags { get; set; }

    public virtual DbSet<Notificate> Notificates { get; set; }

    public virtual DbSet<Republish> Republishes { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=LibraryManagement");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccName).HasName("PK__Account__092245520483F489");

            entity.ToTable("Account");

            entity.Property(e => e.AccName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cccd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CCCD");
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AccountLogin>(entity =>
        {
            entity.HasKey(e => e.AccName).HasName("PK__AccountL__09224552BE35A89A");

            entity.ToTable("AccountLogin");

            entity.Property(e => e.AccName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PassHashCode).IsUnicode(false);

            entity.HasOne(d => d.AccNameNavigation).WithOne(p => p.AccountLogin)
                .HasForeignKey<AccountLogin>(d => d.AccName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AccountLo__Permi__38996AB5");
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

            entity.HasOne(d => d.IdTitleNavigation).WithMany(p => p.Books)
                .HasForeignKey(d => d.IdTitle)
                .HasConstraintName("FK__Book__IdTitle__5070F446");
        });

        modelBuilder.Entity<BookRentDetail>(entity =>
        {
            entity.HasKey(e => new { e.IdBookRental, e.IdBook }).HasName("PK_IdBookRent_IdBook");

            entity.ToTable("BookRentDetail");

            entity.Property(e => e.IdBook)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.StateApprove).HasDefaultValueSql("((0))");
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
            entity.Property(e => e.TimeCreate).HasColumnType("date");

            entity.HasOne(d => d.AccApproveNavigation).WithMany(p => p.BookRentalAccApproveNavigations)
                .HasForeignKey(d => d.AccApprove)
                .HasConstraintName("FK__BookRenta__AccAp__412EB0B6");

            entity.HasOne(d => d.AccSendingNavigation).WithMany(p => p.BookRentalAccSendingNavigations)
                .HasForeignKey(d => d.AccSending)
                .HasConstraintName("FK__BookRenta__AccSe__4222D4EF");
        });

        modelBuilder.Entity<Hashtag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hashtag__3214EC07C7662509");

            entity.ToTable("Hashtag");

            entity.HasMany(d => d.IdTitles).WithMany(p => p.IdHashtags)
                .UsingEntity<Dictionary<string, object>>(
                    "HashtagTitle",
                    r => r.HasOne<Title>().WithMany()
                        .HasForeignKey("IdTitle")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Hashtag_t__IdTit__4CA06362"),
                    l => l.HasOne<Hashtag>().WithMany()
                        .HasForeignKey("IdHashtag")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Hashtag_t__IdHas__4BAC3F29"),
                    j =>
                    {
                        j.HasKey("IdHashtag", "IdTitle").HasName("PK_ID");
                        j.ToTable("Hashtag_title");
                        j.IndexerProperty<string>("IdTitle")
                            .HasMaxLength(50)
                            .IsUnicode(false);
                    });
        });

        modelBuilder.Entity<Notificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC077B06361B");

            entity.ToTable("Notificate");

            entity.Property(e => e.AccReceive)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeSending).HasColumnType("date");

            entity.HasOne(d => d.AccReceiveNavigation).WithMany(p => p.Notificates)
                .HasForeignKey(d => d.AccReceive)
                .HasConstraintName("FK__Notificat__AccRe__3B75D760");
        });

        modelBuilder.Entity<Republish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Republis__3214EC076ADD85DF");

            entity.ToTable("Republish");

            entity.Property(e => e.TimeOfRep).HasColumnType("datetime");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => e.IdTitle).HasName("PK__Title__2123011DEE7FEB85");

            entity.ToTable("Title");

            entity.Property(e => e.IdTitle)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReleaseDate).HasColumnType("date");

            entity.HasOne(d => d.IdRepublishNavigation).WithMany(p => p.Titles)
                .HasForeignKey(d => d.IdRepublish)
                .HasConstraintName("FK__Title__IdRepubli__48CFD27E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
