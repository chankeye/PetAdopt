using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class BlogMap : EntityTypeConfiguration<Blog>
    {
        public BlogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Message)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Blog");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.AnimalId).HasColumnName("AnimalId");
            this.Property(t => t.OperationId).HasColumnName("OperationId");
            this.Property(t => t.ClassId).HasColumnName("ClassId");

            // Relationships
            this.HasMany(t => t.Messages)
                .WithMany(t => t.Blogs)
                .Map(m =>
                    {
                        m.ToTable("Blog_Message_Mapping");
                        m.MapLeftKey("BlogId");
                        m.MapRightKey("MessageId");
                    });

            this.HasOptional(t => t.Animal)
                .WithMany(t => t.Blogs)
                .HasForeignKey(d => d.AnimalId)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Class)
                .WithMany(t => t.Blogs)
                .HasForeignKey(d => d.ClassId)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.OperationInfo)
                .WithMany(t => t.Blogs)
                .HasForeignKey(d => d.OperationId)
                .WillCascadeOnDelete(false);
        }
    }
}
