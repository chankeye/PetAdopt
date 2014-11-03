using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class AskMap : EntityTypeConfiguration<Ask>
    {
        public AskMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Ask");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.ClassId).HasColumnName("ClassId");
            this.Property(t => t.OperationId).HasColumnName("OperationId");

            // Relationships
            this.HasMany(t => t.Messages)
                .WithMany(t => t.Asks)
                .Map(m =>
                    {
                        m.ToTable("Ask_Message_Mapping");
                        m.MapLeftKey("AskId");
                        m.MapRightKey("MessageId");
                    });

            this.HasRequired(t => t.Class)
                .WithMany(t => t.Asks)
                .HasForeignKey(d => d.ClassId)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.OperationInfo)
                .WithMany(t => t.Asks)
                .HasForeignKey(d => d.OperationId)
                .WillCascadeOnDelete(false);
        }
    }
}
