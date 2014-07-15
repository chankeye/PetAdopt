using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class Blog_Message_MappingMap : EntityTypeConfiguration<Blog_Message_Mapping>
    {
        public Blog_Message_MappingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MessageId, t.BlogId });

            // Properties
            this.Property(t => t.MessageId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.BlogId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Blog_Message_Mapping");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.BlogId).HasColumnName("BlogId");

            // Relationships
            this.HasRequired(t => t.Blog)
                .WithMany(t => t.Blog_Message_Mapping)
                .HasForeignKey(d => d.BlogId);

        }
    }
}
