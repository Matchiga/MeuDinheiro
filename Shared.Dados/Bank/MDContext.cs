using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Shared.Bank;

public class MDContext : IdentityDbContext<PersonWithAccess, AccessProfile, int>
{
    public DbSet<Revenues> Revenue { get; set; }
    public DbSet<Cost> Cost { get; set; }

    public string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MeuDinheiro;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    public MDContext(DbContextOptions<MDContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<IdentityUserLogin<int>>()
                    .HasKey(login => new { login.LoginProvider, login.ProviderKey });

        modelBuilder.Entity<IdentityUserRole<int>>()
            .HasKey(role => new { role.UserId, role.RoleId });

        modelBuilder.Entity<Cost>()
        .HasOne(d => d.Category)
        .WithMany()
        .HasForeignKey(d => d.CategoryId);
    }
}
