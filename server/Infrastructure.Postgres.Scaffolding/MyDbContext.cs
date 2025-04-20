using System;
using System.Collections.Generic;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Postgres.Scaffolding;

public partial class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Devicelog> Devicelogs { get; set; }
    public virtual DbSet<Plant> Plants { get; set; }
    public virtual DbSet<UserPlant> UserPlants { get; set; }
    public virtual DbSet<WateringLog> WateringLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user", "plants");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Hash).HasColumnName("hash");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Salt).HasColumnName("salt");
        });

        modelBuilder.Entity<Plant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plant_pkey");

            entity.ToTable("plant", "plants");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PlantName).HasColumnName("plant_name");
            entity.Property(e => e.PlantType).HasColumnName("plant_type");
            entity.Property(e => e.MoistureLevel).HasColumnName("moisture_level");
            entity.Property(e => e.MoistureThreshold).HasColumnName("moisture_threshold");
            entity.Property(e => e.IsAutoWateringEnabled).HasColumnName("is_auto_watering_enabled");
        });

        modelBuilder.Entity<UserPlant>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PlantId }).HasName("user_plant_pkey");

            entity.ToTable("user_plant", "plants");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PlantId).HasColumnName("plant_id");
            entity.Property(e => e.LastWatered).HasColumnName("last_watered");
            entity.Property(e => e.IsOwner).HasColumnName("is_owner");

            entity.HasOne(up => up.User)
                .WithMany(u => u.UserPlants)
                .HasForeignKey(up => up.UserId);

            entity.HasOne(up => up.Plant)
                .WithMany(p => p.UserPlants)
                .HasForeignKey(up => up.PlantId);
        });

        modelBuilder.Entity<WateringLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("watering_log_pkey");

            entity.ToTable("watering_log", "plants");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PlantId).HasColumnName("plant_id");
            entity.Property(e => e.TriggeredByUserId).HasColumnName("triggered_by_user_id");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            entity.Property(e => e.Method).HasColumnName("method");
            entity.Property(e => e.MoistureBefore).HasColumnName("moisture_before");
            entity.Property(e => e.MoistureAfter).HasColumnName("moisture_after");

            entity.HasOne(e => e.Plant)
                .WithMany(p => p.WateringLogs)
                .HasForeignKey(e => e.PlantId);

            entity.HasOne(e => e.TriggeredByUser)
                .WithMany()
                .HasForeignKey(e => e.TriggeredByUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
