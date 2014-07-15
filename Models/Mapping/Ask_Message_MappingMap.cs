using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class Ask_Message_MappingMap : EntityTypeConfiguration<Ask_Message_Mapping>
    {
        public Ask_Message_MappingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MessageId, t.AskId });

            // Properties
            this.Property(t => t.MessageId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AskId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Ask_Message_Mapping");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.AskId).HasColumnName("AskId");

            // Relationships
            this.HasRequired(t => t.Ask)
                .WithMany(t => t.Ask_Message_Mapping)
                .HasForeignKey(d => d.AskId);

        }
    }
}
