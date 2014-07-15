using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class News_Message_MappingMap : EntityTypeConfiguration<News_Message_Mapping>
    {
        public News_Message_MappingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MessageId, t.NewsId });

            // Properties
            this.Property(t => t.MessageId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NewsId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("News_Message_Mapping");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.NewsId).HasColumnName("NewsId");

            // Relationships
            this.HasRequired(t => t.News)
                .WithMany(t => t.News_Message_Mapping)
                .HasForeignKey(d => d.NewsId);

        }
    }
}
