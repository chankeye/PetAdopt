using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class Shelter_Picture_MappingMap : EntityTypeConfiguration<Shelter_Picture_Mapping>
    {
        public Shelter_Picture_MappingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.PictureId, t.ShelterId });

            // Properties
            this.Property(t => t.PictureId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ShelterId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Shelter_Picture_Mapping");
            this.Property(t => t.PictureId).HasColumnName("PictureId");
            this.Property(t => t.ShelterId).HasColumnName("ShelterId");

            // Relationships
            this.HasRequired(t => t.Shelter)
                .WithMany(t => t.Shelter_Picture_Mapping)
                .HasForeignKey(d => d.ShelterId);

        }
    }
}
