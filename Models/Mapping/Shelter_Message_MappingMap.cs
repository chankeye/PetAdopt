using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class Shelter_Message_MappingMap : EntityTypeConfiguration<Shelter_Message_Mapping>
    {
        public Shelter_Message_MappingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MessageId, t.ShelterId });

            // Properties
            this.Property(t => t.MessageId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ShelterId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Shelter_Message_Mapping");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.ShelterId).HasColumnName("ShelterId");

            // Relationships
            this.HasRequired(t => t.Shelter)
                .WithMany(t => t.Shelter_Message_Mapping)
                .HasForeignKey(d => d.ShelterId);

        }
    }
}
