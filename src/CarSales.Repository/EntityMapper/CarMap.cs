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
                .HasColumnName("Id")
                .HasColumnType("Int");

            builder.Property(x => x.ClientId)
                .HasColumnName("ClientId")
                .HasColumnType("int")
                .IsRequired();
           
            builder.Property(x => x.Brand)
                .HasColumnName("Brand")
                .HasColumnType("NVARCHAR(20)")
                .IsRequired();
           
            builder.Property(x => x.Model)
                .HasColumnName("Model")
                .HasColumnType("NVARCHAR(20)")
                .IsRequired();
           
            builder.Property(x => x.VinCode)
                .HasColumnName("VinCode")
                .HasColumnType("VARCHAR(17)")
                .IsRequired();
           
            builder.Property(x => x.CarNumber)
                .HasColumnName("CarNumber")
                .HasColumnType("NVARCHAR(15)")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("Price")
                .HasColumnType("Int")
                .IsRequired();

            builder.Property(x => x.ReleaseDate)
                .HasColumnName("ReleaseDate")
                .HasColumnType("DateTime")
                .IsRequired();

            builder.Property(x => x.StartedSale)
                .HasColumnName("StartedSaleDate")
                .HasColumnType("DateTime")
                .IsRequired();
                
            builder.Property(x => x.FinishedSale)
                .HasColumnName("FinishedSaleDate")
                .HasColumnType("DateTime")
                .IsRequired();

            builder.Property(x => x.CreationDate)
                .HasColumnName("CreationDate")
                .HasColumnType("DateTime");

            builder.Property(x => x.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .HasColumnType("DateTime");

            builder.Property(x => x.DeletedAt)
                .HasColumnName("DeletedAt")
                .HasColumnType("DateTime");

            builder.Property(x => x.IsSold)
                .HasColumnName("IsSold")
                .HasColumnType("bit");
        }
    }
}
