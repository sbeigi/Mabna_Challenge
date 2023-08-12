using Infrastructure.DataBaseWithNoRelation.CustomEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBaseWithNoRelation;

public partial class NoRelationDBContext : DbContext
{
    public virtual DbSet<IntrumentLastTrade> LastTrades { get; set; }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IntrumentLastTrade>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Name).HasColumnType("varchar(250)");
            entity.Property(e => e.Close).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DateEn).HasColumnType("datetime");
            entity.Property(e => e.High).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Low).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Open).HasColumnType("decimal(19, 4)");
        });
    }
}
