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
    public class ClientMap : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(x => x.Id)
                .HasName("pk_ClientId");

            builder.Property(x => x.Id).ValueGeneratedOnAdd()
                .HasColumnType("Int");

            builder.Property(x => x.IdentityNumber)
                .HasColumnType("NVARCHAR(11)")              
                .IsRequired();

            builder.Property(x => x.FirstName)
                .HasColumnType("NVARCHAR(20)");

            builder.Property(x => x.SecondName)
                .HasColumnType("NVARCHAR(30)");

            builder.Property(x => x.BirthDate)
                .HasColumnType("DateTime");

            builder.Property(x => x.PhoneNumber)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnType("NVARCHAR(50)");

            builder.Property(x => x.Address)
                .HasColumnType("NVARCHAR(50)");

            builder.Property(x => x.CreationDate)
                .HasColumnType("DateTime");

            builder.Property(x => x.ModifiedDate)
                .HasColumnType("DateTime");

            builder.Property(x => x.DeletedAt)
                .HasColumnType("DateTime");

        }
    }
}
