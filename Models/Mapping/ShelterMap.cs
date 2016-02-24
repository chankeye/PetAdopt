using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class ShelterMap : EntityTypeConfiguration<Shelter>
    {
        public ShelterMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Introduction)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Url)
                .HasMaxLength(100);

            this.Property(t => t.Address)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Phone)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Shelters");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Introduction).HasColumnName("Introduction");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.AreaId).HasColumnName("AreaId");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.OperationId).HasColumnName("OperationId");
            this.Property(t => t.CoverPhoto).HasColumnName("CoverPhoto");

            // Relationships
            this.HasRequired(t => t.Area)
                .WithMany(t => t.Shelters)
                .HasForeignKey(d => d.AreaId);
            this.HasRequired(t => t.OperationInfo)
                .WithMany(t => t.Shelters)
                .HasForeignKey(d => d.OperationId);

        }
    }
}
