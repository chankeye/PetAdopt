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
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Message");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Message1).HasColumnName("Message");
            this.Property(t => t.OperationId).HasColumnName("OperationId");

            // Relationships
            this.HasMany(t => t.News)
                .WithMany(t => t.Messages)
                .Map(m =>
                    {
                        m.ToTable("News_Message_Mapping");
                        m.MapLeftKey("MessageId");
                        m.MapRightKey("NewsId");
                    });

            this.HasMany(t => t.Shelters)
                .WithMany(t => t.Messages)
                .Map(m =>
                    {
                        m.ToTable("Shelter_Message_Mapping");
                        m.MapLeftKey("MessageId");
                        m.MapRightKey("ShelterId");
                    });

            this.HasRequired(t => t.OperationInfo)
                .WithMany(t => t.Messages)
                .HasForeignKey(d => d.OperationId)
                .WillCascadeOnDelete(false);
        }
    }
}
