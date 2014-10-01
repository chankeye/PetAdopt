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
                .HasMaxLength(30);

            this.Property(t => t.Phone)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.CoverPhoto)
                .HasMaxLength(100);

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
            this.HasMany(t => t.Messages)
                .WithMany(t => t.Shelters)
                .Map(m =>
                    {
                        m.ToTable("Shelter_Message_Mapping");
                        m.MapLeftKey("ShelterId");
                        m.MapRightKey("MessageId");
                    });

            this.HasMany(t => t.Pictures)
                .WithMany(t => t.Shelters)
                .Map(m =>
                    {
                        m.ToTable("Shelter_Picture_Mapping");
                        m.MapLeftKey("ShelterId");
                        m.MapRightKey("PictureId");
                    });

            this.HasRequired(t => t.Area)
                .WithMany(t => t.Shelters)
                .HasForeignKey(d => d.AreaId);
            this.HasRequired(t => t.OperationInfo)
                .WithMany(t => t.Shelters)
                .HasForeignKey(d => d.OperationId);

        }
    }
}
