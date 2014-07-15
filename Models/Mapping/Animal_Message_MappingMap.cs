using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class Animal_Message_MappingMap : EntityTypeConfiguration<Animal_Message_Mapping>
    {
        public Animal_Message_MappingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MessageId, t.AnimalId });

            // Properties
            this.Property(t => t.MessageId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AnimalId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Animal_Message_Mapping");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.AnimalId).HasColumnName("AnimalId");

            // Relationships
            this.HasRequired(t => t.Animal)
                .WithMany(t => t.Animal_Message_Mapping)
                .HasForeignKey(d => d.AnimalId);

        }
    }
}
