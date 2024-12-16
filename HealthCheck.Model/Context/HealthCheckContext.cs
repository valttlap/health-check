using System;
using System.Collections.Generic;

using HealthCheck.Model.Entities;

using Microsoft.EntityFrameworkCore;

namespace HealthCheck.Model.Context;

public partial class HealthCheckContext : DbContext
{
    public HealthCheckContext(DbContextOptions<HealthCheckContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<SessionUser> SessionUsers { get; set; }

    public virtual DbSet<Vote> Votes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pkey");

            entity.ToTable("category", "health_check");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ExampleBadEn).HasColumnName("example_bad_en");
            entity.Property(e => e.ExampleBadFi).HasColumnName("example_bad_fi");
            entity.Property(e => e.ExampleGoodEn).HasColumnName("example_good_en");
            entity.Property(e => e.ExampleGoodFi).HasColumnName("example_good_fi");
            entity.Property(e => e.TitleEn).HasColumnName("title_en");
            entity.Property(e => e.TitleFi).HasColumnName("title_fi");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("session_pkey");

            entity.ToTable("session", "health_check");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CurrentCategoryId)
                .HasDefaultValue(1)
                .HasColumnName("current_category_id");

            entity.HasOne(d => d.CurrentCategory).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.CurrentCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("session_current_category_id_fkey");
        });

        modelBuilder.Entity<SessionUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("session_user_pkey");

            entity.ToTable("session_user", "health_check");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.SessionId).HasColumnName("session_id");

            entity.HasOne(d => d.Session).WithMany(p => p.SessionUsers)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("session_user_session_id_fkey");
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vote_pkey");

            entity.ToTable("vote", "health_check");

            entity.HasIndex(e => new { e.CategoryId, e.UserId, e.SessionId }, "vote_category_id_user_id_session_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VoteValue).HasColumnName("vote_value");

            entity.HasOne(d => d.Category).WithMany(p => p.Votes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("vote_category_id_fkey");

            entity.HasOne(d => d.Session).WithMany(p => p.Votes)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("vote_session_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Votes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("vote_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
