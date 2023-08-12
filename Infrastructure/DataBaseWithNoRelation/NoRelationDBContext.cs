using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBaseWithNoRelation;

public partial class NoRelationDBContext : DbContext
{
    public NoRelationDBContext()
    {
    }

    public NoRelationDBContext(DbContextOptions<NoRelationDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Instrument> Instruments { get; set; }

    public virtual DbSet<Trade> Trades { get; set; }

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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
