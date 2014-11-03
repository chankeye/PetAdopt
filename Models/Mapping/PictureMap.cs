using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class PictureMap : EntityTypeConfiguration<Picture>
    {
        public PictureMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Position)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Picture");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Position).HasColumnName("Position");

            // Relationships
            this.HasMany(t => t.Shelters)
                .WithMany(t => t.Pictures)
                .Map(m =>
                    {
                        m.ToTable("Shelter_Picture_Mapping");
                        m.MapLeftKey("PictureId");
                        m.MapRightKey("ShelterId");
                    });
        }
    }
}
