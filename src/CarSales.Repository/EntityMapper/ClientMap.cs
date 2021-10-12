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
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.IdentityNumber)
                .HasColumnType("VARCHAR(11)")             
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


        }
    }
}
