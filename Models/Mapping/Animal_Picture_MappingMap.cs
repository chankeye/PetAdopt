using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class Animal_Picture_MappingMap : EntityTypeConfiguration<Animal_Picture_Mapping>
    {
        public Animal_Picture_MappingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.PictureId, t.AnimalId });

            // Properties
            this.Property(t => t.PictureId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AnimalId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Animal_Picture_Mapping");
            this.Property(t => t.PictureId).HasColumnName("PictureId");
            this.Property(t => t.AnimalId).HasColumnName("AnimalId");

            // Relationships
            this.HasRequired(t => t.Animal)
                .WithMany(t => t.Animal_Picture_Mapping)
                .HasForeignKey(d => d.AnimalId);

        }
    }
}
