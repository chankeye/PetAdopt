using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class AnimalMap : EntityTypeConfiguration<Animal>
    {
        public AnimalMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CoverPoto)
                .HasMaxLength(100);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Introduction)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Address)
                .HasMaxLength(30);

            this.Property(t => t.Phone)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Animal");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CoverPoto).HasColumnName("CoverPoto");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Introduction).HasColumnName("Introduction");
            this.Property(t => t.Age).HasColumnName("Age");
            this.Property(t => t.SheltersId).HasColumnName("SheltersId");
            this.Property(t => t.AreaId).HasColumnName("AreaId");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.StatusId).HasColumnName("StatusId");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.OperationId).HasColumnName("OperationId");
            this.Property(t => t.ClassId).HasColumnName("ClassId");

            // Relationships
            this.HasOptional(t => t.Area)
                .WithMany(t => t.Animals)
                .HasForeignKey(d => d.AreaId);
            this.HasRequired(t => t.Class)
                .WithMany(t => t.Animals)
                .HasForeignKey(d => d.ClassId);
            this.HasRequired(t => t.OperationInfo)
                .WithMany(t => t.Animals)
                .HasForeignKey(d => d.OperationId);
            this.HasOptional(t => t.Shelter)
                .WithMany(t => t.Animals)
                .HasForeignKey(d => d.SheltersId);
            this.HasRequired(t => t.Status)
                .WithMany(t => t.Animals)
                .HasForeignKey(d => d.StatusId);

        }
    }
}
