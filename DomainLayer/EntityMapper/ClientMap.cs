using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.EntityMapper
{
    public class ClientMap : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(x => x.Id)
                .HasName("pk_ClientId");

            builder.Property(x => x.Id).ValueGeneratedOnAdd()
                .HasColumnName("Id")
                .HasColumnType("Int");
            builder.Property(x => x.IdentityNumber)
                .HasColumnName("IdentityNumber")
                .HasColumnType("NVARCHAR(11)")
                .IsRequired();
            builder.Property(x => x.FirstName)
                .HasColumnName("FirstName")
                .HasColumnType("NVARCHAR(20)");
            builder.Property(x => x.SecondName)
                .HasColumnName("SecondName")
                .HasColumnType("NVARCHAR(30)");
            builder.Property(x => x.BirthDate)
                .HasColumnName("BirthDate")
                .HasColumnType("DateTime");
            builder.Property(x => x.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasColumnType("NVARCHAR(15)")
                .IsRequired();
            builder.Property(x => x.Email)
                .HasColumnName("Email")
                .HasColumnType("NVARCHAR(50)");
            builder.Property(x => x.Address)
                .HasColumnName("Address")
                .HasColumnType("NVARCHAR(50)");
            builder.Property(x => x.CreationDate)
                .HasColumnName("CreationDate")
                .HasColumnType("DateTime");
            builder.Property(x => x.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .HasColumnType("DateTime");
            builder.Property(x => x.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("bit");

        }
    }
}
