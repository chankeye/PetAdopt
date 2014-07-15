using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class Help_Message_MappingMap : EntityTypeConfiguration<Help_Message_Mapping>
    {
        public Help_Message_MappingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MessageId, t.HelpId });

            // Properties
            this.Property(t => t.MessageId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HelpId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Help_Message_Mapping");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.HelpId).HasColumnName("HelpId");

            // Relationships
            this.HasRequired(t => t.Help)
                .WithMany(t => t.Help_Message_Mapping)
                .HasForeignKey(d => d.HelpId);

        }
    }
}
