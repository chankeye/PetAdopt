using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Message1)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Message");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Message1).HasColumnName("Message");
            this.Property(t => t.OperationId).HasColumnName("OperationId");

            // Relationships
            this.HasMany(t => t.Activities)
                .WithMany(t => t.Messages)
                .Map(m =>
                    {
                        m.ToTable("Activity_Message_Mapping");
                        m.MapLeftKey("MessageId");
                        m.MapRightKey("ActivityId");
                    });

            this.HasMany(t => t.Animals)
                .WithMany(t => t.Messages)
                .Map(m =>
                    {
                        m.ToTable("Animal_Message_Mapping");
                        m.MapLeftKey("MessageId");
                        m.MapRightKey("AnimalId");
                    });

            this.HasMany(t => t.Asks)
                .WithMany(t => t.Messages)
                .Map(m =>
                    {
                        m.ToTable("Ask_Message_Mapping");
                        m.MapLeftKey("MessageId");
                        m.MapRightKey("AskId");
                    });

            this.HasMany(t => t.Blogs)
                .WithMany(t => t.Messages)
                .Map(m =>
                    {
                        m.ToTable("Blog_Message_Mapping");
                        m.MapLeftKey("MessageId");
                        m.MapRightKey("BlogId");
                    });

            this.HasRequired(t => t.OperationInfo)
                .WithMany(t => t.Messages)
                .HasForeignKey(d => d.OperationId);

        }
    }
}
