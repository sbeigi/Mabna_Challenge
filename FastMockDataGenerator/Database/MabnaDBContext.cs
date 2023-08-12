using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FastMockDataGenerator.Database;

public partial class MabnaDBContext : DbContext
{
    public MabnaDBContext()
    {
    }

    public MabnaDBContext(DbContextOptions<MabnaDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Instrument> Instruments { get; set; }

    public virtual DbSet<Trade> Trades { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-3NV03L4;Initial Catalog=MabnaChallenge;Integrated Security=True; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Instrument>(entity =>
        {
            entity.ToTable("Instrument");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Trade>(entity =>
        {
            entity.ToTable("Trade");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Close).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DateEn).HasColumnType("datetime");
            entity.Property(e => e.High).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Low).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Open).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.Instrument).WithMany(p => p.Trades)
                .HasForeignKey(d => d.InstrumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Trade_Instrument");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
