using CarSales.Domain.Models;
using CarSales.Repository.EntityMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarSales.Repository
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientMap());

            modelBuilder.ApplyConfiguration(new CarMap());

            modelBuilder.Entity<Client>()
                .HasIndex(x => x.IdentityNumber)
                .IsUnique();

            modelBuilder.Entity<Car>()
                .HasIndex(x => x.VinCode)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Car> Cars { get; set; }

        public async override Task<int> SaveChangesAsync(CancellationToken token=default)
        {
            ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Deleted || e.State == EntityState.Modified || e.State == EntityState.Added)
                .ToList()
                .ForEach(e =>
                {
                    if(e.State == EntityState.Added)
                    {
                        e.Entity.CreationDate = DateTime.Now;
                    }
                    else if(e.State == EntityState.Modified)
                    {
                        e.Entity.ModifiedDate = DateTime.Now;
                    }
                    else if(e.State == EntityState.Deleted)
                    {
                        e.State = EntityState.Modified;
                        e.Entity.DeletedAt = DateTime.Now;
                    }
                });
            return await base.SaveChangesAsync();
        }

    }
}
