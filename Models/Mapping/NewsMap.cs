using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class NewsMap : EntityTypeConfiguration<News>
    {
        public NewsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(1000);

            this.Property(t => t.Url)
                .HasMaxLength(100);

            this.Property(t => t.CoverPhoto)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("News");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.AreaId).HasColumnName("AreaId");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.OperationId).HasColumnName("OperationId");
            this.Property(t => t.CoverPhoto).HasColumnName("CoverPhoto");

            // Relationships
            this.HasOptional(t => t.Area)
                .WithMany(t => t.News)
                .HasForeignKey(d => d.AreaId)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.OperationInfo)
                .WithMany(t => t.News)
                .HasForeignKey(d => d.OperationId)
                .WillCascadeOnDelete(false);
        }
    }
}
