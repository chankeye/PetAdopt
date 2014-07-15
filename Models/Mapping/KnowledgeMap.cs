using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class KnowledgeMap : EntityTypeConfiguration<Knowledge>
    {
        public KnowledgeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Knowledge");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.ClassId).HasColumnName("ClassId");
            this.Property(t => t.OperationId).HasColumnName("OperationId");

            // Relationships
            this.HasRequired(t => t.Class)
                .WithMany(t => t.Knowledges)
                .HasForeignKey(d => d.ClassId);
            this.HasRequired(t => t.OperationInfo)
                .WithMany(t => t.Knowledges)
                .HasForeignKey(d => d.OperationId);

        }
    }
}
