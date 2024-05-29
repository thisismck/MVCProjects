using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LocalizationFromDB.Data;

public partial class MvcprojectsContext : DbContext
{
    public MvcprojectsContext()
    {
    }

    public MvcprojectsContext(DbContextOptions<MvcprojectsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LocalizationCulture> LocalizationCultures { get; set; }

    public virtual DbSet<LocalizationResource> LocalizationResources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LocalizationCulture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Localiza__3214EC07B7B261A4");

            entity.HasIndex(e => e.CultureCode, "UQ__Localiza__A710808ED84E653D").IsUnique();

            entity.Property(e => e.CultureCode).HasMaxLength(10);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
        });

        modelBuilder.Entity<LocalizationResource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Localiza__3214EC070D0ADEB5");

            entity.Property(e => e.Culture).HasMaxLength(10);
            entity.Property(e => e.ResourceKey).HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
