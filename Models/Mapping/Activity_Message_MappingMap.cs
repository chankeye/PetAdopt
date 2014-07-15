using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class Activity_Message_MappingMap : EntityTypeConfiguration<Activity_Message_Mapping>
    {
        public Activity_Message_MappingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MessageId, t.ActivityId });

            // Properties
            this.Property(t => t.MessageId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ActivityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Activity_Message_Mapping");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.ActivityId).HasColumnName("ActivityId");

            // Relationships
            this.HasRequired(t => t.Activity)
                .WithMany(t => t.Activity_Message_Mapping)
                .HasForeignKey(d => d.ActivityId);

        }
    }
}
