using CarSales.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.EntityMapper
{
    public class CarMap :IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(x => x.Id)
                .HasName("pk_CarId");

            builder.Property(x => x.Id).ValueGeneratedOnAdd()
                .HasColumnType("Int");

            builder.Property(x => x.ClientId)
                .HasColumnType("int")
                .IsRequired();
           
            builder.Property(x => x.Brand)
                .HasColumnType("NVARCHAR(20)")
                .IsRequired();
           
            builder.Property(x => x.Model)
                .HasColumnType("NVARCHAR(20)")
                .IsRequired();
           
            builder.Property(x => x.VinCode)
                .HasColumnType("VARCHAR(17)")
                .IsRequired();
           
            builder.Property(x => x.CarNumber)
                .HasColumnType("NVARCHAR(15)")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("Int")
                .IsRequired();

            builder.Property(x => x.ReleaseDate)
                .HasColumnType("DateTime")
                .IsRequired();

            builder.Property(x => x.StartedSale)
                .HasColumnType("DateTime")
                .IsRequired();
                
            builder.Property(x => x.FinishedSale)
                .HasColumnType("DateTime")
                .IsRequired();

            builder.Property(x => x.CreationDate)
                .HasColumnType("DateTime");

            builder.Property(x => x.ModifiedDate)
                .HasColumnType("DateTime");

            builder.Property(x => x.DeletedAt)
                .HasColumnType("DateTime");

            builder.Property(x => x.IsSold)
                .HasColumnType("bit");
        }
    }
}
