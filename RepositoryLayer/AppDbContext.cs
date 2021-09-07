using Microsoft.EntityFrameworkCore;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.EntityMapper;

namespace RepositoryLayer
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

            base.OnModelCreating(modelBuilder);
        }

    }
}
