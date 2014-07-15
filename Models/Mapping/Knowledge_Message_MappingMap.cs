using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class Knowledge_Message_MappingMap : EntityTypeConfiguration<Knowledge_Message_Mapping>
    {
        public Knowledge_Message_MappingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MessageId, t.KnowledgeId });

            // Properties
            this.Property(t => t.MessageId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.KnowledgeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Knowledge_Message_Mapping");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.KnowledgeId).HasColumnName("KnowledgeId");

            // Relationships
            this.HasRequired(t => t.Knowledge)
                .WithMany(t => t.Knowledge_Message_Mapping)
                .HasForeignKey(d => d.KnowledgeId);
            this.HasRequired(t => t.Message)
                .WithMany(t => t.Knowledge_Message_Mapping)
                .HasForeignKey(d => d.KnowledgeId);

        }
    }
}
