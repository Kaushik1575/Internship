using ApprenticeshipManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ApprenticeshipManagement.Data;

public class InternshipDb : DbContext
{
    public InternshipDb(DbContextOptions<InternshipDb> options) : base(options)
    {
    }

    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Apprentice> Apprentices => Set<Apprentice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasIndex(a => a.Email).IsUnique();
        });

        modelBuilder.Entity<Apprentice>(entity =>
        {
            entity.HasIndex(a => a.Email).IsUnique();
            entity.HasIndex(a => a.ApprenticeId).IsUnique();
        });
    }
}
